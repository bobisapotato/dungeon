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

    private int maxEnemies = 1;
    private int minEnemies = 1;
    private int maxPaths = 50;
    private int minPaths = 10;

    private Vector3 startPos;
    private Quaternion startRot;

    private EnemyPathMovement enemyMovementScript;

    private GameObject enemyInstance;

    // Start is called before the first frame update
    void Awake()
    {
        numberOfEnemies = GetNumberOfEnemiesInRoom();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            paths = new GameObject[numberOfPathPositions = GetNumbeOfPathPositions()];

            for (int j = 0; j < numberOfPathPositions; j++)
            {
                paths[j] = Instantiate(pathPosPrefab, transform, false);
                paths[j].transform.localPosition = GetPathPos();
            }

            startPos = GetStartPos();
            startRot = GetStartRot(startPos);

            enemyInstance = Instantiate(enemyPrefab, transform, false);
            enemyInstance.transform.localPosition = startPos;
            enemyInstance.transform.localRotation = startRot;

            enemyMovementScript = enemyInstance.GetComponent<EnemyPathMovement>();

            enemyMovementScript.pathPositions = paths;
            enemyMovementScript.player = player;
        }
    }

    int GetNumberOfEnemiesInRoom()
    {
        int num = Random.Range(minEnemies, maxEnemies);

        return num;
    }

    int GetNumbeOfPathPositions()
    {
        int num = Random.Range(minPaths, maxPaths);

        return num;
    }

    Vector3 GetStartPos()
    {
        Vector3 pos;

        pos.x = Random.Range(-5f, 5f);
        pos.y = 0f;
        pos.z = Random.Range(-5f, 5f);

        return pos;
    }

    Quaternion GetStartRot(Vector3 pos)
    {
        Vector3 direction = (paths[0].transform.position - pos).normalized;
        Quaternion rot = Quaternion.LookRotation
            (new Vector3(direction.x, 0f, direction.z));

        return rot;
    }

    Vector3 GetPathPos()
    {
        Vector3 pos;

        pos.x = Random.Range(-7.5f, 7.5f);
        pos.y = 0f;
        pos.z = Random.Range(-7.5f, 7.5f);

        return pos;
    }
}
