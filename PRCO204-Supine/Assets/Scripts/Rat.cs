using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
	float rotTime;
	float rotateWait;
	float rotateLorR;
	float walkWait;
	float walkTime;

	float moveSpeed = 10f;
	float rotSpeed = 100f;

	float distanceToCheckForObstacle = 4f;

	bool isWalking;
	bool isRotatingRight;
	bool isRotatingLeft;

	bool isWandering;
	bool isObstacleInTheWay;

	RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
		if (!isWandering && !isObstacleInTheWay)
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

	void FixedUpdate()
	{
		CheckForObstacle();
	}

	void CheckForObstacle()
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, distanceToCheckForObstacle))
		{
			isObstacleInTheWay = true;
		}
		else
		{
			isObstacleInTheWay = false;
		}
	}

	IEnumerator Wander()
	{
		isWandering = true;

		rotTime = UnityEngine.Random.Range(0, 1);
		rotateWait = UnityEngine.Random.Range(0, 0);
		rotateLorR = UnityEngine.Random.Range(0, 2);
		walkWait = UnityEngine.Random.Range(0, 0);
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
}
