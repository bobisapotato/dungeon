using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    // Items have a canvas which contains the 'X' prompt to show which items can be picked up. 
    // This script gets player transform and only shows prompt when they're within range 

    private GameObject player;
    [SerializeField]
    private float promptDistance;
    [SerializeField]
    private Animator promptAnimator;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerDistance();
    }

    public void checkPlayerDistance()
    {
        if(Vector3.Distance(player.transform.position, this.transform.position) <= promptDistance )
        {
            promptAnimator.SetBool("visible", true);
        }
        else
        {
            promptAnimator.SetBool("visible", false);
        }
    }
}
