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
   
    public void openGame()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        SceneManager.LoadScene("Game");
    }

    public void openWin()
    {
        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;
        LevelManager.level = 0;

        SceneManager.LoadScene("WinScreen");
    }
    public void openLose()
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


    

}
