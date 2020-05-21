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
        LevelManager.level = 0;

        SceneManager.LoadScene("DemoWin2");
    }
    public void openLoseScreen()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;
        LevelManager.level = 0;

        SceneManager.LoadScene("LoseScreen");
    }


    public void openMenu()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("MainMenu");
    }

    public void openInstructions()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("Objectives");
    }


	#endregion

}
