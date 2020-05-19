using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to the objects that take damage from traps,
// i.e player.
public class TrapDamage : MonoBehaviour
{
    // Variables.
    private int damage = 3;

    float timer = 0f;
    float coolDown = 1f;

    void Update()
    {
        timer += Time.deltaTime;
    }

    // Needs additional checks if going to be attached to enemies,
    // as they do not have the component HealthSystem.
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Trap" && timer >= coolDown)
        {
            GetComponentInChildren<HealthSystem>().gameObject.SendMessage("TakeDamage", damage);
            timer = 0f;
        }
    }
}
