using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Variables 
    [SerializeField]
    private GameObject keyPrefab;

    [SerializeField] 
    private int health = 100;

    private float shakeHitAmount = 0.5f;
    private float shakeDieAmount = 0.5f;
    float coolDown = 1f;

    private Room parentRoom;

    private EnemyCountManager enemyCountManager;
    public GameObject crossbowPrefab;

    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject smallSlime;
    [SerializeField]
    private GameObject squidgePrefab;
    [SerializeField]
    private GameObject squidgePrefabSmall;

    [SerializeField]
    private bool isSlime = false;

    private bool hasDied = false;

    private Animator animator;

    private Color original;
    [SerializeField]
    private Color tempColor;

    bool changeColor;

    // Start is called before the first frame update
    void Start()
    {
        original = GetComponentInChildren<MeshRenderer>().material.color;

        if (GetComponentInParent<Room>())
        {
            parentRoom = GetComponentInParent<Room>();
        }
        else
        {
            Debug.LogError("No parent room found attached to enemy " + this.gameObject.name + ". Enemies need to be children of the room object.");
        }

        enemyCountManager = GameObject.FindGameObjectWithTag("EnemyCountMan").GetComponent<EnemyCountManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasDied)
        {
            hasDied = true;

            // Skulls/small slimes should just explode.
            if (!isSlime)
            {
                if (gameObject.name.Contains("Ranged"))
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                }
                else
                {
                    Instantiate(squidgePrefabSmall, transform.position, transform.rotation);
                }
            }
            // Big slimes spawn smaller slimes.
            else
            {
                Instantiate(squidgePrefab, transform.position, transform.rotation);

                int i = Random.Range(2, 3);

                for (int j = 0; j < i; j++)
                {
                    parentRoom.enemyCountManager.enemyCount++;
                    Vector3 newPos = new Vector3(transform.position.x - (j * 0.5f), -3.8f, transform.position.z - (j * 0.5f));
                    GameObject go = Instantiate(smallSlime, newPos, transform.rotation, parentRoom.transform);
                    parentRoom.addEnemiesToRoom(go.GetComponent<EnemyHealth>());
                }
            }

            if (CameraShake.shake <= shakeDieAmount)
            {
                CameraShake.shake = shakeDieAmount;
            }

            Die();
        }

        if (changeColor)
        {
            if (gameObject.GetComponentInChildren<MeshRenderer>().material.color == original)
            {
                gameObject.GetComponentInChildren<MeshRenderer>().material.color = tempColor;
            }
            else
            {
                gameObject.GetComponentInChildren<MeshRenderer>().material.color = original;
            }
        }
    }


    // Takes the amount sent to the method away from the enemies health
    // then applies a knockback affect to the enemy.
    public void TakeDamage(int amount)
    {
        //if (isSlime)
        //{
        //    animator.Play("SlimeHurt");
        //}

        health -= amount;

        if (CameraShake.shake <= shakeHitAmount)
        {
            CameraShake.shake = shakeHitAmount;
        }

        if (!changeColor)
        {
            changeColor = true;

            Invoke("ResetColour", coolDown);
        }
    }
    
    void Die()
    {
        triggerCrossbowCheck();
        

        parentRoom.enemyKilled(this);

        if (parentRoom.enemyCountManager.enemyCount == 0)
        {
            Instantiate(keyPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
    
    public void triggerCrossbowCheck()
    {
        // If half the enemies have been killed, spawn a crossbow
        if (enemyCountManager.halfEnemyCount == enemyCountManager.enemyCount)
        {
            if(!enemyCountManager.checkDroppedCrossbow())
            {
                Instantiate(crossbowPrefab, transform.position, crossbowPrefab.transform.rotation);
                enemyCountManager.dropCrossbow();
            }
        }
    }

    void ResetColour()
    {
        changeColor = false;
        gameObject.GetComponentInChildren<MeshRenderer>().material.color = original;
    }
}
