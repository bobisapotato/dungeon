using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Variables
    public static int level = 0;
    public static int numberOfRooms = 5;

    // Scene names.
    private string winScreen = "WinScreen";
    private string levelName = "Game";

    private LevelGeneration levelGen;

    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == levelName)
        {
            levelGen = GameObject.FindGameObjectWithTag("LevelGenManager").GetComponent<LevelGeneration>();

            if (levelGen != null)
            {
                levelGen.maximumRooms = numberOfRooms;
            }

            numberOfRooms += 5;
        }
    }

    // Manages the scene to be loaded next.
    public void LoadLevel()
    {
        level++;

        // Reset static values for next level.
        PlayerAttack.isHoldingRangedWeapon = false;
        PlayerAttack.isHoldingWeapon = false;

        switch (level)
        {
            case 1:
                EnemySpawnManager.isJustSlimes = true;
                EnemySpawnManager.isJustSkeletons = false;

                SceneManager.LoadScene(levelName);
                break;
            case 2:
                EnemySpawnManager.isJustSlimes = false;
                EnemySpawnManager.isJustSkeletons = true;

                SceneManager.LoadScene(levelName);
                break;
            case 3:
                EnemySpawnManager.isJustSlimes = false;
                EnemySpawnManager.isJustSkeletons = false;

                SceneManager.LoadScene(levelName);
                break;
            case 4:
                level = 0;
                numberOfRooms = 5;

                SceneManager.LoadScene(winScreen);
                break;
        }
    }
}
