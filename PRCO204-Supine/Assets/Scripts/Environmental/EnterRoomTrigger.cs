using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Each door has an enter room trigger.
// This activates when player collides.
// Sends message up to the doors parent room to say that player is in the room.
public class EnterRoomTrigger : MonoBehaviour
{
    // Error handling.
    private void Start()
    {
        if(!this.GetComponentInParent<Room>())
        {
            Debug.LogError("No Parent Room found. A room trigger should only exist as a child a room.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.GetComponentInParent<Room>().setPlayerInRoom(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.GetComponentInParent<Room>().setPlayerInRoom(false);
        }
    }
}
