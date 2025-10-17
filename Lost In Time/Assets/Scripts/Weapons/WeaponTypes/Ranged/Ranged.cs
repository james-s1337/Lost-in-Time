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
    protected override void Awake()
    {
        base.Awake();

        canFire = true;
        numOfShots = weaponData.numOfShots;
    }

    protected virtual void Start()
    {
        if (weaponData)
        {
            cooldown = weaponData.cooldown;
            damage = weaponData.damage;
            damage = weaponData.damage;
            bounce = weaponData.bounces;
            pierce = weaponData.piercing;
            travelTime = weaponData.travelTime;
            travelDistance = weaponData.travelDistance;
            knockback = weaponData.knockback;
            recoil = weaponData.recoil;
            critChance = weaponData.critChance;
            critDamage = weaponData.critDamage;

            AddPerk(0, WeaponModifier.Cooldown, 100);
            AddPerk(1, WeaponModifier.ReloadSpeed, 100);
            AddPerk(2, WeaponModifier.SpeedDMG, 100);
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

        Recoil();
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

    protected void Recoil()
    {
        player.core.Movement.AddForce(-(player.gameObject.transform.position + new Vector3(player.core.Movement.facingDir, 0, 0)), recoil);
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

        bullet.GetComponent<Projectile>().SetDamage(damage + speedDamage);
        bullet.GetComponent <Projectile>().SetTravelTime(travelTime);
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

    public override void AddPerk(int perkIndex, WeaponModifier weapMod, int value)
    {
        base.AddPerk(perkIndex, weapMod, value);
        StartCoroutine(ApplyModifier(weapMod));
    }

    public override WeaponModifier RemovePerk(int perkIndex)
    {
        WeaponModifier mod = base.RemovePerk(perkIndex);
        StartCoroutine(ApplyModifier(mod));

        return mod;
    }

    private IEnumerator ApplyModifier(WeaponModifier mod)
    {
        int value = weaponModifiers[mod];

        switch (mod)
        {
            case WeaponModifier.Cooldown:
                cooldown = weaponData.cooldown / ((value + 100) / 100);
                // Expand object pool if cooldown is low enough
                if (weaponType == WeaponCatalogue.Boomerang)
                {
                    break;
                }

                int fireRate = Mathf.RoundToInt(travelTime / cooldown);
                int bulletCountDiff = fireRate - bulletPrefabPool.Count;

                if (numOfShots > 1)
                {
                    bulletCountDiff = (fireRate * numOfShots) - bulletPrefabPool.Count;
                }

                if (bulletCountDiff >= 0)
                {
                    for (int i = 0; i < bulletCountDiff + 1; i++)
                    {
                        GameObject newBulletPrefab = Instantiate(bulletPrefabPool[0]);
                        newBulletPrefab.GetComponent<Projectile>().Initialize();
                        newBulletPrefab.transform.parent = bulletPrefabPool[0].transform.parent;

                        bulletPrefabPool.Add(newBulletPrefab);
                    }
                }
                break;
            case WeaponModifier.Damage:
                damage = weaponData.damage + weaponData.damage * value / 100;
                break;
            case WeaponModifier.Bounce: // Clone dummy projectile for bounce
                bounce = weaponData.bounces + value;
                break;
            case WeaponModifier.Piercing:
                pierce = weaponData.piercing + value;
                break;
            case WeaponModifier.TravelDistance:
                travelDistance = weaponData.travelDistance + value;
                break;
            case WeaponModifier.Knockback:
                knockback = weaponData.knockback + value;
                break;
            case WeaponModifier.Recoil:
                recoil = weaponData.recoil + value;
                break;
            case WeaponModifier.CritChance:
                critChance = weaponData.critChance + value;
                break;
            case WeaponModifier.CritDamage:
                critDamage = weaponData.critDamage + value;
                break;
            case WeaponModifier.ReloadSpeed:
                reloadTime = weaponData.burstCooldown / ((value + 100) / 100);
                break;
            case WeaponModifier.Ammo:
                ammo = weaponData.numOfShots + value;
                break;
            case WeaponModifier.SpeedDMG:
                speedDamage = (int)(weaponData.damage * (charData.movementSpeed) * value / 100);
                break;
        }

        yield return null;
    }
}
