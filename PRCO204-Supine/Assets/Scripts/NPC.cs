using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/* Makes enemies follow and attack the player */
public class NPC : MonoBehaviour
{
	public Transform target;

	private Animator anim;

	private bool isRotatingLeft = false;
	private bool isRotatingRight = false;
	private bool isWalking = false;

	private float moveSpeed = 3f;
	private float rotSpeed = 100f;
	private float lookRadius = 15f;
	private float stoppingDistance = 2f;

	private int rotTime;
	private int rotateWait;
	private int rotateLorR;
	private int walkTime;
	private int walkWait;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Update() 
	{
		if (!anim.GetBool("isWandering") && !anim.GetBool("isFollowing")) 
		{
			StartCoroutine(Wander());
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

	void FixedUpdate()
	{
		// Get the distance to the player
		float distance = Vector3.Distance(target.position, transform.position);

		// If inside the radius
		if (distance <= lookRadius)
		{
			FaceTarget();

			// Move towards the player
			anim.SetBool("isFollowing", true);
			anim.SetBool("isWandering", false);

			if (distance <= stoppingDistance)
			{
				// Attack
				FaceTarget();
			}
		}
		else 
		{
			anim.SetBool("isFollowing", false);
		}
	}

	// Point towards the player
	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}

	IEnumerator Wander() 
	{
		rotTime = Random.Range(1, 3);
		rotateWait = Random.Range(1, 4);
		rotateLorR = Random.Range(0, 3);
		walkWait = Random.Range(1, 4);
		walkTime = Random.Range(1, 5);

		anim.SetBool("isWandering", true);
		anim.SetBool("isFollowing", false);

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
}
