using System.Collections;
using UnityEngine;

public class Burst : Ranged
{
    private float burstCooldown;
    private void Awake()
    {
        weaponType = WeaponCatalogue.Burst;
    }
    protected override void Start()
    {
        base.Start();
        burstCooldown = weaponData.burstCooldown;
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
