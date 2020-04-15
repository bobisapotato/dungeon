using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Makes enemies follow and attack the player.
// Uses the animator window in Unity to simulate a FSM.

// Enemies do damage to the player when they are close
// enough.
public class EnemyMovement : MonoBehaviour
{
	public Transform target;
	public Transform eyes;

	private Animator anim;

	private bool isRotatingLeft = false;
	private bool isRotatingRight = false;
	private bool isWalking = false;

	[SerializeField]
	private float moveSpeed = 5f;
	[SerializeField]
	private float rotSpeed = 100f;
	[SerializeField]
	private float lookRadius = 2.5f;
	[SerializeField]
	private float stoppingDistance = 3f;

	private int rotTime;
	private int rotateWait;
	private int rotateLorR;
	private int walkTime;
	private int walkWait;

	private int damage = 10;
	private float attackCoolDown = 0f;
	private float attackCoolDownTime = 1f;
	private float attackRadius = 5f;

	void Start()
	{
		anim = GetComponent<Animator>();

		// Josie's code, check through
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update() 
	{
		// Get the distance to the player.
		float attackDistance = Vector3.Distance(target.position, transform.position);

		if (attackCoolDown <= attackCoolDownTime)
		{
			attackCoolDown += Time.deltaTime;
		}

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

		// If inside the radius, attack.
		if (attackDistance <= attackRadius && attackCoolDown > attackCoolDownTime)
		{
			attackCoolDown = 0f;
			HealthManager.playerHealth.TakeDamage(damage);
		}
	}

	void FixedUpdate()
	{
		// Get the distance to the player.
		float movementDistance = Vector3.Distance(target.position, eyes.position);

		// If inside the radius.
		if (movementDistance <= lookRadius)
		{
			FaceTarget();

			// Move towards the player.
			anim.SetBool("isFollowing", true);
			anim.SetBool("isWandering", false);

			if (movementDistance <= stoppingDistance)
			{
				// Attack here.
				FaceTarget();
			}
		}
		else
		{
			anim.SetBool("isFollowing", false);
		}
	}

	// Point towards the player.
	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation
			(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, 
			lookRotation, Time.deltaTime * 5f);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(eyes.position, lookRadius);
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}

	// Selects random values to simulate wandering around.
	IEnumerator Wander() 
	{
		rotTime = Random.Range(1, 2);
		rotateWait = Random.Range(1, 2);
		rotateLorR = Random.Range(0, 2);
		walkWait = Random.Range(1, 2);
		walkTime = Random.Range(1, 2);

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
