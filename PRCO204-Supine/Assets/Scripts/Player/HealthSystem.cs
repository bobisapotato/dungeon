using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    // Variables.
    public int health;

    private int minHealth = 0;
    private int maxHealth = 100;

    // Constructor.
    public HealthSystem(int health)
    {
        this.health = health;
    }

    // Getters and Setters.
    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        if ((health - damage) <= minHealth)
        {
            health = minHealth;
        }
        else 
        {
            health -= damage;
        }
    }

    public void Heal(int heal)
    {
        if ((health + heal) >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health -= heal;
        }
    }
}
