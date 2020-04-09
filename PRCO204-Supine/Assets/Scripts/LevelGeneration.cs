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
	[SerializeField] private List<GameObject> roomsInScene = new List<GameObject>();
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

	



	private void Start()
	{
		// to start, we just create the starter room at 0.0
		startSpawn = new Vector3(0, 0, 0);
		startRot = new Quaternion(0, 0, 0, 0);
		Instantiate(startRoomPrefab, startSpawn, startRot);
		//populateSpawnPtList();
		populateRoomDirectionLists();
		openPaths = openSpawnPts.Count;

		roomsInScene.Add(startRoomPrefab);

		for(int x = 0; x < openPaths; x++)
		{

			spawnRoom(openSpawnPts[0].GetComponent<RoomSpawnPoint>());
		}
		


	}

	private void Update()
	{
		// spawn rooms one by one in update to bug check

		int index = Random.Range(0, openSpawnPts.Count - 1);

		if (Input.GetKeyDown(KeyCode.Q))
		{
			if(openSpawnPts[index].GetComponent<RoomSpawnPoint>().checkSpawnIsOpen())
			{
				if (openSpawnPts[index].GetComponent<RoomSpawnPoint>().open)
				{
					spawnRoom(openSpawnPts[index].GetComponent<RoomSpawnPoint>());
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
		if (Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log(openSpawnPts.Count + "spawns in list on click T");
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			refreshOpenSpawnList();
		}



		//Debug.Log(openPaths);
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
	}
	public void addNewSpawnPt(GameObject g)
	{
		//Debug.Log("Adding new spawn to list in top method - now list is " + openPaths);
		//Debug.Log("when spawn " + g.name + " is added, its open value is " + g.GetComponent<RoomSpawnPoint>().open);
		// when a new room spawn pt is instatiated, it should call this to add itself to the active list
		if (g.GetComponent<RoomSpawnPoint>().open == true)
		{
			openSpawnPts.Add(g);
			openPaths++;
		}

		// double check that point is open, it will remove itself if it isn't
		g.GetComponent<RoomSpawnPoint>().checkSpawnIsOpen();
		
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

		//if (openSpawnPts.Contains(g))
		//{
			// when a room is spawned, the relevant point becomes inactive, and this is called to remove it
			openSpawnPts.Remove(g);
			openPaths--;
			g.GetComponent<RoomSpawnPoint>().setSpawnInactive();
			//Debug.Log("has removed " + g.name);
		//}
	}

	

	#endregion

	// Manage roomsInScene
	public void addNewRoomToScene(GameObject g, RoomSpawnPoint spawn)
	{
		// adds Gamoebject to roomsInScene list
		roomsInScene.Add(g);
		totalRoomsSoFar++;
		spawn.setSpawnInactive();
		removeFromSpawnList(spawn.gameObject);
	}

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
	public void spawnRoom(RoomSpawnPoint spawnPoint)
	{
		// spawns in a random room at the given point
		// room must be selected from correct list so doors align

		GameObject roomToSpawn = null;

		List<string> doorsRequired = new List<string>();
		List<string> doorsAvoided = new List<string>();

		string direction = spawnPoint.spawnDirection;

		Debug.Log("DIRECTION OF SPAWN PT = " + direction +  "FROM ROOM " +	spawnPoint.GetComponentInParent<Room>().gameObject.name);

		Debug.Log("sensor array found = " + spawnPoint.GetComponentsInChildren<RoomSpawnSensor>().Length);
		// check sensors to see if any doors are needed 
		foreach (RoomSpawnSensor sensor in spawnPoint.GetComponentsInChildren<RoomSpawnSensor>())
		{

			//Debug.Log("Running iterate through sensors");


			//Debug.Log("Finds the original room at direction " + sensor.direction + " : " + sensor.checkMustHave());
			if(sensor.checkRoomWasFound() && sensor.checkMustHave())
			{
				doorsRequired.Add(sensor.getRequiredDoorDir());
				Debug.Log("needs door in dir " + sensor.getRequiredDoorDir());
			}
			if(sensor.checkRoomWasFound() && sensor.checkMustNot())
			{
				doorsAvoided.Add(sensor.getRequiredDoorDir());
				Debug.Log("avoid door in dir " + sensor.getRequiredDoorDir());
			}


		}


		//if (direction == "N")
		//{
		//	// spawn direction N needs a room with a south facing door to link to
		//	// get random index in range

		//	//int index = Random.Range(0, rooms_southDoor.Count);
		//	//roomToSpawn = rooms_southDoor[index];
		//	//turnOffOppositeSpawn("S", roomToSpawn.GetComponent<Room>());

		//	doorsRequired.Add("S");
		//}
		//if (direction == "E")
		//{
		//	//int index = Random.Range(0, rooms_westDoor.Count);
		//	//roomToSpawn = rooms_westDoor[index];
		//	//turnOffOppositeSpawn("W", roomToSpawn.GetComponent<Room>());
		//	doorsRequired.Add("W");
		//}
		//if (direction == "S")
		//{
		//	//int index = Random.Range(0, rooms_northDoor.Count);
		//	//roomToSpawn = rooms_northDoor[index];
		//	//turnOffOppositeSpawn("N", roomToSpawn.GetComponent<Room>());
		//	doorsRequired.Add("N");
		//}
		//if (direction == "W")
		//{
		//	//int index = Random.Range(0, rooms_eastDoor.Count);
		//	//roomToSpawn = rooms_eastDoor[index];
		//	//turnOffOppositeSpawn("E", roomToSpawn.GetComponent<Room>());

		//	doorsRequired.Add("E");
		//}

		List<GameObject> roomsToChooseFrom = populateTempRoomList(doorsRequired, doorsAvoided);

		int index = Random.Range(0, roomsToChooseFrom.Count - 1);
		Debug.Log("Index is " + index + " and roomsList is " + roomsToChooseFrom.Count);

		roomToSpawn = roomsToChooseFrom[index];

		
		// console output to return a room
		Debug.Log("Room " + roomToSpawn.name + " has been selected to spawn from spawn point " + spawnPoint.gameObject.name);

		Vector3 tempTransform = spawnPoint.transform.position;

		// spawn said room at that pos
		Instantiate(roomToSpawn, tempTransform, Quaternion.identity);

		addNewRoomToScene(roomToSpawn.gameObject, spawnPoint);

	}

	public List<GameObject> populateTempRoomList(List<string> requiredDirs, List<string> avoidedDirs)
	{
		List<GameObject> tempRoomList = new List<GameObject>();
		List<GameObject> secondTempRoomList = new List<GameObject>();

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

		// remove any rooms with doors in the mustAvoid list

		Debug.Log("Added rooms to temp list successfully");

		if(avoidedDirs.Contains("N"))
		{
			foreach(GameObject g in tempRoomList)
			{
				if(g.GetComponent<Room>().nDoor)
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

		foreach(GameObject g in secondTempRoomList)
		{
			tempRoomList.Remove(g);
		}

		Debug.Log("removed rooms right");

		return tempRoomList;
	}

	public void turnOffOppositeSpawn(string direction, Room newRoom)
	{

		// When a room spawns, it has it's own spawn point that points back into the original room
		// based on what room this is, we automatically remove the appropriate spawn point from open list

		// get the spawnPts associated with new room
		RoomSpawnPoint[] tempSpawnPtArray = newRoom.GetComponentsInChildren<RoomSpawnPoint>();

		RoomSpawnPoint toRemove;

		toRemove = newRoom.GetSpawnPoint(direction);

		Debug.Log("!!!!!!Trying to remove from " + newRoom.gameObject.name + " Pt " + toRemove.gameObject.name);

		removeFromSpawnList(toRemove.gameObject);
	}

}
