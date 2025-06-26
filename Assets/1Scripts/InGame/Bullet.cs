using System;
using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField,Header("弾の速度")] private float speed = 10f;
    [Networked] private TickTimer LifeTimer { get; set; }
    private int _damage = 1;
    private int _shooterId = -1;
    
    public override void FixedUpdateNetwork()
    {
        if(LifeTimer.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += transform.right * speed * Runner.DeltaTime;
    }

    public void Init()
    {
        LifeTimer = TickTimer.CreateFromSeconds(Runner, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TankController hitTank = other.GetComponent<TankController>();
            if (hitTank.PlayerId != _shooterId)
            {
                hitTank.TakeDamage(_damage);
                Runner.Despawn(Object);
            }
        }
    }
    private void OnDestroy()
    {
        EffectManager.Instance.PlayAnimation(EffectType.Smoke,transform.position);
    }
    public void SetShooterId(int id) { _shooterId = id; }
}
