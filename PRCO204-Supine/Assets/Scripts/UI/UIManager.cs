using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomCodeUI;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("FindRoomCode", 0.25f);
    }

    void FindRoomCode() 
    {
        if (WSConnection.roomCode != null)
        {
            roomCodeUI.text = WSConnection.roomCode;
        }
        else
        {
            roomCodeUI.text = "----";
        }
    }
}
