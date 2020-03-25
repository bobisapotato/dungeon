using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Each room has at least one door which can be locked and unlocked.
    // Starts closed, opens when player walks in.
    // public methods can be accessed from other scripts to lock and unlock it again based on objectives.

    public Animator animator;
    // Door always starts closed, opens when player collides for the first time.
    public bool open = false;  
    public bool locked = false;
    
    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        if (!this.GetComponentInParent<Room>())
        {
            Debug.LogError("No parent room found. A door should always be a child of a room");
        }
    }

    private void Update()
    {
        // For testing purposes - set locked value in anim in the inspector.
        if (locked)
        {
            lockDoor();
        }
        else
        {
            unlockDoor();
        }

        // For testing purposes - set open value in inspector.
        if(open)
        {
            animator.SetBool("Open", true);
        }
        else
        {
            animator.SetBool("Open", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("IsCollidingWSomething");

        if (collision.gameObject.tag == "Player" && !open && !locked)
        {
            animator.SetBool("Open", true);
            open = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !open && !locked)
        {
            animator.SetBool("Open", true);
            open = true;
        }
    }

    public void lockDoor()
    {
        // Door can be locked from other scripts - doors close and player can't pass through doorway.
        animator.SetBool("Locked", true);
        locked = true;
    }

    public void unlockDoor()
    {
        // Door unlocks, opens, can be passed through.
        animator.SetBool("Locked", false);
        locked = false;
    }
}
