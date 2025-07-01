using Fusion;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private static WeaponManager _instance;
    public static WeaponManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
    
    // public void RPC_Fire(Vector3 instancePosition, Quaternion direction, int shooterId)
    // {
    //     var tmp = Runner.Spawn(bulletPrefab, instancePosition, direction, Object.InputAuthority,
    //         (runner, o) => { o.GetComponent<Bullet>().Init(); });
    //     tmp.GetComponent<Bullet>().SetShooterId(shooterId);
    // }
    
    public void RPC_SendFire(Vector3 instancePosition, Quaternion direction, int shooterId)
    {
        
    }

    public void RPC_RelayFire(Vector3 instancePosition, Quaternion direction, int shooterId)
    {
        
    }
}