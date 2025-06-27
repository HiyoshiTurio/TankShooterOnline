using System;
using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject smokeEffectPrefab;
    GameObject _activeEffectPool; //アクティブ状態のオブジェクトプール
    private static EffectManager _instance;
    public static EffectManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        _activeEffectPool = new GameObject("ActiveEffectPool");
        _activeEffectPool.transform.SetParent(transform);
    }

    public void PlayAnimation(EffectType effectType, Vector3 objTransform)
    {
        switch (effectType)
        {
            case(EffectType.Smoke) :
            {
                GameObject obj = InstanceFromPool();
                obj.transform.position = objTransform;
                break;
            }
            case (EffectType.Explosion):
            {
                break;
            }
        }
    }

    //このスクリプトがアタッチされているオブジェクトを親として、プールに格納する
    GameObject InstanceFromPool()
    {
        GameObject obj;
        if (transform.childCount <= 1)
        {
            obj = Instantiate(smokeEffectPrefab, transform);
        }
        else
        {
            obj = transform.GetChild(1).gameObject;
        }
        obj.SetActive(true);
        obj.transform.SetParent(_activeEffectPool.transform);
        Debug.Log(obj.name);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
    }
}

public enum EffectType
{
    Smoke,
    Explosion,
}