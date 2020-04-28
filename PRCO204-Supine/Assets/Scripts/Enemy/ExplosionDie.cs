using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("Die", 4f);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
