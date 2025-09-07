using System;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : SimulationBehaviour
{
    private readonly List<Bullet_Contained> activeBullets = new(64);
    private readonly Stack<Bullet_Contained> deactiveBullets = new(64);
    [SerializeField] GameObject bulletPrefab;
    private static BulletContainer _instance;
    public static BulletContainer Instance => _instance;
    
    void Awake() 
    {
        _instance = this;
    }
    void Start() 
    {
        // 弾の初期化
        for (int i = 0; i < 64; i++) {
            var bullet = Instantiate(bulletPrefab, transform).GetComponent<Bullet_Contained>();
            deactiveBullets.Push(bullet);
        }
    }
    public override void FixedUpdateNetwork() 
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--) 
        {
            var bullet = activeBullets[i];
            // 弾の消去判定を行う
            if (!bullet.IsAlive) 
            {
                bullet.Deactivate();
                activeBullets.Remove(bullet);
                deactiveBullets.Push(bullet);
            }
        }
    }
    public override void Render() 
    {
        float tick = Runner.Tick - 1 + Runner.LocalAlpha;
        // 弾の位置を更新する
        foreach (var bullet in activeBullets) 
        {
            bullet.Render(tick, Runner.DeltaTime);
        }
    }
    public void FireBarrage(int playerId, Vector2 position, Quaternion direction, float tick)
    {
        Debug.Log("Firing barrage");
        Bullet_Contained bullet = deactiveBullets.Pop();
        activeBullets.Add(bullet);
        bullet.Init(playerId,position, direction,tick);
    }
}