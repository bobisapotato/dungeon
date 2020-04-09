using System.Collections;
using System.Collections.Generic;
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
			if(openSpawnPts[index].GetComponent<RoomSpawnPoint>().open)
			{
				spawnRoom(openSpawnPts[index].GetComponent<RoomSpawnPoint>());
			}
			else
			{
				openSpawnPts.Remove(openSpawnPts[index].GetComponent<RoomSpawnPoint>().gameObject);
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
		Debug.Log("Adding new spawn to list in top method - now list is " + openPaths);
		Debug.Log("when spawn " + g.name + " is added, its open value is " + g.GetComponent<RoomSpawnPoint>().open);
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
		Debug.Log("spawn point " + g.name + " is in list? = " + openSpawnPts.Contains(g));

		//if (openSpawnPts.Contains(g))
		//{
			// when a room is spawned, the relevant point becomes inactive, and this is called to remove it
			openSpawnPts.Remove(g);
			openPaths--;
			g.GetComponent<RoomSpawnPoint>().setSpawnInactive();
			Debug.Log("has removed " + g.name);
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

		string direction = spawnPoint.spawnDirection;

		if (direction == "N")
		{
			// spawn direction N needs a room with a south facing door to link to
			// get random index in range
			int index = Random.Range(0, rooms_southDoor.Count);
			roomToSpawn = rooms_southDoor[index];
			turnOffOppositeSpawn("S", roomToSpawn.GetComponent<Room>());
		}
		if (direction == "E")
		{
			int index = Random.Range(0, rooms_westDoor.Count);
			roomToSpawn = rooms_westDoor[index];
			turnOffOppositeSpawn("W", roomToSpawn.GetComponent<Room>());
		}
		if (direction == "S")
		{
			int index = Random.Range(0, rooms_northDoor.Count);
			roomToSpawn = rooms_northDoor[index];
			turnOffOppositeSpawn("N", roomToSpawn.GetComponent<Room>());
		}
		if (direction == "W")
		{
			int index = Random.Range(0, rooms_eastDoor.Count);
			roomToSpawn = rooms_eastDoor[index];
			turnOffOppositeSpawn("E", roomToSpawn.GetComponent<Room>());
		}

		
		// console output to return a room
		Debug.Log("Room " + roomToSpawn.name + " has been selected to spawn from spawn point " + spawnPoint.gameObject.name);

		Vector3 tempTransform = spawnPoint.transform.position;

		// spawn said room at that pos
		Instantiate(roomToSpawn, tempTransform, Quaternion.identity);

		addNewRoomToScene(roomToSpawn.gameObject, spawnPoint);

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
