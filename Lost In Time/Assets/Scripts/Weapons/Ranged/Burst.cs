using System.Collections;
using UnityEngine;

public class Burst : Ranged
{
    private float burstCooldown;
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Burst;
    }
    protected override void Start()
    {
        base.Start();
        burstCooldown = weaponStats.baseReloadTime;
    }
    protected override void Update()
    {
        if (Time.time >= timeSinceLastFire + burstCooldown && !isFiring)
        {
            canFire = true;
        }
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
