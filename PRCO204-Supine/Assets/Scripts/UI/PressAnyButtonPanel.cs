using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyButtonPanel : MonoBehaviour
{
    // Press any button panel shows at the start of the Game
    // Once player input is recieved, it animated to become transparent

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // NEEDS TO BE LINKED UP WITH UNITY INPUT SYSTEM COS IDK 

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("FadingPanel");
        }
    }

    
}
