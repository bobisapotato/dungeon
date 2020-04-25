using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    private float timer = 0f;
    private float coolDown = 2f;

    private int hpBoost = 2;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= coolDown)
        {
            HealthManager.playerHealth.Heal(hpBoost);
            timer = 0f;
        }
    }
}
