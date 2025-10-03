using System.Collections;
using UnityEngine;

public class Rang : Ranged
{
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Pistol;
    }
    protected override void Update()
    {
        
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

    public void SetCanFireTrue()
    {
        canFire = true;
    }
}
