using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sends message to unlock room when list of enemies have all been destroyed
// Should only exist in a GO with the 'Room' script
public class EnemysUnlockRoom : MonoBehaviour
{
    // Variables.
    #region
    [SerializeField] private List<GameObject> activeEnemyList = new List<GameObject>();
    private Room room;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Get the associated room - through error i.
        if (this.GetComponent<Room>())
        {
            room = this.GetComponent<Room>();
        }
        else
        {
            Debug.LogError("EnemyUnlockRoom script found without an associated Room script.");
        }
    }


    public void enemyKilled(GameObject enemy)
    {
        // When an enemy dies, it will send a message here so it is removed from the list of active enemies.
        activeEnemyList.Remove(enemy);

        if (activeEnemyList.Count == 0)
        {
            // No enemies left in room.
            room.unlockAllDoors();
        }
    }

   
    
}
