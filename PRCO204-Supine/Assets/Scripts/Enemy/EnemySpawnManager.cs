using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject pathPosPrefab;

    [SerializeField]
    private Transform player;

    private GameObject[] paths;

    private int numberOfEnemies;
    private int numberOfPathPositions;

    private int maxEnemies = 3;
    private int minEnemies = 1;
    private int maxPaths = 10;
    private int minPaths = 2;

    private Vector3 startPos;
    private Quaternion startRot;

    private EnemyPathMovement enemyMovementScript;

    private GameObject enemyInstance;

    // Start is called before the first frame update
    void Awake()
    {
        // Set the number of enemies for the room.
        numberOfEnemies = GetNumberOfEnemiesInRoom();

        // For each enemy, instantiate a number of paths.
        for (int i = 0; i < numberOfEnemies; i++)
        {
            paths = new GameObject[numberOfPathPositions = GetNumbeOfPathPositions()];

            for (int j = 0; j < numberOfPathPositions; j++)
            {
                paths[j] = Instantiate(pathPosPrefab, transform, false);
                paths[j].transform.localPosition = GetPathPos();
            }

            // Set the start position and rotation for the enemy.
            startPos = GetStartPos();
            startRot = GetStartRot(startPos);

            // Instantiate the enemy, not in world space.
            enemyInstance = Instantiate(enemyPrefab, transform, false);
            enemyInstance.transform.localPosition = startPos;
            enemyInstance.transform.localRotation = startRot;

            // Set the paths and the target for the enemy.
            enemyMovementScript = enemyInstance.GetComponent<EnemyPathMovement>();

            enemyMovementScript.pathPositions = paths;
            enemyMovementScript.player = player;
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
    int GetNumbeOfPathPositions()
    {
        int num = Random.Range(minPaths, maxPaths);

        return num;
    }

    // Returns a random Vector3 for position.
    Vector3 GetStartPos()
    {
        Vector3 pos;

        pos.x = Random.Range(-5f, 5f);
        pos.y = 0f;
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
        pos.y = 0f;
        pos.z = Random.Range(-7.5f, 7.5f);

        return pos;
    }
}
