using Fusion;
using UnityEngine;

public class TankController : NetworkBehaviour, INetworkInput
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 3f;
    
    [Networked,OnChangedRender(nameof(OnNickNameChanged))] 
    public NetworkString<_16> NickName { get; set; }
    [Networked] public NetworkButtons InputPrevious { get; set; }
    private PlayerView _playerView;
    private int _playerId = -1;
    private int _health = 100;
    public int PlayerId => _playerId;

    public override void Spawned()
    {
        _playerView = GetComponent<PlayerView>();
        _playerView.SetTransform(transform);
        _playerView.SetNickName(NickName.Value);
        
        if (Object.HasInputAuthority)
        {
            // RPCでプレイヤー名を設定する処理をホストに実行してもらう
            Rpc_SetNickName(PlayerData.NickName);
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void Rpc_SetNickName(string nickName) {
        NickName = nickName;
    }
    public void OnNickNameChanged() {
        // 更新されたプレイヤー名をテキストに反映する
        _playerView.SetNickName(NickName.Value);
    }

    void Update()
    {
        if (Input.GetButton("Jump")) {
            //DoJump();
            Debug.Log("Jump");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Shot(barrelObj.transform.position, barrelObj.transform.rotation, _playerId);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<PlayerInput>(out var input) == false) return;

        // compute pressed/released state
        var pressed = input.Buttons.GetPressed(InputPrevious);
        var released = input.Buttons.GetReleased(InputPrevious);

        // store latest input as 'previous' state we had
        InputPrevious = input.Buttons;

        // movement (check for down)
        var vector = default(Vector3);

        if (input.Buttons.IsSet(MyButtons.Forward)) { vector.y += 1; }
        if (input.Buttons.IsSet(MyButtons.Backward)) { vector.y -= 1; }

        if (input.Buttons.IsSet(MyButtons.Left)) { vector.x  -= 1; }
        if (input.Buttons.IsSet(MyButtons.Right)) { vector.x += 1; }

        TankMove(vector);
        RotateBarrel(barrelObj, input.MousePos);

        // jump (check for pressed)
        if (pressed.IsSet(MyButtons.Jump)) {
        }
    }

    void TankMove(Vector3 vector)
    {
        float h = vector.x;
        float v = vector.y;
        Vector3 vec = new Vector3(h, v, 0).normalized;
        transform.position += vec * moveSpeed * Runner.DeltaTime;
        
        if (h > 0.1 || v > 0.1 || h < -0.1 || v < -0.1)
        {
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void RotateBarrel(GameObject rotationObj, Vector3 mousePos)
    {
        Vector2 tmp = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float angle = Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg;
        rotationObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shot(Vector3 instancePosition,Quaternion direction, int shooterId)
    {
        Debug.Log($"ID: {shooterId}");
        //var tmp = _networkRunner.Spawn(bulletPrefab, instancePosition, direction);
        //tmp.GetComponent<Bullet>().SetShooterId(shooterId);
    }

    public void TakeDamage(int damage) { _health -= damage; }
    public void SetPlayerId(int playerId) { _playerId = playerId; }
}
public static class PlayerData
{
    public static string NickName {
        get => PlayerPrefs.GetString("NickName", "No Name");
        set => PlayerPrefs.SetString("NickName", value);
    }
}