using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to items to make them
// float up and down. Procedural Animation?
public class IdleItem : MonoBehaviour
{
    // Variables
    public bool isIdle;

    private bool isUp = false;

    private float step = 0.002f;

    private float maxHeight = -2.75f;
    private float minHeight = -3.25f;

    private SphereCollider col;

    void Awake()
    {
        col = gameObject.GetComponent<SphereCollider>();

        if (gameObject.tag == "Key")
        {
            col.enabled = false;

            Invoke("EnablePickUp", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUp)
        {
            transform.position += new Vector3(0f, step, 0f);
        }
        else 
        {
            transform.position -= new Vector3(0f, step, 0f);
        }

        if (transform.position.y >= maxHeight)
        {
            isUp = true;
        }
        else if (transform.position.y <= minHeight)
        {
            isUp = false;
        }
    }

    void EnablePickUp()
    {
        col.enabled = true;
    }
}
