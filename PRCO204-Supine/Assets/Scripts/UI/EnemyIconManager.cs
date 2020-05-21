using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIconManager : MonoBehaviour
{
    private Animator enemyIconAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyIconAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (LevelGeneration.isFinished)
        {
            setEnemyCounterIcon();
            LevelGeneration.isFinished = false;
        }
    }

    // Sets the icon used in the enemy counter UI based on enemy types in scene. 
    // Uses the animator attached to icon, playing appropriate anim based on enemies. 
    private void setEnemyCounterIcon()
    {
        if (EnemySpawnManager.isJustSlimes && !EnemySpawnManager.isJustSkeletons)
        {
            enemyIconAnimator.Play("SlimeIcon");
        }
        if (EnemySpawnManager.isJustSkeletons && !EnemySpawnManager.isJustSlimes)
        {
            enemyIconAnimator.Play("SkullIcon");
        }
        if (!EnemySpawnManager.isJustSkeletons && !EnemySpawnManager.isJustSlimes)
        {
            enemyIconAnimator.Play("SkullAndSlimeIcon");
        }
    }
}
