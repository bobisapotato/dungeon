using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnSensor : MonoBehaviour
{

    // Each room spawn point has 4 sensors.
    // These identify any rooms surrounding the spawn point, to dictate what requirements
    // the new room should meet to avoid doors to nowhere.

    // VARS
    public string direction;
    private BoxCollider sensorCollider;
    private Room foundRoom;
    private bool mustHaveDoor;
    private bool mustNotHaveDoor;

    // Start is called before the first frame update
    void Start()
    {
        // collider is off, toggled on to check relevant surroundings
        sensorCollider = gameObject.GetComponent<BoxCollider>();
        sensorCollider.enabled = false;

        checkSensor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkMustHave()
    {
        return mustHaveDoor;
    }

    public bool checkMustNot()
    {
        return mustNotHaveDoor;
    }

    public string getRequiredDoorDir()
    {
        return direction;
    }

    public bool checkRoomWasFound()
    {
        if(foundRoom)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void checkSensor()
    {
        StartCoroutine("toggleSensorCollider");
    }

    public IEnumerator toggleSensorCollider()
    {
        yield return new WaitForSeconds(0.3f);
        sensorCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        sensorCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // when something alerts the sensor, it checks the associated room and 
        // updates the 'need related room' bool

        //if (other.CompareTag("Room"))
        //{

            foundRoom = other.gameObject.GetComponentInParent<Room>();
            //Debug.Log("Found room " + foundRoom.name + " in the direction " + direction);

            if (direction == "N")
            {
                if (foundRoom.sDoor)
                {
                    mustHaveDoor = true;
                }
                else mustNotHaveDoor = true;
            }
            if (direction == "E")
            {
                if (foundRoom.wDoor)
                {
                    mustHaveDoor = true;
                    
                }
                else mustNotHaveDoor = true;
            }
            if (direction == "S")
            {
                if (foundRoom.nDoor)
                {
                    mustHaveDoor = true;
                    
                }
                else mustNotHaveDoor = true;
            }
            if (direction == "W")
            {
                if (foundRoom.eDoor)
                {
                    mustHaveDoor = true;

                }
                else mustNotHaveDoor = true;
            }
        //}

    }

        
}
