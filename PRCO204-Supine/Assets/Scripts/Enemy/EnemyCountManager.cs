using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
   // public int halfEnemyCount { get => (int)Mathf.Ceil(startEnemyTotal / 2); }
    public int startEnemyTotal;


    public void startUpEnemyCounter()
    {
        // save list of rooms

        rooms = levelManager.getRoomsInScene();
        Invoke("populateEnemyList", 1f);
    }

    private void populateEnemyList()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<EnemyHealth>())
            {
                enemiesInLevel.Add(enemy.GetComponent<EnemyHealth>());
            }
        }

        enemyCount = enemiesInLevel.Count;
        startEnemyTotal = enemyCount;
        updateLabel();
    }

    public void enemyKilled(EnemyHealth enemyKilled)
    {
        // when enemy is killed it sends message here to remove it from list
        enemiesInLevel.Remove(enemyKilled);
        enemyCount--;
        updateLabel();
    }

    public void updateLabel()
    {
        enemyCountLabel.text = ("x" + enemyCount.ToString());
    }

    public int halfEnemyCount
    {
        get => (int)Mathf.Ceil(startEnemyTotal / 2f); 
    }

    
}
