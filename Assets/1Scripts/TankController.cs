using Fusion;
using UnityEngine;

public class TankController : NetworkBehaviour, INetworkInput
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 3f;
    [Networked] public NetworkString<_16> NickName { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    private PlayerView _playerView;
    //private Animator _animator;
    private int _playerId = -1;
    private int _health = 100;
    public int PlayerId => _playerId;

    public override void Spawned()
    {
        //_animator = GetComponent<Animator>();
        //_playerId = InGameManager.Instance.GetPlayerId();
        _playerView = GetComponent<PlayerView>();
        _playerView.SetNickName(NickName.Value);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<MyInput>(out var input) == false)
        {
            Debug.Log("No input");
            return;
        };
        // compute pressed/released state
        var pressed = input.Buttons.GetPressed(ButtonsPrevious);
        var released = input.Buttons.GetReleased(ButtonsPrevious);

        // store latest input as 'previous' state we had
        ButtonsPrevious = input.Buttons;

        // movement (check for down)
        var vector = default(Vector3);

        if (input.Buttons.IsSet(MyButtons.Forward)) { vector.y += 1; }
        if (input.Buttons.IsSet(MyButtons.Backward)) { vector.y -= 1; }

        if (input.Buttons.IsSet(MyButtons.Left)) { vector.x  -= 1; }
        if (input.Buttons.IsSet(MyButtons.Right)) { vector.x += 1; }

        TankMove(vector);

        // jump (check for pressed)
        if (pressed.IsSet(MyButtons.Jump)) {
            //DoJump();
            Debug.Log("Jump");
        }

        if (pressed.IsSet(MyButtons.Attack))
        {
            Debug.Log("Attack");
            Shot(barrelObj.transform.position, barrelObj.transform.rotation, _playerId);
        }
    }
    void TankMove(Vector3 vector)
    {
        Debug.Log("Move: " + vector);
        float h = vector.x;
        float v = vector.y;
        Vector3 vec = new Vector3(h, v, 0).normalized;
        transform.position += vec * moveSpeed / 60;
        if (h > 0.1 || v > 0.1 || h < -0.1 || v < -0.1)
        {
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void RotateBarrel()
    {
        Vector2 tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg;
        barrelObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shot(Vector3 instancePosition,Quaternion direction, int shooterId)
    {
        GameObject bullet = Instantiate(bulletPrefab, instancePosition, direction);
        bullet.GetComponent<Bullet>().SetShooterId(shooterId);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }

    // public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    // public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    // public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    // public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    // public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    // public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    // public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    // public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    // public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    // public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    // public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    // public void OnInput(NetworkRunner runner, NetworkInput input) { }
    // public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    // public void OnConnectedToServer(NetworkRunner runner){}
    // public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    // public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    // public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    // public void OnSceneLoadDone(NetworkRunner runner) { }
    // public void OnSceneLoadStart(NetworkRunner runner) { }
}

public struct MyInput : INetworkInput
{
    public NetworkButtons Buttons;
    //public Vector3 AimDirection;
}

enum MyButtons
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3,
    Jump = 4,
    Attack = 5,
}