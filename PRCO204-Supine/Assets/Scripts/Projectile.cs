using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    //Variables
    [SerializeField] float velocity;
    [SerializeField] int damage;

    // Start is called before the first frame update
    // Adds a force the the projectile.
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * velocity);
        StartCoroutine(Despawn());
    }

    // Waits 5 seconds to check to see if anything collides with it,
    // if not it destroys itself.
    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5.0f);
        Die();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // If the bullet collides with a gameobject with the tag, "enemy" it 
    // sends a message to the enemy's health script to run the take damage
    // method. It then kills itself.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SendMessage("TakeDamage", damage);
            Die();
        }
        else
        {
            Die();
        }
    }


    // Destroys the gameobject this is attached to.
    private void Die()
    {
        Destroy(gameObject);
    }
}
