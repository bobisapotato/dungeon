using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class CameraHideObstacles : MonoBehaviour
{
    // Objects are hidden if they are in between player and camera
    // Once player is no longer obscured, obstacles is shown again

    // VARS
    #region
    private float maxRayRange = 30f;
    private RaycastHit hit;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] GameObject obstacle;

    //[SerializeField] GameObject[] obstacles;
    #endregion
    private GameObject lastWall;
    float alpha = 1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        checkPlayerObscured();

        if (obstacle)
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
            out hit))
        {
            // sets bool based on what raycast hits

            if (hit.transform == player.transform)
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
            if (lastWall != obstacle)
            {
                alpha = 1f;

                lastWall = obstacle;
            }
            else
            {
                //obstacle.GetComponent<MeshRenderer>().enabled = false;
                if (alpha > 0.5f)
                {
                    alpha -= 0.1f;
                }

                obstacle.GetComponent<MeshRenderer>().material.color = new Color
                    (obstacle.GetComponent<MeshRenderer>().material.color.r,
                    obstacle.GetComponent<MeshRenderer>().material.color.g,
                    obstacle.GetComponent<MeshRenderer>().material.color.b, alpha);
            }
        }
    }

    public void resetObstacle()
    {
        // sets old obstacle to visible again and sets obstacle value to null

        if (obstacle)
        {
            if (obstacle.GetComponent<MeshRenderer>())
            {
                obstacle.GetComponent<MeshRenderer>().material.color = new Color
                   (obstacle.GetComponent<MeshRenderer>().material.color.r,
                   obstacle.GetComponent<MeshRenderer>().material.color.g,
                   obstacle.GetComponent<MeshRenderer>().material.color.b, 1f);
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
