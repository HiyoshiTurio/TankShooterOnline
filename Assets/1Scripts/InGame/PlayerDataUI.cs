using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    public string playerName;
    void Start()
    {
        nameInputField.onValueChanged.AddListener(delegate { SetPlayerName(nameInputField.text); });
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }
}
