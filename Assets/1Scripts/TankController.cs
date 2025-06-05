using Fusion;
using UnityEngine;

public class TankController : NetworkBehaviour
{
    [SerializeField] private GameObject barrelObj;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 3f;
    private int _playerId = -1;
    private int _health = 100;
    private Animator _animator;
    public int PlayerId => _playerId;
    public override void Spawned()
    {
        _animator = GetComponent<Animator>();
        _playerId = InGameManager.Instance.GetPlayerId();
    }
    void Update()
    {
        PlayerInput();
    }
    public override void FixedUpdateNetwork()
    {
        TankMove();
    }
    void PlayerInput()
    {
        RotateBarrel();
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrefab, barrelObj.transform.position, barrelObj.transform.rotation);
            bullet.GetComponent<Bullet>().SetShooterId(_playerId);
        }
    }

    void TankMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
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
    public void TakeDamage(int damage) { _health -= damage; }
}
