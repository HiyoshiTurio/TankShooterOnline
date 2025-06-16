using UnityEngine;
using Fusion;

public class TestPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 3f;
    [Networked] public NetworkString<_16> NickName { get; set; }
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
        // movement (check for down)
        var vector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        vector.Normalize();

        TankMove(vector);

        // jump (check for pressed)
        if (Input.GetButton("Jump")) {
            //DoJump();
            Debug.Log("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attack");
            Shot(barrelObj.transform.position, barrelObj.transform.rotation, _playerId);
        }
        RotateBarrel();
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
        GameObject bullet = Instantiate(bulletPrefab, instancePosition, direction);
        bullet.GetComponent<Bullet>().SetShooterId(shooterId);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}
