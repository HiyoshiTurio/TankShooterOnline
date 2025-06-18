using Fusion;
using UnityEngine;

public class TankController : NetworkBehaviour, INetworkInput
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 3f;
    [Networked] public NetworkString<_16> NickName { get; set; }
    [Networked] public NetworkButtons InputPrevious { get; set; }
    private PlayerView _playerView;
    //private NetworkRunner _networkRunner;
    //private Animator _animator;
    private int _playerId = -1;
    private int _health = 100;
    public int PlayerId => _playerId;

    public override void Spawned()
    {
        //_animator = GetComponent<Animator>();
        _playerView = GetComponent<PlayerView>();
        _playerView.SetNickName(NickName.Value);
        //_networkRunner = InGameManager.Instance.Runner;
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
        // var vector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        // vector.Normalize();
        // TankMove(vector);
        // RotateBarrel();
        if (GetInput<MyInput>(out var input) == false) return;

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

        // jump (check for pressed)
        if (pressed.IsSet(MyButtons.Jump)) {
            //DoJump();
        }
    }

    void TankMove(Vector3 vector)
    {
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
        Debug.Log($"ID: {shooterId}");
        //var tmp = _networkRunner.Spawn(bulletPrefab, instancePosition, direction);
        //tmp.GetComponent<Bullet>().SetShooterId(shooterId);
    }

    public void TakeDamage(int damage) { _health -= damage; }
    public void SetPlayerId(int playerId) { _playerId = playerId; }
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