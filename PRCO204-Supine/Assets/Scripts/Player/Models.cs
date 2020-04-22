using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject noweapon;
    public GameObject crossbow;
    public GameObject sword;

    public void setAll(bool value)
    {
        noweapon.SetActive(value);
        crossbow.SetActive(value);
        sword.SetActive(value);
    }

}
