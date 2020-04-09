using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private GameObject[] pathPositions;

    private int currentPos = 0;

    private float wanderingSpeed = 3.33f;
    private float followingSpeed = 4f;
    private float radius = 5f;
    private float step;

    private bool isFollowing;

    // Update is called once per frame
    void Update()
    {
        // Get the distance to the player.
        float movementDistance = Vector3.Distance(target.position, transform.position);

        // If inside the radius.
        if (movementDistance <= radius) 
        {
            step = followingSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            isFollowing = true;
        }
        else
        {
            step = wanderingSpeed * Time.deltaTime;

            if (isFollowing)
            {
                isFollowing = false;
            }
            else
            {
                MoveTowardsNextPoint();
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == pathPositions[currentPos])
        {
            int oldPos = currentPos;

            currentPos = Random.Range(0, pathPositions.Length);

            if (currentPos == oldPos)
            {
                currentPos++;

                if (currentPos >= pathPositions.Length)
                {
                    currentPos = 0;
                }
            }
        }
    }

    void MoveTowardsNextPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, pathPositions[currentPos].transform.position, step);
        isFollowing = false;
    }
}
