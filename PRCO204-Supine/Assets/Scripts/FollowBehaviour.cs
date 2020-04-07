using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{
    private float speed = 5f;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, PlayerMovement.playerPos, speed * Time.deltaTime);
    }
}
