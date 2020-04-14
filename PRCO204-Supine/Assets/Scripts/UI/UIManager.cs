using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text roomCodeUI;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FindRoomCode", 0.25f);
    }

    void FindRoomCode() 
    {
        roomCodeUI.text = WSConnection.roomCode;
    }
}
