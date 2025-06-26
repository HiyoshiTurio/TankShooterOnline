using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject smokeEffectPrefab;

    private static EffectManager _instance;
    public static EffectManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }
    public void PlayAnimation(EffectType effectType, Vector3 objTransform)
    {
        switch (effectType)
        {
            case(EffectType.Smoke) :
            {
                Instantiate(smokeEffectPrefab, objTransform, quaternion.identity);
                break;
            }
            case (EffectType.Explosion):
            {
                break;
            }
        }
    }
}

public enum EffectType
{
    Smoke,
    Explosion,
}