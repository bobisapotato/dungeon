using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject player;

    private int damage = 10;

    private float attackCoolDown = 0f;
    private float attackCoolDownTime = 1f;
    private float attackRadius = 5f;

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
        if (movementDistance <= attackRadius && attackCoolDown > attackCoolDownTime)
        {
            attackCoolDown = 0f;

            HealthManager.playerHealth.TakeDamage(damage);

            // Debug until we have UI set up for player health.
            Debug.Log(HealthManager.playerHealth.GetHealth());
        }
    }
}
