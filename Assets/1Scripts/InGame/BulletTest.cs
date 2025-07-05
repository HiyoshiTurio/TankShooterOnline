using Unity.VisualScripting;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    [SerializeField,Header("弾の速度")] private float speed = 10f;
    [SerializeField,Header("弾の消滅までの時間")] private float lifeTime = 1.0f;
    private bool _isAlive = false;
    private Vector2 _initialPosition;
    float _timer = 0.0f;
    public bool IsAlive => _isAlive;
    
    public void Render(float tick, float deltaTime)
    {
        transform.position += transform.right * speed * deltaTime * tick;
        _timer += deltaTime * tick;
        if (_timer >= lifeTime)
        {
            Deactivate();
        }
    }
    private void OnDestroy()
    {
        EffectManager.Instance.PlayAnimation(EffectType.Smoke,transform.position);
    }
    public void Init(Vector2 pos, Quaternion direction)
    {
        gameObject.SetActive(true);
        _initialPosition = pos;
        transform.position = pos;
        transform.rotation = direction;
        _isAlive = true;
        _timer = 0.0f;
    }

    public void Deactivate()
    {
        _isAlive = false;
        this.GameObject().SetActive(false);
    }
}
