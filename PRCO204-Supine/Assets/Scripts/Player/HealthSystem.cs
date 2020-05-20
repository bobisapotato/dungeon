using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    // Variables.
    public int health;

    private int minHealth = 0;
    private int maxHealth = 10;

    bool invulnerable = false;

    float coolDown = 1f;
    float shakeHitAmount = 1.5f;

    private int startPlayerHealth = 10;
    private int oldHealth;

    private GameManager gameMan;

    private Animator playerAnimator;

    [SerializeField]
    private Animator heartsUIAnim;

    // Start is called before the first frame update
    void Start()
    {
        health = startPlayerHealth;
        oldHealth = GetHealth();

        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (invulnerable)
        {
            Invoke("ResetVulnerability", coolDown);
        }


        oldHealth = GetHealth();


        if (oldHealth <= 0)
        {
            playerAnimator.Play("PlayerDie");
            Invoke("playerDie", 1f);
        }
    }

    private void playerDie()
    {
        gameMan.openDemoLose2();
    }

    // Getters and Setters.
    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Error handling.
        if(damage != 1)
        {
            damage = 1;
        }

        if (!invulnerable)
        {
            invulnerable = true;
            playerAnimator.SetBool("invulnerable", true);

            if (CameraShake.shake <= shakeHitAmount)
            {
                CameraShake.shake = shakeHitAmount;
            }

            if ((health - damage) <= minHealth)
            {
                health = minHealth;
            }
            else
            {
                health -= damage;
            }
        }

        updateHeartAnim();
    }

    public void Heal(int heal)
    {
        if ((health + heal) >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += heal;
        }

        updateHeartAnim();
    }

    void ResetVulnerability()
    {
        invulnerable = false;
        playerAnimator.SetBool("invulnerable", false);
    }

    private void updateHeartAnim()
    {
        heartsUIAnim.SetInteger("health", health);
    }
}
