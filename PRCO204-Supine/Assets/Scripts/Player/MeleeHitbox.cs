using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    // Variables.
    Collider hitbox;

    [SerializeField] 
    private int melee;

    [SerializeField]
    private AudioSource swordHit;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = gameObject.GetComponent<CapsuleCollider>();
    }

    // If anything with an enemy tag is inside the trigger hitbox when
    // its active, it sends a message to enemy health and runs the method
    // take damage. Then disables the hitbox.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            swordHit.Play();
            other.gameObject.SendMessage("TakeDamage", melee);
            hitbox.enabled = false;
        }
    }
}
