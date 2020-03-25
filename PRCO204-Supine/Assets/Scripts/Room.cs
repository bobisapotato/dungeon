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
    private bool doorsLocked = false;
    public bool playerInRoom;

    #endregion

    // Start is called before the first frame update.
    void Start()
    {
        if(!this.GetComponentInChildren<Door>())
        {
            Debug.LogError("No doors are attached to this room. Each room requires at least one door child");
        }

        doors = this.GetComponentsInChildren<Door>();
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

        doorsLocked = false;
    }
}
