using System;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : SimulationBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    private readonly List<Bullet_Contained> _activeBullets = new(64);
    private readonly Stack<Bullet_Contained> _deactiveBullets = new(64);
    private static BulletContainer _instance;
    public static BulletContainer Instance => _instance;
    
    void Awake() 
    {
        _instance = this;
    }
    public void Start()
    {
        // 弾の初期化
        for (int i = 0; i < 64; i++) 
        {
            var bullet = Instantiate(bulletPrefab, transform).GetComponent<Bullet_Contained>();
            _deactiveBullets.Push(bullet);
        }
    }
    public override void FixedUpdateNetwork() 
    {
        for (int i = _activeBullets.Count - 1; i >= 0; i--) 
        {
            var bullet = _activeBullets[i];
            // 弾の消去判定を行う
            if (!bullet.IsAlive) 
            {
                _activeBullets.Remove(bullet);
                _deactiveBullets.Push(bullet);
            }
        }
    }
    public override void Render() 
    {
        float tick = Runner.Tick - 1 + Runner.LocalAlpha;
        // 弾の位置を更新する
        foreach (var bullet in _activeBullets) 
        {
            bullet.Render(tick, Runner.DeltaTime);
        }
    }
    public void FireBarrage(int playerId, Vector2 position, Quaternion direction, float tick)
    {
        Bullet_Contained bullet = _deactiveBullets.Pop();
        _activeBullets.Add(bullet);
        bullet.Init(playerId,position, direction,tick);
    }
}