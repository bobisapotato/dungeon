using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class CameraHideObstacles : MonoBehaviour
{
    // Objects are hidden if they are in between player and camera
    // Once player is no longer obscured, obstacles is shown again

    // VARS
    #region
    private float maxRayRange = 50f;
    private RaycastHit hit;
    private GameObject player;
    private Camera mainCamera;
    private LevelGeneration levelGenMan;
    [SerializeField] GameObject obstacle;

    [SerializeField] List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    #endregion
    private GameObject lastWall;
    float alpha = 1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponentInChildren<Camera>();
        levelGenMan = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();
        updateEnemyTargetList();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        checkPlayerObscured();

        foreach(GameObject target in targets)
        {
            checkTargetObscured(target);
        }
        checkOldObstacles();

        if (obstacle)
        {
            hideObstacle();
        }
    }

    public bool checkPlayerObscured()
    {
        // returns true if player is hidden from camera by an obstacle

        // draws ray to see in scene for testing
        Debug.DrawRay(mainCamera.transform.position,
            (player.transform.position - mainCamera.transform.position), Color.magenta);



        // checks if ray cast hits anything
        if (Physics.Raycast(mainCamera.transform.position, (player.transform.position - mainCamera.transform.position),
            out hit))
        {
			
            // sets bool based on what raycast hits

            if (hit.transform == player.transform)
            {
                //resetObstacle();
                return false;
            }
            else
            {
                newObstacle(hit.collider.gameObject);
                return true;
            }
        }
        else
        {
            //resetObstacle();
            return false;
        }
    }

    public bool checkTargetObscured(GameObject target)
    {
        // returns true if player is hidden from camera by an obstacle

        // draws ray to see in scene for testing
        Debug.DrawRay(mainCamera.transform.position,
            (target.transform.position - mainCamera.transform.position), Color.blue);

        // checks if ray cast hits anything
        if (Physics.Raycast(mainCamera.transform.position, (target.transform.position - mainCamera.transform.position),
            out hit, maxRayRange))
        {

            // sets bool based on what raycast hits

            if (hit.transform == target.transform)
            {
                //resetObstacle();
                return false;
            }
            else
            {
                newObstacle(hit.collider.gameObject);
                return true;
            }
        }
        else
        {
            //resetObstacle();
            return false;
        }
    }

   

    public void hideObstacle()
    {
        // sets mesh renderer to inactive for given GO
        if (obstacle.GetComponent<MeshRenderer>())
        {
            if (lastWall != obstacle)
            {
                alpha = 1f;

                lastWall = obstacle;
            }
            else
            {
                //obstacle.GetComponent<MeshRenderer>().enabled = false;
                if (alpha > 0.5f)
                {
                    alpha -= 0.1f;
                }

                obstacle.GetComponent<MeshRenderer>().material.color = new Color
                    (obstacle.GetComponent<MeshRenderer>().material.color.r,
                    obstacle.GetComponent<MeshRenderer>().material.color.g,
                    obstacle.GetComponent<MeshRenderer>().material.color.b, alpha);
            }
        }
    }

    public void checkOldObstacles()
    {
        // goes through obstacle list, if any don't need to be there anymore
        // then we run resetObstacle for it 

        foreach(GameObject obsGO in obstacles)
        {
            bool beingUsed = false;

            // run all the raycasts for all targets. 
            // if none hit the obstacle, remove it 
            foreach(GameObject target in targets)
            {
                if (Physics.Raycast(mainCamera.transform.position, (target.transform.position - mainCamera.transform.position),
                    out hit, maxRayRange))
                {
                    if (hit.transform == obsGO.transform)
                    {
                        beingUsed = true;
                    }
                }
            }
            if(!beingUsed)
            {
                // reset it 
                Debug.Log("old obs removed");
                resetObstacle(obsGO);
            }

        }
    }

    public void resetObstacle(GameObject obs)
    {
        // sets old obstacle to visible again and sets obstacle value to null

        if (obs)
        {
            if (obs.GetComponent<MeshRenderer>())
            {
                obs.GetComponent<MeshRenderer>().material.color = new Color
                   (obs.GetComponent<MeshRenderer>().material.color.r,
                   obs.GetComponent<MeshRenderer>().material.color.g,
                   obs.GetComponent<MeshRenderer>().material.color.b, 1f);
            }
            obstacles.Remove(obs);
        }
    }

    public void newObstacle(GameObject newObstacle)
    {
        // sets new obstacle so old obstacle can be properly reset
        obstacles.Add(newObstacle);
        //resetObstacle();
        obstacle = newObstacle;
    }

    public GameObject[] getEnemiesInRoom()
    {
        // finds current room
        // returns all the enemies in that room

        Room currentRoom = null;
        foreach(GameObject g in levelGenMan.getRoomsInScene())
        {
            Room tempRoom = g.GetComponent<Room>();

            if(tempRoom.playerInRoom)
            {
                currentRoom = tempRoom;
            }
        }

        GameObject[] enemies = new GameObject[currentRoom.getEnemiesInRoom().Count];

        int counter = 0;
        foreach(EnemyHealth enem in currentRoom.getEnemiesInRoom())
        {
            enemies[counter] = enem.gameObject;
            counter++;
        }

        return enemies;
    }

    public void updateEnemyTargetList()
    {
        // whenever we enter a new room, change the enemies on the target list
        // to the ones in that room. 

        targets.Clear();
        targets.Add(player);

        foreach(GameObject g in getEnemiesInRoom())
        {
            targets.Add(g);
        }
    }

}
