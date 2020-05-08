using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyButton : MonoBehaviour
{
    // Start screen shows game name and credits.
    // This is set up so as soon as any input is recieved, the menu screen is loaded. 

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = this.gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // NEEDS TO BE LINKED UP WITH UNITY INPUT SYSTEM COS IDK 

        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.openDemoMenu2();
        }
    }

    
}
