using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{

    // Each door has a spawn point.
    // Has a value of string N E S or W depending on which direction the spawn pt is.
    // This is where new room will be spawned when level is generated.
    // Each spawn point has 4 'feelers', gameobjects with colliders that check which objects are
    // directly above, below, or next to the spawn point. 
    // This is to help identify what room to spawn.

    public string spawnDirection;
    public LevelGeneration levelGenManager;
    public BoxCollider checkSpawnCollider;
    public bool open = true;

    public bool wasTriggered = false;

    private void Start()
    {
        // get the level gen manager
        levelGenManager = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();
        // get the collider
        checkSpawnCollider = this.gameObject.GetComponent<BoxCollider>();

        // turn off the collider - only used to see if spawnpt is active
        checkSpawnCollider.enabled = false;

        open = true;

        levelGenManager.addNewSpawnPt(this.gameObject);

        checkSpawnCollider.enabled = true;
    }

   

    public void setSpawnInactive()
    {
        open = false;
    }

  
    private void OnTriggerEnter(Collider other)
    {
        // This is only triggered when turned on on checkSpawnIsOpen and if something is already in that spot.
        // If this runs, the associated spawnPt should be closed.
        // This should never need to run, as rooms should never spawn on top of one another anyway; this is just a backup.

        if (other.gameObject.GetComponent<Room>() || other.GetComponentInParent<Room>() && other.tag!= "RoomSpawn")
        {
            wasTriggered = true;
            if (open)
            {
                open = false;
                levelGenManager.removeFromSpawnList(this.gameObject);
            }
            checkSpawnCollider.enabled = false;
        }
    }

}
