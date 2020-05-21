using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    // Variables.
    public static float timer = 0f;
    private float coolDown;

    private int hpBoost = 1;

    private int maxHealth = 100;

    void Start()
    {
        maxHealth = GetComponentInChildren<HealthSystem>().GetMaxHealth();

        coolDown = 10f;
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
            coolDown = 6f;
        }
    }
}
