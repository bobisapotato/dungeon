using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //Variables / unity events
    [SerializeField] int health = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }

    }


    //Takes the amount sent to the method away from the enemies health

    //needs hit.transform.SendMessage("TakeDamage", amount); in player attack
    public void TakeDamage(int amount)
    {
        health -= amount;
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
