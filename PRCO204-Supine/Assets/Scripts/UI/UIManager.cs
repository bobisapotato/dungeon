
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomCodeUI;

    // Start is called before the first frame update
 
    public void SetText(string text) => roomCodeUI.text = $"Code: {text}";
}
