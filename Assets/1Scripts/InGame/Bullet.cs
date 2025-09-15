using UnityEngine;

public class Bullet_Contained : MonoBehaviour
{
    [SerializeField,Header("弾の速度")] private float speed = 2f;
    [SerializeField,Header("弾の消滅までの時間")] private float lifeTime = 1.0f;
    private int _id;
    private bool _isAlive = false;
    private Vector3 _initialPosition;
    private float _tick = 0.0f;
    private float _timer = 0.0f;
    public bool IsAlive => _isAlive;
    
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
    
     public void Render(float tick, float deltaTime)
     {
         _timer += deltaTime * (tick - _tick);
         if (_timer >= lifeTime)
         {
             Deactivate();
             transform.position = _initialPosition + transform.right * speed * lifeTime;
             return;
         }
         transform.position = _initialPosition + transform.right * speed * (deltaTime) * (tick - _tick);
    }
    // public void Render(float tick, float deltaTime)
    // {
    //     _timer += Time.deltaTime;
    //     if (_timer >= lifeTime)
    //     {
    //         Deactivate();
    //         transform.position = _initialPosition + transform.right * speed * lifeTime;
    //     }
    //     transform.position = _initialPosition + transform.right * speed * _timer;
    // }

    public void Deactivate()
    {
        if (_isAlive == false) return;
        _isAlive = false;
        EffectManager.Instance.PlayAnimation(EffectType.Smoke, transform.position);
        gameObject.SetActive(false);
    }
}
