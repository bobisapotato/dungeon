using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Objects such as walls should have this script.
// It works with CameraHideAllWalls to make all the walls transparent so player and enemies aren't obscured.
// Should be attached directly to the model, not the parent GO.
// Materials need to be dragged and dropped in to the inspector.
public class HideableObject : MonoBehaviour
{
    public Material opaqueMaterial;
    public Material transparentmaterial;
    

    private float alpha = 1f;
   
   
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Hideable";

        transparentmaterial.color = new Color(transparentmaterial.color.r, transparentmaterial.color.g, transparentmaterial.color.b, 0.5f);
        // Setup for 2 materials to swap between - not render mode, but col.
    }

    // Changes the alpha value of the material depending whether you
    // want to hide or show the object.
    public void hideObject()
    {
        if (alpha > 0.5f)
        {
            alpha -= 0.1f;
            transparentmaterial.color = new Color(transparentmaterial.color.r, transparentmaterial.color.g, transparentmaterial.color.b, alpha);
        }

        this.gameObject.GetComponent<MeshRenderer>().material = transparentmaterial;
    }

    public void showObject()
    {
        if (alpha < 1f)
        {
            alpha += 0.1f;
            transparentmaterial.color = new Color(transparentmaterial.color.r, transparentmaterial.color.g, transparentmaterial.color.b, alpha);
        }
        if (alpha == 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = opaqueMaterial;
        }
        else if (alpha < 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = transparentmaterial;
        }
    }
}
