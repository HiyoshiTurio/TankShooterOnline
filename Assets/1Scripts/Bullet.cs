using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField,Header("弾の速度")] private float speed = 10f;
    [Networked] private TickTimer LifeTimer { get; set; }
    private int _damage = 1;
    private int _shooterId = -1;
    public override void Spawned()
    {
        Destroy(gameObject, 5f);
    }
    public override void FixedUpdateNetwork()
    {
        if(LifeTimer.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += transform.right * speed * Runner.DeltaTime;
    }

    public void Init()
    {
        LifeTimer = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TankController hitTank = other.GetComponent<TankController>();
            if (hitTank.PlayerId != _shooterId)
            {
                hitTank.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
    public void SetShooterId(int id) { _shooterId = id; }
}
