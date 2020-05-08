using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    private int damage = 20;

    bool isDamaged = false;

    float timer = 0f;
    float coolDown = 1f;

    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Trap" && timer >= coolDown)
        {
            GetComponentInChildren<HealthSystem>().gameObject.SendMessage("TakeDamage", damage);
            timer = 0f;
        }
    }
}
