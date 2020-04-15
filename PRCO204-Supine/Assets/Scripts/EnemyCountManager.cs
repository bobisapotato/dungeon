using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI enemyCountLabel;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startUpEnemyCounter()
    {
        // save list of rooms

        rooms = levelManager.getRoomsInScene();
        populateEnemyList();
    }

    private void populateEnemyList()
    {
        //foreach(GameObject g in rooms)
        //{
        //    foreach(EnemyHealth enemy in g.GetComponent<Room>().getEnemiesInRoom())
        //    {
        //        enemiesInLevel.Add(enemy);
        //    }
        //}

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            
            enemiesInLevel.Add(enemy.GetComponent<EnemyHealth>());
        }

        enemyCount = enemiesInLevel.Count;
        updateLabel();
    }

    public void enemyKilled(EnemyHealth enemyKilled)
    {
        // when enemy is killed it sends message here to remove it from list
        enemiesInLevel.Remove(enemyKilled);
        enemyCount--;
        updateLabel();
        if(enemyCount == 0)
        {
            // show win screen via the gameManager
            gameManager.openDemoWin2();
        }
    }

    public void updateLabel()
    {
        enemyCountLabel.text = ("Enemy Count = " + enemyCount.ToString());
    }
}
