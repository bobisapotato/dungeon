using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls walls fading in and out.
// This will see if a player or enemy is obscured, and if so, turn all walls transparent. 
// Walls need the 'HideableObject' Script filled out in the inspector with two materials, 
// one opaque one transparent, for this to work. 
public class CameraHideAllWalls : MonoBehaviour
{
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

    // Sends a ray out to check if any of the targets are hidden.
    public void triggerTargetRay()
    {
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
                // Don't need to hide.
            }
            else if(hit.collider.gameObject.GetComponent<MeshRenderer>())
            {
                // Hide.
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
                        // Don't need to hide.
                    }
                    else
                    {
                        // Hide.
                        needToHideObjects = true;
                    }
                }
        }

        if (needToHideObjects)
        {
            hideAllObjects();
        }
        else
        {
            showAllObjects();
        }

    }

    // Gets a list of all the enemies in the level when the game starts. 
    private void getAllEnemies()
    {        
        foreach (GameObject room in levelGenerationManager.getRoomsInScene())
        {
            foreach(EnemyHealth enemy in room.GetComponent<Room>().getEnemiesInRoom())
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    // Returns list of enemies in current room. 
    private List<GameObject> getEnemiesInCurrentRoom()
    {
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

        return enemiesInCurrentRoom;
    }

    public void populateHideableList()
    {
        Debug.Log("trying to pop list");

        getAllEnemies();

        // Gets all the hideableObjects from the scene.
        foreach (GameObject hideableObj in GameObject.FindGameObjectsWithTag("Hideable"))
        {
            hideables.Add(hideableObj.GetComponent<HideableObject>());
        }
    }

    // Hides all the hideable objects in the hideables list.
    private void hideAllObjects()
    {
        foreach(HideableObject hide in hideables)
        {
            hide.hideObject();
        }
    }

    // Shows all the hideable objects in the hideables list.
    private void showAllObjects()
    {
        foreach (HideableObject hide in hideables)
        {
            hide.showObject();
        }
    }
}
