using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    // Variables
    Collider hitbox;

    [SerializeField] int melee;
    [SerializeField] GameObject projectile;


    // Start is called before the first frame update
    // Gets the collider of the gameobject and stores it 
    // as hitbox for later reference.
    void Start()
    {
        hitbox = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame and checks for key presses.
    // Executes the corrisponding attack.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            Attack1();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) == true)
        {
            Attack2();
        }

    }

    // Enables the hitbox to attack.
    private void Attack1()
    {
        hitbox.enabled = true;
        StartCoroutine(DisableCollider());
    }

    // Instantiates the projectile to be shot.
    private void Attack2()
    {

        Instantiate(projectile, transform.position, projectile.transform.rotation);

        //   RaycastHit hit;
        //   Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));


        //if (Physics.Raycast(ray, out hit, hitRange))
        //   {
        //       if (hit.transform.gameObject.tag == "Enemy")
        //       {
        //           Debug.Log("hit");
        //           hit.transform.gameObject.SendMessage("TakeDamage", ranged);
        //       }
        //   }
    }


    // If anything with an enemy tag is inside the trigger hitbox when
    // its active, it sends a message to enemy health and runs the method
    // take damage. Then disables the hitbox.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SendMessage("TakeDamage", melee);
            hitbox.enabled = false;
        }
    }

    // After 1 second, if nothing with the enemy tag
    // is in the hitbox, it disables it.
    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1.0f);

        hitbox.enabled = false;
    }
}
