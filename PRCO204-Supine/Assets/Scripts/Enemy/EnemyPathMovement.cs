using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathMovement : MonoBehaviour
{
    public Transform player;

    public GameObject[] pathPositions;

    private Rigidbody rb;

    private int currentPos = 0;

    [SerializeField]
    private float wanderingSpeed = 5f;
    [SerializeField]
    private float followingSpeed = 6.66f;
    [SerializeField]
    private float rotationSpeed = 7.5f;

    private float playerRadius = 5f;
    private float pointRadius = 0.5f;

    private float step;

    [SerializeField]
    private bool isFollowing;
    [SerializeField]
    private bool isMoving = true;
    [SerializeField]
    private bool isRotating = false;

    [SerializeField]
    Vector3 direction;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the distance to the player.
        float movementDistance = Vector3.Distance(player.transform.position, 
            transform.position);

        // If inside the radius.
        if (movementDistance <= playerRadius)
        {
            isFollowing = true;

            step = followingSpeed * Time.deltaTime;

            //StartCoroutine(FaceTarget(player.gameObject));

            if (!isRotating)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    player.transform.position, step);
            }
        }
        else
        {
            step = wanderingSpeed * Time.deltaTime;

            if (isFollowing)
            {
                StartCoroutine(PauseMovement());
                isFollowing = false;
            }
            else
            {
                MoveTowardsNextPoint();
            }
        }
    }

    void MoveTowardsNextPoint()
    {
        if (isMoving)
        {
            isFollowing = false;

            //StartCoroutine(FaceTarget(pathPositions[currentPos]));

            if (!isRotating)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    pathPositions[currentPos].transform.position, step);
            }

            // Get the distance to the point.
            float movementDistance = Vector3.Distance(pathPositions[currentPos].transform.position, 
                transform.position);

            if (movementDistance <= pointRadius)
            {
                NextPoint();
            }
        }
    }

    // Point towards the target GameObject.
    IEnumerator FaceTarget(GameObject target)
    {
        // Wait for a bit.
        yield return new WaitForSeconds(0.1f);

        isRotating = true;

        direction = (target.transform.position - transform.localPosition).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation,
                     lookRotation, Time.deltaTime * rotationSpeed);

        if (transform.rotation == lookRotation)
        {
            isRotating = false;
        }
    }

    IEnumerator PauseMovement()
    {
        // Backup and clear velocities.
        Vector3 linearBackup = rb.velocity;
        Vector3 angularBackup = rb.angularVelocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Finally freeze the body in place so forces like gravity or 
        // movement won't affect it.
        rb.constraints = RigidbodyConstraints.FreezeAll;

        isMoving = false;

        // Wait for a bit.
        yield return new WaitForSeconds(0.1f);

        // Unfreeze before restoring velocities.
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Restore the velocities.
        rb.velocity = linearBackup;
        rb.angularVelocity = angularBackup;

        isMoving = true;
    }

    void NextPoint()
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
