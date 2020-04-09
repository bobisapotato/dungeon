﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{

    // Each door has a spawn point
    // Has a value of string N E S or W depending on which direction the spawn pt is 
    // This is where new room will be spawned when level is generated

    public string spawnDirection;
    public LevelGeneration levelGenManager;
    public BoxCollider checkSpawnCollider;
    public bool open = true;

    private void Start()
    {
        // get the level gen manager
        levelGenManager = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();
        // get the collider
        checkSpawnCollider = this.gameObject.GetComponent<BoxCollider>();

        // turn off the collider - only used to see if spawnpt is active
        checkSpawnCollider.enabled = false;

        open = true;

        checkSpawnIsOpen();

        levelGenManager.addNewSpawnPt(this.gameObject);

        Debug.Log(levelGenManager.openSpawnPts.Count + " spawns in list when spawn point " + gameObject.name + " is made");
        
    }


    public void checkSpawnIsOpen()
    {
        // turns on collider, if it triggers it will change value of open
        Debug.Log("checking spawn " + this.gameObject.name);
        StartCoroutine("toggleSpawnCollider");
    }

    public void setSpawnInactive()
    {
        Debug.Log("set spawn inactive being called for " + gameObject.name);
        open = false;

    }

    public IEnumerator toggleSpawnCollider()
    {
        checkSpawnCollider.enabled = true;
        Debug.Log("Turned collider on" + gameObject.name);
        yield return new WaitForSeconds(0.5f);
        checkSpawnCollider.enabled = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("Something trriggers this");
    //    // this is only triggered when turned on on checkSpawnIsOpen and if something is already in that spot
    //    // if this runs, the associated spawnPt should be closed
    //    if (open)
    //    {
    //        open = false;
    //        levelGenManager.removeFromSpawnList(this.gameObject);
    //    }
    //    checkSpawnCollider.enabled = false;
    //}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something triggers this collider on " + this.gameObject.name);
        // this is only triggered when turned on on checkSpawnIsOpen and if something is already in that spot
        // if this runs, the associated spawnPt should be closed

        if (open)
        {
            open = false;
            levelGenManager.removeFromSpawnList(this.gameObject);
        }
        checkSpawnCollider.enabled = false;
    }

}
