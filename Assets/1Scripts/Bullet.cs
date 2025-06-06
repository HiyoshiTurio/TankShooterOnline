using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed = 10f;
    private int _damage = 1;
    private int _shooterId = -1;
    public override void Spawned()
    {
        Destroy(gameObject, 5f);
    }
    void FixedUpdate()
    {
        transform.position += transform.right * speed / 60;
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
