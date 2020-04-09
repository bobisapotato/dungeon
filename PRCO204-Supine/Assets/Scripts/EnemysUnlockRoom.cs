using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysUnlockRoom : MonoBehaviour
{
    // Sends message to unlock room when list of enemies have all been destroyed
    // Should only exist in a GO with the 'Room' script

    // VARS
    #region
    [SerializeField] private List<GameObject> activeEnemyList = new List<GameObject>();
    private Room room;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // get the associated room - through error i
        if (this.GetComponent<Room>())
        {
            room = this.GetComponent<Room>();
        }
        else
        {
            Debug.LogError("EnemyUnlockRoom script found without an associated Room script.");
        }

        //// populate list from GOs with enemy tag in children 
        //GameObject[] tempArray = this.GetComponentsInChildren<GameObject>();
        //foreach(GameObject g in tempArray)
        //{
        //    // loop through all GameObject Children

        //    if(g.tag == "Enemy")
        //    {
        //        // Add to list if it is tagged
        //        activeEnemyList.Add(g);
        //    }
        //}
    }

    private void Update()
    {
        // for testing

       
    }



    public void enemyKilled(GameObject enemy)
    {
        // when an enemy dies, it will send a message here so it is removed from the list of active enemies
        activeEnemyList.Remove(enemy);

        if (activeEnemyList.Count == 0)
        {
            // no enemies left in room

            room.unlockAllDoors();
        }
    }

   
    
}
