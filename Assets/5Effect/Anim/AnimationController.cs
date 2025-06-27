using UnityEngine;

public class AnimationController : MonoBehaviour
{
    void ReruenPool()
    {
        EffectManager.Instance.ReturnToPool(gameObject);
    }
}
