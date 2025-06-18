using Fusion;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    private NetworkRunner _runner;
    public static InGameManager Instance => _instance;
    public NetworkRunner Runner => _runner;
    private int _playerId = 1;
    void Awake()
    {
        if (_instance!= null && _instance!= this) Destroy(gameObject);
        else _instance = this;
    }
    public int GetPlayerId() { return _playerId++; }

    public void SetRunner(NetworkRunner runner)
    {
        _runner = runner;
    }
}
