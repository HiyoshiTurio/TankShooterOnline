using UnityEngine;

public class Bullet_Contained : MonoBehaviour
{
    [SerializeField,Header("弾の速度")] private float speed = 2f;
    [SerializeField,Header("弾の消滅までの時間")] private float lifeTime = 1.0f;
    private int _id;
    private bool _isAlive = false;
    private Vector2 _initialPosition;
    private float _tick = 0.0f;
    private float _timer = 0.0f;
    public bool IsAlive => _isAlive;
    
    public void Render(float tick, float deltaTime)
    {
        transform.position += transform.right * speed * (deltaTime) * (tick - _tick);
        _timer += deltaTime * (tick - _tick);
        if (_timer >= lifeTime)
        {
            Deactivate();
        }
    }
    public void Init(int id, Vector2 pos, Quaternion direction, float tick)
    {
        gameObject.SetActive(true);
        _id = id;
        _initialPosition = pos;
        transform.position = pos;
        transform.rotation = direction;
        _tick = tick;
        _isAlive = true;
        _timer = 0.0f;
    }

    public void Deactivate()
    {
        if (_isAlive == false) return;
        _isAlive = false;
        EffectManager.Instance.PlayAnimation(EffectType.Smoke, transform.position);
        gameObject.SetActive(false);
    }
}
