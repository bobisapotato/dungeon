using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	// one LevelManager object in the scene, containing this script
	// controls spawning of rooms from prefabs to create a level 

	// VARS
	public GameObject[] roomPrefabs;
	[SerializeField] private GameObject[] roomsInScene;
	public int maximumRooms;
	[SerializeField] private int totalRoomsSoFar;
	[SerializeField] private int openPaths;
	public GameObject startRoomPrefab;

	public GameObject[] getRoomsInScene()
	{
		return roomsInScene;
	}

	private void Start()
	{
		// to start, we just create the starter room at 0.0
		Vector3 startSpawn = new Vector3(0, 0, 0);
		Quaternion startRot = new Quaternion(0, 0, 0, 0);
		Instantiate(startRoomPrefab, startSpawn, startRot);
	}

}
