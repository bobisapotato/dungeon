using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour
{
    // Variables.
    public GameObject noweapon;
    public GameObject crossbow;
    public GameObject sword;
    public GameObject key;

    public void setAll(bool value)
    {
        noweapon.SetActive(value);
        crossbow.SetActive(value);
        sword.SetActive(value);
    }

    public void activateKey()
    {
        Debug.Log("showingKey");
        key.SetActive(true);
    }

}
