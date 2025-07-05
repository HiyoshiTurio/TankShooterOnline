using System;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : SimulationBehaviour
{
    private readonly List<BulletTest> activeBullets = new(64);
    private readonly Stack<BulletTest> inActiveBullets = new(64);
    [SerializeField] GameObject bulletPrefab;
    static BulletContainer _instance;
    public static BulletContainer Instance => _instance;
    
    void Awake() {
        _instance = this;
    }
    void Start() {
        // 弾の初期化
        for (int i = 0; i < 64; i++) {
            var bullet = Instantiate(bulletPrefab, transform).GetComponent<BulletTest>();
            inActiveBullets.Push(bullet);
        }
    }
    public override void FixedUpdateNetwork() {
        for (int i = activeBullets.Count - 1; i >= 0; i--) {
            var bullet = activeBullets[i];
            // 弾の消去判定を行う
            if (!bullet.IsAlive) {
                bullet.Deactivate();
                activeBullets.Remove(bullet);
                inActiveBullets.Push(bullet);
            }
        }
    }
    
    public override void Render() {
        float tick = (Runner.Tick - 1 + Runner.LocalAlpha);
        // 弾の位置を更新する
        foreach (var bullet in activeBullets) {
            bullet.Render(tick, Runner.DeltaTime);
        }
    }
    
    public void InstanceBullet(Vector2 position, Quaternion direction)
    {
        BulletTest bullet = inActiveBullets.Pop();
        activeBullets.Add(bullet);
        bullet.Init(position, direction);
    }
}