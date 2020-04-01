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
	public List<GameObject> activeSpawnPts = new List<GameObject>();
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
		populateSpawnPtList();
		populateRoomDirectionLists();

		// test room gen
		// spawn room for each point in first room
		spawnRoom(activeSpawnPts[0].GetComponent<RoomSpawnPoint>());
		spawnRoom(activeSpawnPts[1].GetComponent<RoomSpawnPoint>());
		spawnRoom(activeSpawnPts[2].GetComponent<RoomSpawnPoint>());
		spawnRoom(activeSpawnPts[3].GetComponent<RoomSpawnPoint>());
	}

	private void Update()
	{
		// TESTING - PRESS R TO RESPAWN ROOMS
		if (Input.GetKeyDown(KeyCode.R))
		{
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Room"))
			{
				g.SetActive(false);
			}
			Instantiate(startRoomPrefab, startSpawn, startRot);

			spawnRoom(activeSpawnPts[0].GetComponent<RoomSpawnPoint>());
			spawnRoom(activeSpawnPts[1].GetComponent<RoomSpawnPoint>());
			spawnRoom(activeSpawnPts[2].GetComponent<RoomSpawnPoint>());
			spawnRoom(activeSpawnPts[3].GetComponent<RoomSpawnPoint>());

		}
	}

	public List<GameObject> getRoomsInScene()
	{
		return roomsInScene;
	}

	// Managing list of active spawn points
	#region
	public void populateSpawnPtList()
	{
		GameObject[] tempSpawnPts = GameObject.FindGameObjectsWithTag("RoomSpawn");
		foreach(GameObject g in tempSpawnPts)
		{
			if(g.GetComponent<RoomSpawnPoint>().open == true)
			{
				activeSpawnPts.Add(g);
			}
		}
		openPaths = activeSpawnPts.Count;
	}

	public void addToSpawnPtList(GameObject g)
	{
		// when a new room spawn pt is instatiated, it should call this to add itself to the active list
		if (g.GetComponent<RoomSpawnPoint>().open == true)
		{
			activeSpawnPts.Add(g);
		}
		openPaths++;
	}

	public void removeFromSpawnList(GameObject g)
	{
		// when a room is spawned, the relevant point becomes inactive, and this is called to remove it
		activeSpawnPts.Remove(g);
		openPaths--;
	}
	#endregion

	// Manage roomsInScene
	public void addNewRoomToScene(GameObject g)
	{
		// adds Gamoebject to roomsInScene list
		roomsInScene.Add(g);
		totalRoomsSoFar++;
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



		if (spawnPoint.spawnDirection == "N")
		{
			// spawn direction N needs a room with a south facing door to link to
			// get random index in range
			int index = Random.Range(0, rooms_southDoor.Count);
			roomToSpawn = rooms_southDoor[index];
		}
		if (spawnPoint.spawnDirection == "E")
		{
			int index = Random.Range(0, rooms_westDoor.Count);
			roomToSpawn = rooms_westDoor[index];
		}
		if (spawnPoint.spawnDirection == "S")
		{
			int index = Random.Range(0, rooms_northDoor.Count);
			roomToSpawn = rooms_northDoor[index];
		}
		if (spawnPoint.spawnDirection == "W")
		{
			int index = Random.Range(0, rooms_eastDoor.Count);
			roomToSpawn = rooms_eastDoor[index];
		}

		// console output to return a room
		Debug.Log("Room " + roomToSpawn.name + " has been selected to spawn from spawn point " + spawnPoint.gameObject.name);

		Vector3 tempTransform = spawnPoint.transform.position;
		

		// spawn said room at that pos
		Instantiate(roomToSpawn, tempTransform, Quaternion.identity);

		addNewRoomToScene(roomToSpawn.gameObject);
	}

}
