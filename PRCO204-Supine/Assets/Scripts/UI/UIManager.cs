using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Variables
    [SerializeField]
    private TextMeshProUGUI roomCodeUI;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("FindRoomCode", 0.25f);
    }

    // Updates the UI to display the server generated
    // room code in-game.
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
