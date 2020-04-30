using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : MonoBehaviour
{
    // Objects such as walls should have this script.
    // It works with CameraHideAllWalls to make all the walls transparent so player and enemies aren't obscured.
    // Should be attached directly to the model, not the parent GO.
    // Materials need to be dragged and dropped in to the inspector.


    public Material opaqueMaterial;
    public Material transparentmaterial;
    public Material cutoutMaterial;


    public Color cutoutOpaque;
    public Color cutoutHide;
    // Start is called before the first frame update
    void Start()
    {
        // Setup for full cutout mode
        cutoutMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        cutoutOpaque = cutoutMaterial.color;
        cutoutHide = new Color(cutoutOpaque.r, cutoutOpaque.g, cutoutOpaque.g, 0.5f);

        this.gameObject.tag = "Hideable";

        transparentmaterial.color = new Color(transparentmaterial.color.r, transparentmaterial.color.g, transparentmaterial.color.b, 0.5f);
        // setup for 2 materials to swap between - not render mode, but col
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hideObject()
    {
        //hide the obj
        Debug.Log("changing col");
        //this.gameObject.GetComponent<MeshRenderer>().material.color = cutoutHide;
        this.gameObject.GetComponent<MeshRenderer>().material = transparentmaterial;
    }

    public void showObject()
    {
        //this.gameObject.GetComponent<MeshRenderer>().material.color = cutoutOpaque;
        this.gameObject.GetComponent<MeshRenderer>().material = opaqueMaterial;
    }
}
