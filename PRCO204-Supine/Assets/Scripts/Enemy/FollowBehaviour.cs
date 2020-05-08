using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class FollowBehaviour : StateMachineBehaviour
{
    private float speed = 3.33f;
	private float rotSpeed = 100f;

	private bool isRotating;
	private Vector3 direction;
	private Quaternion lookRotation;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		Vector3 newPos = new Vector3(PlayerMovement.playerPos.x, animator.transform.position.y, PlayerMovement.playerPos.z);

		FaceTarget(PlayerMovement.playerPos, animator);

		if (!isRotating)
		{
			animator.transform.position = Vector3.MoveTowards(animator.transform.position, newPos, speed * Time.deltaTime);
		}
    }

	void FaceTarget(Vector3 target, Animator anim)
	{
		isRotating = true;

		// Gets the direction the enemy needs to face.
		direction = (target - anim.transform.position).normalized;

		// Sets it to a quaternion.
		lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

		// A new quaternion with the same vector3 and scalar values, except all positive.
		Quaternion lookRotationPositive = new Quaternion(Mathf.Sqrt(Mathf.Pow(lookRotation.x, 2f)),
			Mathf.Sqrt(Mathf.Pow(lookRotation.y, 2f))
		, Mathf.Sqrt(Mathf.Pow(lookRotation.z, 2f)), Mathf.Sqrt(Mathf.Pow(lookRotation.w, 2f)));

		// A new quaternion with the same vector3 and scalar values, except all negative.
		Quaternion lookRotationNegative = new Quaternion(lookRotationPositive.x * -1f,
			lookRotationPositive.y * -1f, lookRotationPositive.z * -1f, lookRotationPositive.w * -1f);

		// Rotate the player towards the original quaternion.
		anim.transform.rotation = Quaternion.Slerp(anim.transform.rotation,
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
		Quaternion tempTransformRot = new Quaternion((float)Math.Round((double)anim.transform.rotation.x, 1), (float)Math.Round((double)anim.transform.rotation.y, 1),
		(float)Math.Round((double)anim.transform.rotation.z, 1), (float)Math.Round((double)anim.transform.rotation.w, 1));

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
		if (anim.transform.rotation == lookRotation || anim.transform.rotation == lookRotationPositive || anim.transform.rotation == lookRotationNegative)
		{
			isRotating = false;
		}
	}
}
