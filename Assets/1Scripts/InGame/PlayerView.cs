using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameLabel;
    [SerializeField] private TextMeshPro idLabel;
    private Transform _playerTransform;
    public void SetNickName(string nickName)
    {
        nameLabel.text = nickName;
    }
    public void SetPlayerId(int playerId)
    {
        idLabel.text = "ID: " + playerId;
    }
    private void LateUpdate()
    {
        nameLabel.transform.rotation = Quaternion.identity;
        nameLabel.transform.position = _playerTransform.position + new Vector3(0, 1.5f, 0);
        idLabel.transform.rotation = Quaternion.identity;
        idLabel.transform.position = _playerTransform.position + new Vector3(0, 1.0f, 0);
    }
    public void SetTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
