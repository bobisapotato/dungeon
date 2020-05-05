using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyRunAway : MonoBehaviour
{
	private Transform target;

	private bool isRotatingLeft = false;
	private bool isRotatingRight = false;
	private bool isWalking = false;

	[SerializeField]
	private float moveSpeed = 5f;
	[SerializeField]
	private float rotSpeed = 100f;
	[SerializeField]
	private float lookRadius = 10f;

	private int rotTime;
	private int rotateWait;
	private int rotateLorR;
	private int walkTime;
	private int walkWait;

	private RaycastHit hit;

	private bool isObstacleInTheWay;

	private float distanceToCheckForObstacle = 5f;

	[SerializeField]
	private Transform leftCorner;
	[SerializeField]
	private Transform rightCorner;

	[HideInInspector]
	public Room parentRoom;

	bool isWandering;
	bool isRunningAway;
	bool isRotating;

	void Awake()
	{
		// Josie's code, check through
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		PickAMovement();
	}

	void FixedUpdate()
	{
		CheckDistanceToTarget();
		CheckForObstacle();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}

	// Selects random values to simulate wandering around.
	IEnumerator Wander()
	{
		isWandering = true;

		rotTime = UnityEngine.Random.Range(0, 3);
		rotateWait = UnityEngine.Random.Range(1, 2);
		rotateLorR = UnityEngine.Random.Range(0, 2);
		walkWait = UnityEngine.Random.Range(0, 2);
		walkTime = UnityEngine.Random.Range(1, 3);

		yield return new WaitForSeconds(walkWait);
		isWalking = true;

		yield return new WaitForSeconds(walkTime);
		isWalking = false;

		yield return new WaitForSeconds(rotateWait);
		if (rotateLorR == 1)
		{
			isRotatingRight = true;
			yield return new WaitForSeconds(rotTime);
			isRotatingRight = false;
		}
		if (rotateLorR == 2)
		{
			isRotatingLeft = true;
			yield return new WaitForSeconds(rotTime);
			isRotatingLeft = false;
		}

		isWandering = false;
	}

	void PickAMovement()
	{
		if (!isWandering && !isRunningAway && !isObstacleInTheWay)
		{
			StartCoroutine(Wander());
		}
		if (isObstacleInTheWay)
		{
			transform.Rotate(transform.up * Time.deltaTime * (rotSpeed * 2));
		}
		if (isRunningAway)
		{
			RunAway();
		}
		if (isRotatingRight)
		{
			transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
		}
		if (isRotatingLeft)
		{
			transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
		}
		if (isWalking)
		{
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}
	}

	void CheckForObstacle()
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, distanceToCheckForObstacle)
			|| Physics.Raycast(leftCorner.position, transform.forward, out hit, distanceToCheckForObstacle)
			|| Physics.Raycast(rightCorner.position, transform.forward, out hit, distanceToCheckForObstacle))
		{
			if (hit.collider.gameObject.tag != "Player")
			{
				isObstacleInTheWay = true;
			}
			else
			{
				isObstacleInTheWay = false;
			}
		}
		else
		{
			isObstacleInTheWay = false;
		}
	}

	void CheckDistanceToTarget()
	{
		// Get the distance to the player.
		float movementDistance = Vector3.Distance(target.position, transform.position);

		// If inside the radius.
		if (movementDistance <= lookRadius)
		{
			// Move away from the player.
			isRunningAway = true;
			isWandering = false;
		}
		else
		{
			isRunningAway = false;
		}
	}

	void RunAway()
	{
		Vector3 direction = transform.position - target.position;

		StartCoroutine(FaceTarget(-direction));

		if (!isRotating)
		{
			transform.position = Vector3.MoveTowards(transform.position, -direction, moveSpeed * Time.deltaTime);
		}
	}

	// Point towards the target GameObject.
	IEnumerator FaceTarget(Vector3 direction)
	{
		isRotating = true;

		// Wait for a bit.
		yield return new WaitForSeconds(0.1f);

		// Sets it to a quaternion.
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

		// A new quaternion with the same vector3 and scalar values, except all positive.
		Quaternion lookRotationPositive = new Quaternion(Mathf.Sqrt(Mathf.Pow(lookRotation.x, 2f)),
			Mathf.Sqrt(Mathf.Pow(lookRotation.y, 2f))
		, Mathf.Sqrt(Mathf.Pow(lookRotation.z, 2f)), Mathf.Sqrt(Mathf.Pow(lookRotation.w, 2f)));

		// A new quaternion with the same vector3 and scalar values, except all negative.
		Quaternion lookRotationNegative = new Quaternion(lookRotationPositive.x * -1f,
			lookRotationPositive.y * -1f, lookRotationPositive.z * -1f, lookRotationPositive.w * -1f);

		// Rotate the player towards the original quaternion.
		transform.rotation = Quaternion.Slerp(transform.rotation,
					 lookRotation, Time.deltaTime * rotSpeed);

		// Round up each value of the positive to 1 decimal place.
		lookRotationPositive = new Quaternion((float)Math.Round((double)lookRotationPositive.x, 1),
			(float)Math.Round((double)lookRotationPositive.y, 1), (float)Math.Round((double)lookRotationPositive.z, 1),
			(float)Math.Round((double)lookRotationPositive.w, 1));

		// Round up each value of the negative to 1 decimal place.
		lookRotationNegative = new Quaternion((float)Math.Round((double)lookRotationNegative.x, 1),
			(float)Math.Round((double)lookRotationNegative.y, 1), (float)Math.Round((double)lookRotationNegative.z, 1),
			(float)Math.Round((double)lookRotationNegative.w, 1));

		// Create a temp copy of the transform.rotate and round each value up to
		// one decimal place.
		Quaternion tempTransformRot = new Quaternion((float)Math.Round((double)transform.rotation.x, 1), (float)Math.Round((double)transform.rotation.y, 1),
		(float)Math.Round((double)transform.rotation.z, 1), (float)Math.Round((double)transform.rotation.w, 1));

		// Compare the Y and W values.
		// If the temp transform.rotate has both the same Y and same W of either 
		// positive or negative lookRotations, then the enemy is facing
		// the next target and can stop rotating.
		if (tempTransformRot.y == lookRotationPositive.y || tempTransformRot.y == lookRotationNegative.y)
		{
			if (tempTransformRot.w == lookRotationPositive.w || tempTransformRot.w == lookRotationNegative.w)
			{
				isRotating = false;
			}
		}

		// This is an extra step check to make sure it definitely works.
		if (transform.rotation == lookRotation || transform.rotation == lookRotationPositive || transform.rotation == lookRotationNegative)
		{
			isRotating = false;
		}

	}
}
