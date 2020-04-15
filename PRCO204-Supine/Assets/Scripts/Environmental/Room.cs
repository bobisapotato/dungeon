using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{
    // Rooms have at least one door.
    // Multiple rooms make up a level.
    // Rooms have objectives to be met to clear the room.
    // When player enters room for the first time, all doors lock behind them.
    // When room is cleared doors unlock.

    // VARIABLES
    #region
    public Door[] doors;
    public bool roomCleared = false;
    public bool doorsLocked = false;
    public bool playerInRoom;
    public BoxCollider inRoomTrigger;

    // door directions
    public bool nDoor = false;
    public bool eDoor = false;
    public bool sDoor = false;
    public bool wDoor = false;

    // unlock vars

    [SerializeField] private List<EnemyHealth> enemiesInRoom = new List<EnemyHealth>();
    public EnemyCountManager enemyCountManager;

    private LevelGeneration levelGenMan;

    public bool justCreated = true;
    #endregion

    // Start is called before the first frame update.
    void Start()
    {
        if(!this.GetComponentInChildren<Door>())
        {
            Debug.LogError("No doors are attached to this room. Each room requires at least one door child");
        }

        doors = this.GetComponentsInChildren<Door>();
        setUpDoorDirections();

        // get the roomTrigger
        inRoomTrigger = GetComponentInChildren<EnterRoomTrigger>().gameObject.GetComponent<BoxCollider>();

        StartCoroutine("updatebabyBool");
        // get enemyCountManager
        enemyCountManager = GameObject.FindGameObjectWithTag("EnemyCountMan").GetComponent<EnemyCountManager>();

        // get LevelGen from parent.
        levelGenMan = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();

        populateEnemiesInRoom();
    }

    // Update is called once per frame.
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

    public IEnumerator updatebabyBool()
    {
        yield return new WaitForSeconds(0.1f);
        justCreated = false;
    }

    public void populateEnemiesInRoom()
    {
        EnemyHealth[] tempArray = GetComponentsInChildren<EnemyHealth>();

        foreach(EnemyHealth enemy in tempArray)
        {
            enemiesInRoom.Add(enemy);
        }
    }

    public void enemyKilled(EnemyHealth enemyKilled)
    {
        enemiesInRoom.Remove(enemyKilled);

        if(enemiesInRoom.Count == 0)
        {
            // all enemies killed
            unlockAllDoors();
        }

        enemyCountManager.enemyKilled(enemyKilled);
    }

    public List<EnemyHealth> getEnemiesInRoom()
    {
        return enemiesInRoom;
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


    private void OnTriggerEnter(Collider other)
    {
        // Sometimes doors overlap. This shouldn't happen if the spawnSensor script is
        // properly working, but for now the quick fix is just deleting the second room.

        if(other.gameObject.GetComponent<Room>())
        {
            Room otherRoom = other.gameObject.GetComponent<Room>();
            if(otherRoom.justCreated && !justCreated)
            {
                otherRoom.destroyThisRoom();
                Debug.Log("delete other Room");
            }

        }
    }

    public void destroyThisRoom()
    {
        levelGenMan.removeRoomFromScene(this.gameObject);

        foreach(RoomSpawnPoint spawn in GetComponentsInChildren<RoomSpawnPoint>())
        {
            levelGenMan.removeFromSpawnList(spawn.gameObject);
        }
        Destroy(this.gameObject);
    }

}
