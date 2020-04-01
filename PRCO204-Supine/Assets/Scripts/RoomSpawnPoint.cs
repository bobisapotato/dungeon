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

    public void checkSpwnActive()
    {
        // see if theres any rooms in the world at the same pos
        // iterate through all rooms
        foreach(GameObject g in levelGenManager.getRoomsInScene())
        {
            if(g.transform.position == this.transform.position)
            {
                open = false;
            }
        }

    }
}
