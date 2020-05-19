using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject trap;
    [SerializeField]
    private GameObject rangedEnemyPrefab;
    [SerializeField]
    private GameObject meleeEnemyPrefab;

    private GameObject[] paths;

    private int numberOfEnemies;

    private int maxEnemies = 3;
    private int minEnemies = 2;
    private int maxPaths = 30;
    private int minPaths = 20;

    private Vector3 startPos;
    private Quaternion startRot;

    private GameObject enemyInstance;

    public static bool isJustSlimes;
    public static bool isJustSkeletons;

    [SerializeField]
    private Animator enemyIconAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = GetStartPos();
        enemyInstance = Instantiate(trap, transform, false);
        enemyInstance.transform.localPosition = new Vector3(startPos.x, -1.5f, startPos.z);

        // Set the number of enemies for the room.
        numberOfEnemies = GetNumberOfEnemiesInRoom();

        // For each enemy, instantiate a number of paths.
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Set the start position and rotation for the enemy.
            startPos = GetStartPos();
            //startRot = GetStartRot(startPos);

            if (isJustSlimes)
            {
                SpawnSlime();
            }
            else if (isJustSkeletons)
            {
                SpawnSkeleton();
            }
            else
            {
                SpawnBoth();
            }
            

            // Instantiate the enemy, not in world space.
            enemyInstance.transform.localPosition = startPos;
            enemyInstance.transform.localRotation = enemyInstance.transform.rotation;

            CheckIfInsideWall();

            enemyInstance.GetComponent<EnemyMovement>().parentRoom = GetComponentInParent<Room>();
        }

        enemyIconAnimator = GameObject.FindGameObjectWithTag("EnemyIcon").GetComponent<Animator>();

        setEnemyCounterIcon();
    }

    // Returns a random number within a specified range.
    // This value is the number of enemies in the room.
    int GetNumberOfEnemiesInRoom()
    {
        int num = Random.Range(minEnemies, maxEnemies);

        return num;
    }

    // Returns a random number within a specified range.
    // This value is the number of paths for 1 enemy.
    int GetNumberOfPathPositions()
    {
        int num = Random.Range(minPaths, maxPaths);

        return num;
    }

    // Returns a random Vector3 for position.
    Vector3 GetStartPos()
    {
        Vector3 pos;

        pos.x = Random.Range(-5f, 5f);
        pos.y = 1f;
        pos.z = Random.Range(-5f, 5f);

        return pos;
    }

    // Returns a Quaternion for the enemy to
    // be facing the first target point.
    Quaternion GetStartRot(Vector3 pos)
    {
        Vector3 direction = (paths[0].transform.position - pos).normalized;
        Quaternion rot = Quaternion.LookRotation
            (new Vector3(direction.x, 0f, direction.z));

        return rot;
    }

    void SpawnSlime()
    {
        enemyInstance = Instantiate(meleeEnemyPrefab, transform, false);
        enemyInstance.transform.localPosition = new Vector3(startPos.x, 0.1f, startPos.z);
    }

    void SpawnBoth()
    {
        float rnd = Random.Range(0f, 1f);

        // 50/50 chance of spawning each type of enemy.
        if (rnd >= 0.5f)
        {
            enemyInstance = Instantiate(meleeEnemyPrefab, transform, false);
            enemyInstance.transform.localPosition = new Vector3(startPos.x, 0.1f, startPos.z);
        }
        else
        {
            enemyInstance = Instantiate(rangedEnemyPrefab, transform, false);
        }
    }

    void SpawnSkeleton()
    {
        enemyInstance = Instantiate(rangedEnemyPrefab, transform, false);
    }

    void CheckIfInsideWall()
    {
        RaycastHit hit;

        if (Physics.SphereCast(enemyInstance.transform.localPosition, 5f, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Hideable")
            {
                enemyInstance.transform.localPosition = GetStartPos();
                CheckIfInsideWall();
            }
        }
    }

    private void setEnemyCounterIcon()
    {
        // Sets the icon used in the enemy counter UI based on enemy types in scene. 
        // Uses the animator attached to icon, playing appropriate anim based on enemies. 

        if(isJustSlimes)
        {
            enemyIconAnimator.Play("SlimeIcon");
        }
        else if (isJustSkeletons)
        {
            enemyIconAnimator.Play("SkullIcon");
        }
        else
        {
            enemyIconAnimator.Play("SkullAndSlimeIcon");
        }
    }
}
