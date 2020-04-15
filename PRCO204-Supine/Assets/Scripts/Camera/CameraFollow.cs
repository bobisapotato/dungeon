using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tracks the camera to the player via a transform with a smoothdamp applied.
public class CameraFollow : MonoBehaviour
{
    //Variables
    [SerializeField] Transform player;
    [SerializeField] float smoothTime;
    private Vector3 velocity = Vector3.zero;

    
    // Start is called before the first frame update
    void Start()
    {
        // Josie's code, check you're happy w it

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    //Creates a target position from the players current position on the 
    //game and a origin which is where the camera currently is. It then 
    //moves the camera to the target position via a transform with 
    //smoothdamp applied with a velocity. The time it takes to do so can 
    //be adjusted.
    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Vector3 origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(origin, targetPosition, ref velocity, smoothTime);
    }
}
