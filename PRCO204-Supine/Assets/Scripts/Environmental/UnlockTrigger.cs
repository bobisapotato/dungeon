using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTrigger : MonoBehaviour
{
    // For testing, a block which when collided with unlocks the room

    private Room parentRoom;
    void Start()
    {
        if(GetComponentInParent<Room>())
        {
            parentRoom = GetComponentInParent<Room>();
        }
        else
        {
            Debug.LogError("No room found. Unlock Trigger should always be a child of a room.");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // When player hits the unlock trigger, the doors to the parent room are unlocked and the trigger disappears

        if (other.gameObject.tag == "Player") 
        {
            parentRoom.unlockAllDoors();
            this.gameObject.SetActive(false);
        }
    }
}
