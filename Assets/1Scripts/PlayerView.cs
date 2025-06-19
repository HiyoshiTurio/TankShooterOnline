using System;
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameLabel;
    public Transform playerTransform;
    public void SetNickName(string nickName) {
        nameLabel.text = nickName;
    }
    private void LateUpdate()
    {
        nameLabel.transform.rotation = Quaternion.identity;
        nameLabel.transform.position = playerTransform.position + new Vector3(0, 1.5f, 0);
    }
}
