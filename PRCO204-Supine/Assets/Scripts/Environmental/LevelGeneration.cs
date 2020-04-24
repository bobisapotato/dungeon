using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	// one LevelManager object in the scene, containing this script
	// controls spawning of rooms from prefabs to create a level 

	// VARS
	public GameObject[] roomPrefabs;
	[SerializeField] public List<GameObject> roomsInScene = new List<GameObject>();
	public int maximumRooms;
	[SerializeField] private int totalRoomsSoFar = 1;
	[SerializeField] private int openPaths;
	public GameObject startRoomPrefab;
	public List<GameObject> openSpawnPts = new List<GameObject>();
	Quaternion startRot;
	Vector3 startSpawn;

	public List<GameObject> rooms_northDoor = new List<GameObject>();
	public List<GameObject> rooms_eastDoor = new List<GameObject>();
	public List<GameObject> rooms_southDoor = new List<GameObject>();
	public List<GameObject> rooms_westDoor = new List<GameObject>();

	public GameObject deadEndN;
	public GameObject deadEndE;
	public GameObject deadEndS;
	public GameObject deadEndW;
	public GameObject all4;

	public EnemyCountManager enemyCountMan;
	public bool startedEnemyCounter = false;
	private bool hasStoppedSpawning = false;

	public static bool hasTrapDoorSpawned = false;
	[SerializeField]
	private GameObject trapDoorPrefab;
	[SerializeField]
	private int spawnTrapDoorChance = 75;

	

	private void Start()
	{
		// to start, we just create the starter room at 0.0
		startSpawn = new Vector3(0, 0, 0);
		startRot = new Quaternion(0, 0, 0, 0);
		Instantiate(startRoomPrefab, startSpawn, startRot);
		populateRoomDirectionLists();
		openPaths = openSpawnPts.Count;

		roomsInScene.Add(startRoomPrefab);

		//BUILD LEVEL
		//InvokeRepeating("createLevel", 0.5f, 0.1f);
	}

	private void Update()
	{
		if (openSpawnPts.Count() == 0)
		{
			CancelInvoke("createLevel");
			hasStoppedSpawning = true;

			if (!startedEnemyCounter)
			{
				enemyCountMan.startUpEnemyCounter();
				startedEnemyCounter = true;
				
			}
		}

		if (openSpawnPts.Count() != 0 && hasStoppedSpawning)
		{
			InvokeRepeating("createLevel", 0.5f, 0.1f);
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			createLevel();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			InvokeRepeating("createLevel", 0.5f, 1f);
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			InvokeRepeating("createLevel", 0.5f, 0.1f);
		}


	}

	public void createLevel()
	{
		int index = Random.Range(0, openSpawnPts.Count - 1);
		if (openSpawnPts[index].GetComponent<RoomSpawnPoint>().checkSpawnIsOpen())
		{
			if (openSpawnPts[index].GetComponent<RoomSpawnPoint>().open)
			{
				pickHowToSpawnRoom(openSpawnPts[index].GetComponent<RoomSpawnPoint>());
			}
			else
			{
				openSpawnPts.Remove(openSpawnPts[index].GetComponent<RoomSpawnPoint>().gameObject);
			}
		}
		else
		{
			Debug.Log("Was wrongly marked as open, fixed now");
		}
	}
	

	public List<GameObject> getRoomsInScene()
	{
		return roomsInScene;
	}

	// Managing list of active spawn points
	#region
	

	public void refreshOpenSpawnList()
	{
		foreach(GameObject g in openSpawnPts)
		{
			g.GetComponent<RoomSpawnPoint>().checkSpawnIsOpen();
		}

		openPaths = openSpawnPts.Count;
	}
	public void addNewSpawnPt(GameObject g)
	{
		// when a new room spawn pt is instatiated, it should call this to add itself to the active list
		if (g.GetComponent<RoomSpawnPoint>().open == true)
		{
			openSpawnPts.Add(g);
			openPaths++;
		}

		// double check that point is open, it will remove itself if it isn't
		g.GetComponent<RoomSpawnPoint>().checkSpawnIsOpen();
		//Debug.Log(openSpawnPts.Count());
	}


	public void updateSpawnList()
	{

		GameObject[] tempSpawnArray = new GameObject[openPaths];
		// iterate through list removing any inactive
		foreach (GameObject g in openSpawnPts)
		{
			if (!g.GetComponent<RoomSpawnPoint>().open)
			{
				openSpawnPts.Remove(g);
			}
		}
	}

	public void removeFromSpawnList(GameObject g)
	{
		//Debug.Log("spawn point " + g.name + " is in list? = " + openSpawnPts.Contains(g));

		if (openSpawnPts.Contains(g))
		{
			// when a room is spawned, the relevant point becomes inactive, and this is called to remove it
			openSpawnPts.Remove(g);
			openPaths--;
			g.GetComponent<RoomSpawnPoint>().setSpawnInactive();
			//Debug.Log("has removed " + g.name);
		}

		//Debug.Log(openSpawnPts.Count());
	}



	#endregion

	// Manage roomsInScene

	#region
	public void addNewRoomToScene(GameObject g, RoomSpawnPoint spawn)
	{
		// adds Gamoebject to roomsInScene list
		roomsInScene.Add(g);
		totalRoomsSoFar++;
		spawn.setSpawnInactive();
		removeFromSpawnList(spawn.gameObject);
	}

	public void removeRoomFromScene(GameObject g)
	{
		// adds Gamoebject to roomsInScene list
		roomsInScene.Remove(g);
		totalRoomsSoFar--;

		refreshOpenSpawnList();
		
	}
	#endregion
	// Populate lists of room prefabs based on door directions
	private void populateRoomDirectionLists()
	{
		// populate the lists with every room that has a door in a given direction

		foreach(GameObject g in roomPrefabs)
		{
			Room tempRoom = g.GetComponent<Room>();

			if(tempRoom.nDoor)
			{
				rooms_northDoor.Add(g);
			}
			if(tempRoom.eDoor)
			{
				rooms_eastDoor.Add(g);
			}
			if(tempRoom.sDoor)
			{
				rooms_southDoor.Add(g);
			}
			if(tempRoom.wDoor)
			{
				rooms_westDoor.Add(g);
			}
		}
	}

	// Spawning of rooms

	public void pickHowToSpawnRoom(RoomSpawnPoint spawnPoint)
	{
		// selects whether room should be selected from list or be a dead end

		List<string> doorsRequired = new List<string>();
		List<string> doorsAvoided = new List<string>();

		string direction = spawnPoint.spawnDirection;


		// check sensors to see if any doors are needed 
		foreach (RoomSpawnSensor sensor in spawnPoint.GetComponentsInChildren<RoomSpawnSensor>())
		{

			//Debug.Log("Running iterate through sensors");


			//Debug.Log("Finds the original room at direction " + sensor.direction + " : " + sensor.checkMustHave());
			if (sensor.checkRoomWasFound() && sensor.checkMustHave())
			{
				doorsRequired.Add(sensor.getRequiredDoorDir());
				//Debug.Log("needs door in dir " + sensor.getRequiredDoorDir());
			}
			if (sensor.checkRoomWasFound() && sensor.checkMustNot())
			{
				doorsAvoided.Add(sensor.getRequiredDoorDir());
				//Debug.Log("avoid door in dir " + sensor.getRequiredDoorDir());
			}
		}


		if (totalRoomsSoFar + openPaths >= maximumRooms)
		{
			//Debug.Log("needs dead end");
			spawnDeadEnd(spawnPoint, doorsRequired, doorsAvoided);
		}
		else if(totalRoomsSoFar + openPaths < maximumRooms)
		{
			spawnRoomFromList(spawnPoint, doorsRequired, doorsAvoided);
		}
	}
	
	public void spawnRoomFromList(RoomSpawnPoint spawnPoint, List<string> requiredDirs, List<string> avoidedDirs)
	{
		// spawns in a random room at the given point
		// room must be selected from correct list so doors align

		GameObject roomToSpawn = null;

		if(requiredDirs.Count() == 0)
		{
			foreach(GameObject room in roomsInScene)
			{
				if(transform.position == spawnPoint.transform.position)
				{
					Debug.Log("Deleting room spec way");
					room.GetComponent<Room>().destroyThisRoom();
				}
			}
		}
		List<GameObject> roomsToChooseFrom = populateTempRoomList(requiredDirs, avoidedDirs);
		

		foreach(GameObject g in roomsToChooseFrom)
		{
			//Debug.Log("Potential room: " + g.name);
		}

		int index = Random.Range(0, roomsToChooseFrom.Count - 1);
		//Debug.Log("Index is " + index + " and roomsList is " + roomsToChooseFrom.Count);

		roomToSpawn = roomsToChooseFrom[index];

		
		// console output to return a room
		//Debug.Log("Room " + roomToSpawn.name + " has been selected to spawn from spawn point " + spawnPoint.gameObject.name);

		instantiateRoom(roomToSpawn, spawnPoint);
	}

	public void spawnDeadEnd(RoomSpawnPoint spawnPoint, List<string> requiredDirs, List<string> avoidedDirs)
	{
		// For now, if max limit is near, we spawn a dead end room. 
		// This can cause a few issues, as sometimes the new room links to two doors
		// (or more), resulting in doors that lead to no where. 
		// This needs to be considered and added to this section. 
		// If we can't think of anything better, as a quick fix we can just add all the corner room prefabs 
		// to the levelGen, as we have with dead ends, and add more if statements, but this is clunky af.


		if (requiredDirs.Count() != 0)
		{
			GameObject roomToSpawn = findDeadEnd(requiredDirs, avoidedDirs);

			instantiateRoom(roomToSpawn, spawnPoint);
		}
		
	}

	public void instantiateRoom(GameObject room, RoomSpawnPoint spawn)
	{
		if (room && spawn)
		{
			Vector3 tempTransform = spawn.transform.position;

			// spawn said room at that pos
			GameObject newRoom = Instantiate(room, tempTransform, startRot);

			addNewRoomToScene(room.gameObject, spawn);

			// Spawn 1 trap door.
			if (!hasTrapDoorSpawned)
			{
				if (totalRoomsSoFar < maximumRooms)
				{
					int rnd = Random.Range(0, 100);

					if (rnd >= spawnTrapDoorChance)
					{
						SpawnTrapDoor(newRoom);
					}
				}
				else
				{
					SpawnTrapDoor(newRoom);
				}
			}

			
		}
	}

	public List<GameObject> populateTempRoomList(List<string> requiredDirs, List<string> avoidedDirs)
	{
		List<GameObject> tempRoomList = new List<GameObject>();
		List<GameObject> secondTempRoomList = new List<GameObject>();

		if(requiredDirs.Count() == 0)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().closeGame();
		}

		// add all the doors with required directions to temp List
		if (requiredDirs.Contains("N"))
		{
			foreach(GameObject g in rooms_northDoor)
			{
				tempRoomList.Add(g);
			}
		}
		if (requiredDirs.Contains("E"))
		{
			foreach (GameObject g in rooms_eastDoor)
			{
				tempRoomList.Add(g);
			}
		}
		if (requiredDirs.Contains("S"))
		{
			foreach (GameObject g in rooms_southDoor)
			{
				tempRoomList.Add(g);
			}
		}
		if (requiredDirs.Contains("W"))
		{
			foreach (GameObject g in rooms_westDoor)
			{
				tempRoomList.Add(g);
			}
		}

		
		// remove any rooms with mustAvoid directions
		//foreach(GameObject g in secondTempRoomList)
		//{
		//	tempRoomList.Remove(g);
		//}

		// clear secondtemp list for reuse
		//secondTempRoomList.Clear();

		//remove any rooms that only have one/some of the needed directions
		if (requiredDirs.Contains("N"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if(!rooms_northDoor.Contains(g))
				{
					secondTempRoomList.Add(g);
				}
			}
		}
		if (requiredDirs.Contains("E"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (!rooms_eastDoor.Contains(g))
				{
					secondTempRoomList.Add(g);
				}
			}
		}
		if (requiredDirs.Contains("S"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (!rooms_southDoor.Contains(g))
				{
					secondTempRoomList.Add(g);
				}
			}
		}
		if (requiredDirs.Contains("W"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (!rooms_westDoor.Contains(g))
				{
					secondTempRoomList.Add(g);
				}
			}
		}

		// remove any rooms with doors in the mustAvoid list

		//Debug.Log("Added rooms to temp list successfully");

		if (avoidedDirs.Contains("N"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (g.GetComponent<Room>().nDoor)
				{
					secondTempRoomList.Add(g);
				}
			}
		}


		if (avoidedDirs.Contains("E"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (g.GetComponent<Room>().eDoor)
				{
					secondTempRoomList.Add(g);
				}
			}
		}
		if (avoidedDirs.Contains("S"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (g.GetComponent<Room>().sDoor)
				{
					secondTempRoomList.Add(g);
				}
			}
		}
		if (avoidedDirs.Contains("W"))
		{
			foreach (GameObject g in tempRoomList)
			{
				if (g.GetComponent<Room>().wDoor)
				{
					secondTempRoomList.Add(g);
				}
			}
		}

		foreach (GameObject g in secondTempRoomList)
		{
			tempRoomList.Remove(g);
		}
		//Debug.Log("removed rooms right");

		//Debug.Log("Suitable rooms " + tempRoomList.Count());
		return tempRoomList;
	}

	public GameObject findDeadEnd(List<string> requiredDirs, List<string> avoidedDirs)
	{
		GameObject perfectRoom = null;

		// only one door needed

		if(requiredDirs.Count() == 1)
		{
			if(requiredDirs[0] == "N")
			{
				perfectRoom = deadEndN;
			}
			if (requiredDirs[0] == "E")
			{
				perfectRoom = deadEndE;
			}
			if (requiredDirs[0] == "S")
			{
				perfectRoom = deadEndS;
			}
			if (requiredDirs[0] == "W")
			{
				perfectRoom = deadEndW;
			}
		}

		// if two doors needed

		if (requiredDirs.Count() == 2)
		{
			foreach (GameObject g in roomPrefabs)
			{
				Door[] doors = g.GetComponentsInChildren<Door>();

				if (doors.Count() == 2)
				{
					if (requiredDirs.Contains(doors[0].direction) && requiredDirs.Contains(doors[1].direction))
					{
						perfectRoom = g;
					}
				}
			}
		}

		// if 3 doors needed
		if (requiredDirs.Count() == 3)
		{
			foreach (GameObject g in roomPrefabs)
			{
				Door[] doors = g.GetComponentsInChildren<Door>();

				if (doors.Count() == 3)
				{
					if (requiredDirs.Contains(doors[0].direction) && requiredDirs.Contains(doors[1].direction) && requiredDirs.Contains(doors[2].direction))
					{
						perfectRoom = g;
					}
				}
			}
		}

		if (requiredDirs.Count() == 4)
		{
			perfectRoom = all4;
		}

		if(!perfectRoom )
		{
			Debug.Log("no dead end found. required dirs: " + requiredDirs.Count());
			if(requiredDirs.Count() == 2)
			{
				Debug.Log("dirs needed: " + requiredDirs[0] + requiredDirs[1]);
			}

		}
		return perfectRoom;
		
	}

	// Spawns the trap door prefab. 1 per level.
	void SpawnTrapDoor(GameObject room)
	{
		GameObject trapDoor = Instantiate(trapDoorPrefab, room.transform, false);

		hasTrapDoorSpawned = true;
	}

	
}
