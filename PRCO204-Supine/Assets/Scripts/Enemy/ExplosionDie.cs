using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kills the gameobject after 4 seconds.
// Designed for explosion prefabs, but
// can be used elsewhere.
public class ExplosionDie : MonoBehaviour
{
    void Awake()
    {
        Invoke("Die", 4f);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
