using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    // Variables.
    private float timer = 0f;
    private float coolDown = 4f;

    private int hpBoost = 1;

    private int maxHealth = 100;

    void Start()
    {
        maxHealth = GetComponentInChildren<HealthSystem>().GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Heal the player after the cooldown is over.
        if (timer >= coolDown && GetComponentInChildren<HealthSystem>().GetHealth() <= maxHealth)
        {
            GetComponentInChildren<HealthSystem>().Heal(hpBoost);
            timer = 0f;
        }
    }
}
