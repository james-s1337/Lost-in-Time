using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ranged : WeaponType
{
    [SerializeField] protected RangedWeaponData weaponData;

    protected int numOfShots;

    [SerializeField] protected List<GameObject> bulletPrefabPool;

    private Vector2 mouseWorldPosition;
    private Vector2 direction;
    protected int bulletIndex;
    protected virtual void Awake()
    {
        canFire = true;
        numOfShots = weaponData.numOfShots;
    }

    protected virtual void Start()
    {
        if (weaponData)
        {
            cooldown = weaponData.cooldown;
        }
    }

    protected virtual void Update()
    {
        if (Time.time >= timeSinceLastFire + cooldown && !isFiring)
        {
            canFire = true;
        }
    }

    public override void Fire()
    {
        if (!canFire || !weaponData)
        {
            return;
        }
        base.Fire();

        canFire = false;
        isFiring = true;

        if (numOfShots > 1)
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
        for (int i = 0; i < numOfShots; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(cooldown);
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

        /*
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = mouseWorldPosition - (Vector2)transform.position;
        // Calculate angle in radians
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        // Convert to degrees
        float angleDeg = angleRad * Mathf.Rad2Deg;
        */

        GameObject bullet = bulletPrefabPool[bulletIndex];

        bullet.transform.position = transform.position;
        bullet.GetComponent<Projectile>().SetDamage(weaponData.damage);
        // bullet.GetComponent<Projectile>().SetFacingAngle(angleDeg);
        // bullet.GetComponent<Projectile>().SetTravelDirection(direction);

        if (weaponType == WeaponCatalogue.Boomerang)
        {
            bullet.GetComponent<Projectile>().SetTravelDirection(new Vector3(1, 0, 0));
        }

        if (player.core.Movement.facingDir == -1)
        {
            bullet.transform.Rotate(0f, 180f, 0f);

            if (weaponType == WeaponCatalogue.Boomerang)
            {
                bullet.GetComponent<Projectile>().SetTravelDirection(new Vector3(-1, 0, 0));
            }
        }

        

        bullet.SetActive(true);

        bulletIndex++;
    }
}
