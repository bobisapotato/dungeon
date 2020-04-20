using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleItem : MonoBehaviour
{
    [SerializeField]
    private bool isIdle;

    [SerializeField]
    private bool isUp = false;

    float step = 0.002f;

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

        if (transform.position.y >= 1f)
        {
            isUp = true;
        }
        else if (transform.position.y <= 0.5f)
        {
            isUp = false;
        }
    }
}
