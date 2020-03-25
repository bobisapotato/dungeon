using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHideObstacles : MonoBehaviour
{
    // Objects are hidden if they are in between player and camera
    // Once player is no longer obscured, obstacles is shown again

    // VARS
    #region
    private float maxRayRange = 15f;
    private RaycastHit hit;
    private GameObject player;
    private Camera mainCamera;
    [SerializeField] GameObject obstacle;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
   
    private void FixedUpdate()
    {
        Debug.Log("player is hidden = " + checkPlayerObscured());
        
        if(obstacle)
        {
            hideObstacle();
        }
    }

    public bool checkPlayerObscured()
    {
        // returns true if player is hidden from camera by an obstacle

        // draws ray to see in scene for testing
        Debug.DrawRay(mainCamera.transform.position,
            (player.transform.position - mainCamera.transform.position), Color.magenta);

        // checks if ray cast hits anything
        if (Physics.Raycast(mainCamera.transform.position, (player.transform.position - mainCamera.transform.position),
            out hit, maxRayRange))
        {
            
            // sets bool based on what raycast hits

            if(hit.transform == player.transform)
            {
                resetObstacle();
                return false;
            }
            else
            {
                newObstacle(hit.collider.gameObject);
                return true;
            }
        }
        else
        {
            resetObstacle();
            return false;
        }
    }

    public GameObject findObstacle()
    {
        // returns the gameobject which is hiding the player from the camera

        return obstacle;
    }

    public void hideObstacle()
    {
        // sets mesh renderer to inactive for given GO
        if (obstacle.GetComponent<MeshRenderer>())
        {
            obstacle.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void resetObstacle()
    {
        // sets old obstacle to visible again and sets obstacle value to null

        if (obstacle)
        {
            if (obstacle.GetComponent<MeshRenderer>())
            {
                obstacle.GetComponent<MeshRenderer>().enabled = true;
            }
            obstacle = null;
        }
    }

    public void newObstacle(GameObject newObstacle)
    {
        // sets new obstacle so old obstacle can be properly reset

        resetObstacle();
        obstacle = newObstacle;
    }


}
