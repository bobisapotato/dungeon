﻿using System.Collections;
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
        loadingPanel.GetComponent<Animator>().Play("HideLoadingPanel");
        setUIVisibility(true);
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

   public bool checkDroppedCrossbow()
   {
        return droppedCrossbow;
   }

    public void dropCrossbow()
    {
        // called when crossbow is spawned to set bool to true.
        droppedCrossbow = true;
    }

    public void setUIVisibility(bool newSetting)
    {
        foreach(GameObject UI in UItoHide)
        {
            UI.SetActive(newSetting);
        }
    }
}
