using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    // Variables.
    public int health;

    private int minHealth = 0;
    private int maxHealth = 100;

    bool invulnerable = false;

    float coolDown = 1f;
    float shakeHitAmount = 1.5f;

    public Slider healthbar;

    private int startPlayerHealth = 100;
    private int oldHealth;

    private GameManager gameMan;

    private Animator playerAnimator;

    private Color original;
    [SerializeField]
    private Color tempColor;

    [SerializeField]
    private GameObject arm;
    [SerializeField]
    private GameObject swordArm;
    [SerializeField]
    private GameObject crossbowArm;
    [SerializeField]
    private GameObject leg1;
    [SerializeField]
    private GameObject leg2;
    [SerializeField]
    private GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        original = GetComponent<MeshRenderer>().material.color;

        health = startPlayerHealth;
        oldHealth = GetHealth();

        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (invulnerable)
        {
            if (body.GetComponent<MeshRenderer>().material.color == original)
            {
                body.GetComponent<MeshRenderer>().material.color = tempColor;

                arm.GetComponent<MeshRenderer>().material.color = tempColor;
                swordArm.GetComponent<MeshRenderer>().material.color = tempColor;
                crossbowArm.GetComponent<MeshRenderer>().material.color = tempColor;
                leg1.GetComponent<MeshRenderer>().material.color = tempColor;
                leg2.GetComponent<MeshRenderer>().material.color = tempColor;
            }
            else
            {
                body.GetComponent<MeshRenderer>().material.color = original;

                arm.GetComponent<MeshRenderer>().material.color = original;
                swordArm.GetComponent<MeshRenderer>().material.color = original;
                crossbowArm.GetComponent<MeshRenderer>().material.color = original;
                leg1.GetComponent<MeshRenderer>().material.color = original;
                leg2.GetComponent<MeshRenderer>().material.color = original;
            }

            Invoke("ResetVulnerability", coolDown);
        }

        if (GetHealth() != oldHealth)
        {
            healthbar.value = ((float)GetHealth() / 100);
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
        if (!invulnerable)
        {
            invulnerable = true;

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
    }

    void ResetVulnerability()
    {
        invulnerable = false;

        gameObject.GetComponent<MeshRenderer>().material.color = original;

        arm.GetComponent<MeshRenderer>().material.color = original;
        swordArm.GetComponent<MeshRenderer>().material.color = original;
        crossbowArm.GetComponent<MeshRenderer>().material.color = original;
        leg1.GetComponent<MeshRenderer>().material.color = original;
        leg2.GetComponent<MeshRenderer>().material.color = original;
        body.GetComponent<MeshRenderer>().material.color = original;


    }
}
