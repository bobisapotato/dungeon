using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    private float timer = 0f;
    private float coolDown = 2.5f;

    private int hpBoost = 2;

    private int maxHealth = 100;

    [SerializeField]
    private AudioSource playerAudio;

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
            playerAudio.Play();
            GetComponentInChildren<HealthSystem>().Heal(hpBoost);
            timer = 0f;
        }
    }
}
