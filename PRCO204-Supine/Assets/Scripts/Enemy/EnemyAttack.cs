using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Variables.
    private GameObject player;

    [SerializeField]
    private int damage = 10;

    private float attackCoolDown = 0f;
    private float attackCoolDownTime = 1f;
    private float meleeAttackRadius = 3f;
    private float rangedAttackRadius = 20f;

    [SerializeField]
    private AudioSource playerHurt;

    [SerializeField]
    private bool isRanged = false;

    [SerializeField]
    private GameObject fireBallPrefab;

    private EnemyMovement moveScript;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (isRanged)
        {
            meleeAttackRadius = 10f;
        }

        moveScript = GetComponent<EnemyMovement>();

        animator = GetComponent<Animator>();
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
        if (movementDistance <= meleeAttackRadius && attackCoolDown > attackCoolDownTime 
            && moveScript.parentRoom.playerInRoom)
        {
            if (!isRanged)
            {
                attackCoolDown = 0f;

                playerHurt.Play();
                player.GetComponentInChildren<HealthSystem>().gameObject.SendMessage("TakeDamage", damage);
            }
        }

        if (isRanged && movementDistance <= rangedAttackRadius && attackCoolDown > attackCoolDownTime 
            && moveScript.parentRoom.playerInRoom && GetComponent<Animator>().GetBool("isFollowing"))
        {
            attackCoolDown = 0f;

            Vector3 newPos = transform.position + (transform.forward * 2f);

            // Instantiates the projectile to be shot.
            Instantiate(fireBallPrefab, newPos, transform.rotation);
            animator.Play("ShootProjectile");
        }
    }
}
