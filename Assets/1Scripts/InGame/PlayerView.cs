using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameLabel;
    private Transform _playerTransform;
    public void SetNickName(string nickName) {
        nameLabel.text = nickName;
    }
    private void LateUpdate()
    {
        nameLabel.transform.rotation = Quaternion.identity;
        nameLabel.transform.position = _playerTransform.position + new Vector3(0, 1.5f, 0);
    }
    public void SetTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
