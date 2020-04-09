using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Variables 
    [SerializeField] int health = 100;
    [SerializeField] int knockback;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Checks to 
    void Update()
    {
        if (health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }

    }


    // Takes the amount sent to the method away from the enemies health
    // then applies a knockback affect to the enemy.
    public void TakeDamage(int amount)
    {
        health -= amount;

        gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * knockback);
    }
    

    public void DropItem()
    {
        System.Random rand = new System.Random();
        int drop = rand.Next(1, 10);

        if (drop == 10)
        {
            //Spawn dropped item here.
        }

    }


}
