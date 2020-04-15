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
    
	public void closeGame()
    {
        Application.Quit();
    }
    // Demo 1 
    #region
    public void openDemo()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void openDemoMenu()
    {
        SceneManager.LoadScene("DemoMenu");
    }
    #endregion

    // Demo 2
    #region
    public void openDemoScene2()
    {
        SceneManager.LoadScene("DemoScene2");
    }

    public void openDemoWin2()
    {
        SceneManager.LoadScene("DemoWin2");
    }
    public void openDemoLose2()
    {
        SceneManager.LoadScene("DemoLose2");
    }

    public void openDemoMenu2()
    {
        SceneManager.LoadScene("DemoMenu2");
    }
	#endregion

}
