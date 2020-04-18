using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Instantiates healths, and displays it.
public class HealthManager : MonoBehaviour
{
    public Slider healthbar;

    public static HealthSystem playerHealth;

    private int startPlayerHealth = 100;
    private int oldHealth;

    private GameManager gameMan;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = new HealthSystem(startPlayerHealth);
        
        oldHealth = playerHealth.GetHealth();


        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealth() != oldHealth) 
        {
            healthbar.value = ((float)playerHealth.GetHealth() / 100);
            Debug.Log(oldHealth);

        }

        oldHealth = playerHealth.GetHealth();

        

        if (oldHealth <= 0)
        {
            gameMan.openDemoLose2();
        }
    }
}
