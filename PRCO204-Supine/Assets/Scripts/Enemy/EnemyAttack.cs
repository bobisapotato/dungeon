using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject player;

    private int damage = 10;

    private float attackCoolDown = 0f;
    private float attackCoolDownTime = 1f;
    private float attackRadius = 2.5f;

    private float shakeHitAmount = 1f;

    [SerializeField]
    private AudioSource playerHurt;

    [SerializeField]
    private bool isRanged = false;

    [SerializeField]
    private GameObject fireBallPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (isRanged)
        {
            attackRadius = 10f;
        }
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
            if (isRanged)
            {
                attackCoolDown = 0f;
                Vector3 newPos = transform.position + (transform.forward * 2f);
                // Instantiates the projectile to be shot.
                Instantiate(fireBallPrefab, newPos, transform.rotation);

            }
            else
            {
                attackCoolDown = 0f;

                if (CameraShake.shake <= shakeHitAmount)
                {
                    CameraShake.shake = shakeHitAmount;
                }

                playerHurt.Play();
                HealthManager.playerHealth.TakeDamage(damage);
            }
        }
    }
}
