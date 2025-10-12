using Fusion;
using UnityEngine;

public class TankController : NetworkBehaviour, INetworkInput
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletInstancePos;
    [SerializeField,Header("移動速度")] private float moveSpeed = 3f;
    [Networked] public NetworkButtons InputPrevious { get; set; }
    [Networked] private TickTimer Delay { get; set; }
    [Networked] private int Life{get; set;}
    [Networked] private string PlayerName{get; set;}
    private BulletContainer _bulletContainer;
    private PlayerView _playerView;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            PlayerName = Runner.IsServer? "Host" : "Client";
            // RPCでプレイヤー名を設定する処理をホストに実行してもらう
            SetNickName(PlayerName);
            
            SetPlayerId(Runner.LocalPlayer.PlayerId);
        }
        _playerView = GetComponent<PlayerView>();
        _playerView.SetTransform(transform);
        _playerView.SetNickName(PlayerName);
        _bulletContainer = BulletContainer.Instance;
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

        if (pressed.IsSet(MyButtons.Attack))
        {
            if(Object.HasInputAuthority)
                Rpc_FireBarrage();
        }
        
        // jump (check for pressed)
        if (pressed.IsSet(MyButtons.Jump)) 
        {
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void Rpc_FireBarrage(RpcInfo info = default)
    {
        _bulletContainer.FireBarrage(
            0, // プレイヤーID（誰が発射した弾か）
            transform.position, // 発射位置（弾幕がどこから発射されるか）
            barrelObj.transform.rotation, // 発射方向（弾幕がどの方向に発射されるか）
            info.Tick // ティック（弾幕がいつ発射されたか）
        );
    }

    public void TakeDamage(int damage)
    {
        Life -= damage;
    }

    private void TankMove(Vector3 vector)
    {
        float h = vector.x;
        float v = vector.y;
        Vector3 vec = new Vector3(h, v, 0).normalized;
        transform.position += moveSpeed * vec * Runner.DeltaTime;
        
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
}