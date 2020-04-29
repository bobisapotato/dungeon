using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyMaterial : MonoBehaviour
{
    // Start is called before the first frame update

    public float alpha = 0.5f;
    void Start()
    {
        // set up slime to be transparent
        this.GetComponent<MeshRenderer>().material.color = new Color
                   (this.GetComponent<MeshRenderer>().material.color.r,
                   this.GetComponent<MeshRenderer>().material.color.g,
                   this.GetComponent<MeshRenderer>().material.color.b, alpha);
    }
    private void Update()
    {
        this.GetComponent<MeshRenderer>().material.color = new Color
                   (this.GetComponent<MeshRenderer>().material.color.r,
                   this.GetComponent<MeshRenderer>().material.color.g,
                   this.GetComponent<MeshRenderer>().material.color.b, alpha);
    }


}
