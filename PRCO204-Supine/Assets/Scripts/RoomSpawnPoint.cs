using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{

    // Each door has a spawn point
    // Has a value of string N E S or W depending on which direction the spawn pt is 
    // This is where new room will be spawned when level is generated

    public string spawnDirection;
    public LevelGeneration levelGenManager;

    public bool open = true;

    private void Start()
    {
        // get the level gen manager
        levelGenManager = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();

        if(open)
        {
            levelGenManager.addToSpawnPtList(this.gameObject);
        }
    }

    private void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("COLLISION");
        open = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
        setSpawnInactive();
        levelGenManager.
            removeFromSpawnList
            (this.gameObject);
    }

    public void setSpawnInactive()
    {
        open = false;
        
    }

}
