using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameLabel;
    public void SetNickName(string nickName) {
        nameLabel.text = nickName;
    }
}
