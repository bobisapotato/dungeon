using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomTrigger : MonoBehaviour
{
    // Each door has an enter room trigger.
    // This activates when player collides.
    // Sends message up to the doors parent room to say that player is in the room.
    
    private void Start()
    {
        if(!this.GetComponentInParent<Room>())
        {
            Debug.LogError("No Parent Room found. A room trigger should only exist as a child of a door, which is in turn a child of a room.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.GetComponentInParent<Room>().setPlayerInRoom(true);
        }
    }
}
