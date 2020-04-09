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

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = new HealthSystem(startPlayerHealth);

        oldHealth = playerHealth.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealth() != oldHealth) 
        {
            healthbar.value = playerHealth.GetHealth();
        }

        oldHealth = playerHealth.GetHealth();
    }
}
