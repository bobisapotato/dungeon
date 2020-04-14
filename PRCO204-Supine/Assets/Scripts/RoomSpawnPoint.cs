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

    private bool wasTriggered = false;
    

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

        
        InvokeRepeating("constantSpawnCheck", 0.1f, 3f);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine("toggleSpawnCollider");
        }
    }

    public IEnumerator constantSpawnCheck()
    {
        yield return new WaitForSeconds(1f);
        checkSpawnIsOpen();
    }

    public bool checkSpawnIsOpen()
    {
        // turns on collider, if it triggers it will change value of open
        StartCoroutine("toggleSpawnCollider");

        return open;
    }

    public void setSpawnInactive()
    {
        open = false;

    }

    public void finalCheck()
    {
        if(checkSpawnCollider == null)
        {
            Debug.Log("Was null");
            checkSpawnCollider = this.gameObject.GetComponent<BoxCollider>();
        }

        wasTriggered = false;
        manualTurnOn();
        Invoke("ManualTurnOff", 1f);
        if (!wasTriggered)
        {
            if (!open)
            {
                open = true;
                levelGenManager.addNewSpawnPt(this.gameObject);

                Debug.Log("Using new code to add spawn back");
            }
        }
    }

    private void manualTurnOn()
    {
        checkSpawnCollider.enabled = true;
    }

    private void manualTurnOff()
    {
        checkSpawnCollider.enabled = false;
    }

    public IEnumerator toggleSpawnCollider()
    {

        wasTriggered = false;
        checkSpawnCollider.enabled = true;
        //Debug.Log("Turned collider on" + gameObject.name);
        yield return new WaitForSeconds(1f);
        checkSpawnCollider.enabled = false;
        if(!wasTriggered)
        {
            if (!open)
            {
                open = true;
                levelGenManager.addNewSpawnPt(this.gameObject);

                Debug.Log("Using new code to add spawn back");
            }
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        // this is only triggered when turned on on checkSpawnIsOpen and if something is already in that spot
        // if this runs, the associated spawnPt should be closed
        wasTriggered = true;

        if (open)
        {
            open = false;
            levelGenManager.removeFromSpawnList(this.gameObject);
        }
        //checkSpawnCollider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!open)
        {
            open = true;
            levelGenManager.addNewSpawnPt(this.gameObject);
        }
    }

}
