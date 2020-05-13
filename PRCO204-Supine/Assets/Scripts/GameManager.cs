using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Manages loading of scenes

    private void Update()
    {
        //if(Input.GetKeyDown("escape"))
        //{
        //    openDemoMenu2();
        //}
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
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("Demo_Release2");
    }

    public void openDemoWin2()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("DemoWin2");
    }
    public void openDemoLose2()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;
        LevelManager.level = 0;

        SceneManager.LoadScene("DemoLose2");
    }


    public void openDemoMenu2()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("DemoMenu2");
    }

    public void openDemoInstructions2()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("Objectives");
    }


	#endregion

}
