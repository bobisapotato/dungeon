using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack2 : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private int damage = 10;

    private float attackCoolDown = 0f;
    private float attackCoolDownTime = 1f;
    private float meleeAttackRadius = 3f;

    [SerializeField]
    private AudioSource playerHurt;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Add to attack cooldown.
        if (attackCoolDown <= attackCoolDownTime)
        {
            attackCoolDown += Time.deltaTime;
        }

        // Get the distance to the player.
        float movementDistance = Vector3.Distance(player.transform.position,
            transform.position);

        // If inside the radius & attack isn't on cooldown attack.
        if (movementDistance <= meleeAttackRadius && attackCoolDown > attackCoolDownTime)
        {
            attackCoolDown = 0f;

            playerHurt.Play();
            player.GetComponentInChildren<HealthSystem>().gameObject.SendMessage("TakeDamage", damage);
        }
    }
}
