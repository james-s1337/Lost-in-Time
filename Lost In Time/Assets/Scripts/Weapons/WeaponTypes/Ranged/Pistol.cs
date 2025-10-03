using System.Collections;
using UnityEngine;

public class Pistol : Ranged
{
    protected override void Awake()
    {
        base.Awake();   
        weaponType = WeaponCatalogue.Pistol;
    }
    public override void Fire()
    {
        base.Fire();
    }

    protected override void SpawnBullet()
    {
        base.SpawnBullet();

        // Play special effect for pistol
    }

    protected override IEnumerator SpawnBulletThread()
    {
        return base.SpawnBulletThread();
    }
}
