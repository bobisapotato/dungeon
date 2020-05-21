using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EnemyCountManager : MonoBehaviour
{
    // Handles the grand total of enemies to kill.
    // Sets the counter in top of screen.
    // When all UI is formed, this script removes the laoding panel.

    public LevelGeneration levelManager;
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private List<EnemyHealth> enemiesInLevel = new List<EnemyHealth>();
    public int enemyCount = 0;
    public GameManager gameManager;
    public TextMeshProUGUI enemyCountLabel;
   
    public int startEnemyTotal;
    private bool droppedCrossbow = false;

    // Loading Panel
    public GameObject loadingPanel;
    public GameObject[] UItoHide;

    private void Start()
    {
        setUIVisibility(false);
    }

    public void startUpEnemyCounter()
    {
        // Save a list of all the rooms
        rooms = levelManager.getRoomsInScene();
        Invoke("populateEnemyList", 1f);
        
    }

    // Gets all the enemies in the scene.
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
        loadingPanel.GetComponent<Animator>().Play("HideLoadingPanel");
        setUIVisibility(true);
    }

    // Is called when an enemy is killed. 
    public void enemyKilled(EnemyHealth enemyKilled)
    {
        // when enemy is killed it sends message here to remove it from list
        enemiesInLevel.Remove(enemyKilled);
        enemyCount--;
        updateLabel();
    }

    // Updates the UI.
    public void updateLabel()
    {
        enemyCountLabel.text = ("x" + enemyCount.ToString());
    }

    // Returns half the number of total enemies.
    public int halfEnemyCount
    {
        get => (int)Mathf.Ceil(startEnemyTotal / 2f); 
    }

   // Used to only drop the crossbow once per level.
   public bool checkDroppedCrossbow()
   {
        return droppedCrossbow;
   }

    // Called when crossbow is spawned to set bool to true.
    public void dropCrossbow()
    {
        droppedCrossbow = true;
    }

    // Set whether the UI is active or not.
    public void setUIVisibility(bool newSetting)
    {
        foreach(GameObject UI in UItoHide)
        {
            UI.SetActive(newSetting);
        }
    }
}
