using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ranged : WeaponType
{
    [SerializeField] GameObject bulletPrefab;
    protected List<GameObject> bulletPrefabPool = new List<GameObject>();
    protected int bulletIndex;

    private GameObject bulletPoolObject;

    protected virtual void Start()
    {
        bulletPoolObject = new GameObject(weaponType.ToString());
        if (weaponType == WeaponCatalogue.Flamethrower)
        {
            bulletPoolObject.transform.parent = transform;
            bulletPoolObject.AddComponent<FollowParentPosition>();
        }

        UpdateBulletPool();
    }

    protected virtual void Update()
    {
        if (Time.time >= timeSinceLastFire + weaponStats.baseCooldown && !isFiring)
        {
            canFire = true;
        }
    }

    public override void Fire()
    {
        if (!canFire || !weaponStats)
        {
            return;
        }
        base.Fire();

        canFire = false;
        isFiring = true;

        if (weaponStats.numOfShots > 1)
        {
            StartCoroutine(SpawnBulletThread());
        }
        else
        {
            SpawnBullet();
            SetTimeSinceLastFired();
        }
    }

    protected virtual IEnumerator SpawnBulletThread()
    {
        for (int i = 0; i < weaponStats.numOfShots; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(weaponStats.baseCooldown);
        }
        SetTimeSinceLastFired();
    }

    protected void SetTimeSinceLastFired()
    {
        timeSinceLastFire = Time.time;
        isFiring = false;
    }

    protected virtual void SpawnBullet()
    {
        int numOfPrefabs = bulletPrefabPool.Count;
        if (numOfPrefabs <= 0)
        {
            return;
        }

        if (bulletIndex >= numOfPrefabs)
        {
            bulletIndex = 0;
        }

        GameObject bullet = bulletPrefabPool[bulletIndex];

        bullet.transform.position = transform.position;

        Projectile proj = bullet.GetComponent<Projectile>();

        proj.SetWeaponStats(weaponStats);
        proj.SetTravelTime(weaponStats.baseTravelTime);
        proj.SetKnockback(weaponStats.baseKnockback);
        proj.SetPiercing(weaponStats.piercing);
        proj.SetProjectileSpeed(weaponStats.baseProjectileSpeed);

        if (weaponType == WeaponCatalogue.Boomerang)
        {
            proj.SetTravelDirection(new Vector3(1, 0, 0));
            bullet.GetComponent<Boomerang>().SetTravelDistance(weaponStats.baseTravelDistance);
        }

        if (player.core.Movement.facingDir == -1)
        {
            bullet.transform.Rotate(0f, 180f, 0f);

            if (weaponType == WeaponCatalogue.Boomerang)
            {
                proj.SetTravelDirection(new Vector3(-1, 0, 0));
            }
        }

        bullet.SetActive(true);

        bulletIndex++;
    }

    public override void ApplyWeaponModifier(IWeaponModifier mod)
    {
        base.ApplyWeaponModifier(mod);
        UpdateBulletPool();
    }

    public override void RemoveWeaponModifier(IWeaponModifier mod)
    {
        base.RemoveWeaponModifier(mod);
        UpdateBulletPool();
    }

    protected virtual void UpdateBulletPool()
    {
        if (bulletPoolObject == null || weaponType == WeaponCatalogue.Boomerang)
        {
            return;
        }

        int fireRate = Mathf.RoundToInt(weaponStats.baseTravelTime / weaponStats.baseCooldown);
        if (weaponType == WeaponCatalogue.Burst)
        {
            fireRate = Mathf.RoundToInt(weaponStats.baseTravelTime / weaponStats.baseReloadTime);
        }

        int bulletCountDiff = fireRate - bulletPrefabPool.Count;

        if (weaponStats.numOfShots > 1)
        {
            bulletCountDiff *= weaponStats.numOfShots;
        }

        if (bulletCountDiff >= 0)
        {
            for (int i = 0; i < bulletCountDiff + 1; i++)
            {
                GameObject newBulletPrefab = Instantiate(bulletPrefab);
                newBulletPrefab.GetComponent<Projectile>().Initialize();
                newBulletPrefab.transform.parent = bulletPoolObject.transform;

                bulletPrefabPool.Add(newBulletPrefab);
            }
        }
    }
}
