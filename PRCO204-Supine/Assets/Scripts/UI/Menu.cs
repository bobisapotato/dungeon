using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI[] textBoxes;
    public string[] events;

    private int numberOfOptions;
    private int selectedOption;

    PlayerControls controls;

    float x;
    float y;

    float timer;
    float coolDown = 0.1f;

    AudioSource mainMenuAudio;

    [SerializeField]
    AudioClip selectClip;
    [SerializeField]
    AudioClip selectButtonPressClip;

    LevelManager levelGen;
    GameManager gameMan;

    void Awake()
    {
        controls = new PlayerControls();

        // Controller input.
        controls.UI.MoveAcross.performed += ctx => x
        = ctx.ReadValue<float>();
        controls.UI.MoveAcross.canceled += ctx => x = 0f;

        controls.UI.MoveDown.performed += ctx => y
        = ctx.ReadValue<float>();
        controls.UI.MoveDown.canceled += ctx => y = 0f;

        controls.UI.Select.performed += ctx => Select();

        mainMenuAudio = GetComponent<AudioSource>();
        levelGen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        numberOfOptions = textBoxes.Length - 1;
        selectedOption = 0;

        // Set the visual indicator for which option you are on.
        textBoxes[selectedOption].color = new Color32(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= coolDown)
        {
            timer = 0f;

            // Input telling it to go up.
            if (y >= 0.5f)
            {
                mainMenuAudio.clip = selectClip;

                selectedOption += 1;

                // If at end of list go back to top.
                if (selectedOption > numberOfOptions)
                {
                    selectedOption = 0;
                }

                SetTextColors();

                mainMenuAudio.Play();
            }
            // Input telling it to go down.
            else if (y <= -0.5f)
            {
                mainMenuAudio.clip = selectClip;

                selectedOption -= 1;

                // If at end of list go back to top.
                if (selectedOption < 0)
                {
                    selectedOption = numberOfOptions;
                }

                SetTextColors();

                mainMenuAudio.Play();
            }
        }
    }

    // Make sure all others will be black.
    void SetTextColors()
    {
        foreach(TextMeshProUGUI text in textBoxes)
        {
            // Make sure all others will be black.
            text.color = new Color32(0, 0, 0, 255);
        }

        // Set the visual indicator for which option you are on.
        textBoxes[selectedOption].color = new Color32(255, 255, 255, 255);
    }

    void Select()
    {
        mainMenuAudio.clip = selectButtonPressClip;
        mainMenuAudio.Play();

        switch (selectedOption)
        {
            case 0:
                Invoke(events[selectedOption], Mathf.Epsilon);
                break;
            case 1:
                Invoke(events[selectedOption], Mathf.Epsilon);
                break;
            case 2:
                Invoke(events[selectedOption], Mathf.Epsilon);
                break;
        }
    }

    public void LoadGame()
    {
        levelGen.LoadLevel();
    }

    public void LoadInstructions()
    {
        gameMan.openDemoInstructions2();
    }

    public void CloseGame()
    {
        gameMan.closeGame();
    }

    public void LoadMenu()
    {
        gameMan.openDemoMenu2();
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
