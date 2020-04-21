using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAttack : MonoBehaviour
{
    //Variables
    Collider hitbox;

    [SerializeField] int damage;


    // Start is called before the first frame update
    void Start()
    {
        hitbox = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("TakeDamage", damage);
        }
    }
}
