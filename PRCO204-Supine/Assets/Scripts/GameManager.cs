using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Manages loading of scenes

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            openDemoMenu();
        }
    }
    public void openDemo()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void openDemo2()
    {
        SceneManager.LoadScene("Demo_Release2");
    }

    public void openDemoMenu()
    {
        SceneManager.LoadScene("DemoMenu");
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
