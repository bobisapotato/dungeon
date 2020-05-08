using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyButton : MonoBehaviour
{
    // Start screen shows game name and credits.
    // This is set up so as soon as any input is recieved, the menu screen is loaded. 

    private GameManager gameManager;

    private PlayerControls controls;

    float x;

    void Awake()
    {
        controls = new PlayerControls();

        // Controller input.
        controls.UI.PressAnyButton.performed += ctx => LoadGame();

        controls.UI.PressAnyValue.performed += ctx => x
        = ctx.ReadValue<float>();
        controls.UI.PressAnyValue.canceled += ctx => x = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this.gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // NEEDS TO BE LINKED UP WITH UNITY INPUT SYSTEM COS IDK 

        if(x >= 0.5)
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        gameManager.openDemoMenu2();
    }


    // Required for the input system.
    void OnEnable()
    {
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.UI.Disable();
    }
}
