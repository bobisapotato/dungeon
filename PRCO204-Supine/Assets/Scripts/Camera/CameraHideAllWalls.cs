using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHideAllWalls : MonoBehaviour
{
    // Stand in script as we've had raycast issues in CameraHideObstacles. 
    // This will see if a player or enemy is obscured, and if so, turn all walls transparent. 

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private Camera mainCamera;
    private RaycastHit hit;
    private LevelGeneration levelGenerationManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponentInChildren<Camera>();
        levelGenerationManager = GameObject.FindGameObjectWithTag("LevelGenerationManager").GetComponent<LevelGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void triggerTargetRay()
    {
        // Sends a ray out to check if any of the targets are hidden.

        bool needToHideObjects = false;

        // First, check if player is obscured. 
        // checks if ray cast hits anything
        if (Physics.Raycast(mainCamera.transform.position, (player.transform.position - mainCamera.transform.position),
            out hit))
        {
            if(hit.collider.gameObject == player)
            {
                // don't need to hide
            }
            else 
            {
                //hide
                needToHideObjects = true;
            }
        }

        foreach(GameObject enemy in enemies)
        {
            if (Physics.Raycast(mainCamera.transform.position, (enemy.transform.position - mainCamera.transform.position),
            out hit))
            {
                if (hit.collider.gameObject == enemy)
                {
                    // don't need to hide
                }
                else
                {
                    //hide
                    needToHideObjects = true;
                }
            }
        }

        if(needToHideObjects)
        {
            //run hide
        }

    }

    private void getAllEnemies()
    {
        // get a list of all the enemies in the level when the game starts. 

        foreach(GameObject room in levelGenerationManager.getRoomsInScene())
        {
            foreach(EnemyHealth enemy in room.GetComponent<Room>().getEnemiesInRoom())
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }
}
