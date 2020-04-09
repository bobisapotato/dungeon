using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{
    // Rooms have at least one door
    // Multiple rooms make up a level
    // Rooms have objectives to be met to clear the room
    // When player enters room for the first time, all doors lock behind them
    // When room is cleared doors unlock

    // VARIABLES
    #region
    public Door[] doors;
    public bool roomCleared = false;
    public bool doorsLocked = false;
    public bool playerInRoom;

    // door directions
    public bool nDoor = false;
    public bool eDoor = false;
    public bool sDoor = false;
    public bool wDoor = false;

    public bool justCreated = true;

    LevelGeneration levelGenMan;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(!this.GetComponentInChildren<Door>())
        {
            Debug.LogError("No doors are attached to this room. Each room requires at least one door child");
        }

        doors = this.GetComponentsInChildren<Door>();
        setUpDoorDirections();

        // if theres a spawn point at the same loc, set it to inactive

        StartCoroutine("stayinAlive");

    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRoom & !roomCleared & !doorsLocked)
        {
            lockAllDoors();
        }
        else if (playerInRoom & roomCleared & doorsLocked)
        {
            unlockAllDoors();
        }
    }

    public IEnumerator stayinAlive()
    {
        // sets bool justCreated to true after 2 seconds
        // used to compare two rooms when they spawn on top of one another and one must be deleted

        yield return new WaitForSeconds(.2f);
        justCreated = false;
    }

    private void setUpDoorDirections()
    {
        // sets bools for each door dir based on the spawn pts in the children
        foreach(Door d in doors)
        {
            if(d.direction == "N")
            {
                nDoor = true;
            }
            if (d.direction == "E")
            {
                eDoor = true;
            }
            if (d.direction == "S")
            {
                sDoor = true;
            }
            if (d.direction == "W")
            {
                wDoor = true;
            }
        }
        
    }

    public void setPlayerInRoom(bool playerInRoomInput)
    {
        playerInRoom = playerInRoomInput;
    }

    public void lockAllDoors()
    {
        foreach(Door d in doors)
        {
            d.lockDoor();
        }

        doorsLocked = true;
    }

    public void unlockAllDoors()
    {
        foreach (Door d in doors)
        {
            d.unlockDoor();
        }

        roomCleared = true;
        doorsLocked = false;
    }

    // Get spoawn points for room based on direction
	#region
    public RoomSpawnPoint GetSpawnPoint(string direction)
    {
        RoomSpawnPoint tempSpawn = null;

        RoomSpawnPoint[] allSpawnPoints = this.GetComponentsInChildren<RoomSpawnPoint>();

        foreach (RoomSpawnPoint spawn in allSpawnPoints)
        {
            if (direction == spawn.spawnDirection)
            {
                tempSpawn = spawn;
            }
        }
        return tempSpawn;
    }
    #endregion

    // destroy room if it's colliding with another

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Room>())
        {
            Room otherRoom = other.gameObject.GetComponent<Room>();

            if(otherRoom.justCreated && !justCreated)
            {
                Destroy(otherRoom.gameObject);
                Debug.Log("ROOM " + this.gameObject.name + " WON A FIGHT");
            }
            else if (!otherRoom.justCreated && justCreated)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("STALEMATE");
            }
        }
    }

    public void destroyRoom(Room roomToDestroy)
    {
        // remove from all lists in manager
        
       
        foreach (RoomSpawnPoint spawn in GetComponentsInChildren<RoomSpawnPoint>())
        {
            levelGenMan.removeFromSpawnList(spawn.gameObject);
            levelGenMan.removeRoomFromScene(this.gameObject);
        }
    }
}
