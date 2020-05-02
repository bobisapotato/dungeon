using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CameraHideAllWalls : MonoBehaviour
{
    // Stand in script as we've had raycast issues in CameraHideObstacles. 
    // This will see if a player or enemy is obscured, and if so, turn all walls transparent. 

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    private List<HideableObject> hideables = new List<HideableObject>();
    private GameObject player;
    private Camera mainCamera;
    private RaycastHit hit;
    private LevelGeneration levelGenerationManager;
    private float alpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponentInChildren<Camera>();
        levelGenerationManager = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();

        InvokeRepeating("triggerTargetRay", 0.1f, 0.2f);
    }

   

    public void triggerTargetRay()
    {
        // Sends a ray out to check if any of the targets are hidden.

        bool needToHideObjects = false;

        Debug.DrawRay(mainCamera.transform.position,
            (player.transform.position - mainCamera.transform.position), Color.magenta);

        // First, check if player is obscured. 
        // checks if ray cast hits anything
        if (Physics.Raycast(mainCamera.transform.position, (player.transform.position - mainCamera.transform.position),
            out hit))
        {
            if(hit.collider.gameObject == player)
            {
                // don't need to hide
            }
            else if(hit.collider.gameObject.GetComponent<MeshRenderer>())
            {
                //hide
                needToHideObjects = true;
                
            }
        }

        foreach (GameObject enemy in getEnemiesInCurrentRoom())
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

        if (needToHideObjects)
        {
            //run hide

            hideAllObjects();
        }
        else
        {
            showAllObjects();
        }

    }

    private void getAllEnemies()
    {
        // get a list of all the enemies in the level when the game starts. 
        
        foreach (GameObject room in levelGenerationManager.getRoomsInScene())
        {
            

            foreach(EnemyHealth enemy in room.GetComponent<Room>().getEnemiesInRoom())
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    private List<GameObject> getEnemiesInCurrentRoom()
    {
        // returns list of enemies in current room. 
        List<GameObject> enemiesInCurrentRoom = new List<GameObject>();

        foreach (GameObject room in levelGenerationManager.getRoomsInScene())
        {
            Room script = room.GetComponent<Room>();
            
            if (room.GetComponent<Room>().playerInRoom)
            {
                foreach (EnemyHealth enemy in room.GetComponent<Room>().getEnemiesInRoom())
                {
                    enemiesInCurrentRoom.Add(enemy.gameObject);
                }
            }
        }

        //Debug.Log("enemiesInCurrentRoom = " + enemiesInCurrentRoom.Count);
        return enemiesInCurrentRoom;
    }

    public void populateHideableList()
    {
        getAllEnemies();
        // gets all the hideableObjects from the scene
        foreach (GameObject hideableObj in GameObject.FindGameObjectsWithTag("Hideable"))
        {
            hideables.Add(hideableObj.GetComponent<HideableObject>());
        }

        //getAllEnemies();
    }

    private void hideAllObjects()
    {
        // hides all the hideable objects in the hideables list
        foreach(HideableObject hide in hideables)
        {

            hide.hideObject();
        }
    }

    private void showAllObjects()
    {
        foreach (HideableObject hide in hideables)
        {
            hide.showObject();
        }
    }

    
}
