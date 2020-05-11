using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

// Makes enemies follow and attack the player.
// Uses the animator window in Unity to simulate a FSM.

// Enemies do damage to the player when they are close
// enough.
public class EnemyMovement : MonoBehaviour
{
	private Transform target;

	private Animator anim;
	[SerializeField]
	private Animator childAnimator;

	private bool isRotatingLeft = false;
	private bool isRotatingRight = false;
	private bool isWalking = false;

	[SerializeField]
	private float moveSpeed = 5f;
	[SerializeField]
	private float rotSpeed = 100f;
	[SerializeField]
	private float lookRadius = 20f;

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


	void Awake()
	{
		anim = GetComponent<Animator>();

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
		rotTime = UnityEngine.Random.Range(0, 3);
		rotateWait = UnityEngine.Random.Range(1, 2);
		rotateLorR = UnityEngine.Random.Range(0, 2);
		walkWait = UnityEngine.Random.Range(0, 2);
		walkTime = UnityEngine.Random.Range(1, 3);

		anim.SetBool("isWandering", true);
		anim.SetBool("isFollowing", false);

		childAnimator.SetBool("isAngry", false);

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

		anim.SetBool("isWandering", false);
	}

	void PickAMovement()
	{
		if (!anim.GetBool("isWandering") && !anim.GetBool("isFollowing") && !isObstacleInTheWay)
		{
			StartCoroutine(Wander());
		}
		if (isObstacleInTheWay) 
		{
			transform.Rotate(transform.up * Time.deltaTime * (rotSpeed * 2));
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
		float movementDistance = Vector3.Distance(target.position, this.gameObject.transform.position);
		
		// If inside the radius.
		if (movementDistance 
			<= lookRadius && 
			parentRoom.playerInRoom)
		{
			// Move towards the player.
			anim.SetBool("isFollowing", true);
			anim.SetBool("isWandering", false);

			childAnimator.SetBool("isAngry", true);
		}
		else
		{
			anim.SetBool("isFollowing", false);
			childAnimator.SetBool("isAngry", false);
		}
	}
}
