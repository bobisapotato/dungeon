using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountManager : MonoBehaviour
{
    // handles the grand total of enemies to kill.
    // sets the counter in top of screen 
    // ends game when counter hits 0

    public LevelGeneration levelManager;
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private List<EnemyHealth> enemiesInLevel = new List<EnemyHealth>();
    public int enemyCount = 0;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        // save list of rooms
        rooms = levelManager.getRoomsInScene();
        populateEnemyList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void populateEnemyList()
    {
        foreach(GameObject g in rooms)
        {
            foreach(EnemyHealth enemy in g.GetComponent<Room>().getEnemiesInRoom())
            {
                enemiesInLevel.Add(enemy);
            }
        }

        enemyCount = enemiesInLevel.Count;
    }

    public void enemyKilled(EnemyHealth enemyKilled)
    {
        // when enemy is killed it sends message here to remove it from list
        enemiesInLevel.Remove(enemyKilled);
        enemyCount--;

        if(enemyCount == 0)
        {
            // show win screen via the gameManager
        }
    }
}
