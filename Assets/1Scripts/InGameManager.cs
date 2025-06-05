using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager Instance => _instance;
    private int _playerId = 1;
    void Awake()
    {
        if (_instance!= null && _instance!= this) Destroy(gameObject);
        else _instance = this;
    }
    public int GetPlayerId() { return _playerId++; }
}
