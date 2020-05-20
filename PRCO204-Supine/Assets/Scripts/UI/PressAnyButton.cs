using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start screen shows game name and credits.
// This is set up so as soon as any input is recieved, the menu screen is loaded. 
public class PressAnyButton : MonoBehaviour
{
    // Variables.
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
        // Works for controller triggers as well.
        if(x >= 0.5)
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        gameManager.openMenu();
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
