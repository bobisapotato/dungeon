using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject trap;
    [SerializeField]
    private GameObject rangedEnemyPrefab;
    [SerializeField]
    private GameObject meleeEnemyPrefab;
    [SerializeField]
    private GameObject pathPosPrefab;

    [SerializeField]
    private Transform player;

    private GameObject[] paths;

    private int numberOfEnemies;
    private int numberOfPathPositions;

    private int maxEnemies = 3;
    private int minEnemies = 2;
    private int maxPaths = 30;
    private int minPaths = 20;

    private Vector3 startPos;
    private Quaternion startRot;

    private EnemyPathMovement enemyMovementScript;

    private GameObject enemyInstance;

    public static bool isJustSlimes;
    public static bool isJustSkeletons;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = GetStartPos();
        enemyInstance = Instantiate(trap, transform, false);
        enemyInstance.transform.localPosition = new Vector3(startPos.x, -4f, startPos.z);

        // Set the number of enemies for the room.
        numberOfEnemies = GetNumberOfEnemiesInRoom();

        //paths = new GameObject[numberOfPathPositions = GetNumberOfPathPositions()];

        // All the enemies in a room share the same path.
        //for (int j = 0; j < numberOfPathPositions; j++)
        //{
        //    paths[j] = Instantiate(pathPosPrefab, transform, false);
        //    paths[j].transform.localPosition = GetPathPos();
        //}

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

            enemyInstance.GetComponent<EnemyMovement>().parentRoom = GetComponentInParent<Room>();

            // Set the paths and the target for the enemy.
            //enemyMovementScript = enemyInstance.GetComponent<EnemyPathMovement>();

            //enemyMovementScript.pathPositions = paths;
            //enemyMovementScript.player = player;
        }
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

    // Gets a random point for a path.
    Vector3 GetPathPos()
    {
        Vector3 pos;

        pos.x = Random.Range(-7.5f, 7.5f);
        pos.y = 1f;
        pos.z = Random.Range(-7.5f, 7.5f);

        return pos;
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
}
