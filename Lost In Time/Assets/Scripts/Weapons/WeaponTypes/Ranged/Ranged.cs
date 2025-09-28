using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ranged : WeaponType
{
    [SerializeField] protected RangedWeaponData weaponData;
    protected bool canFire;
    protected bool isFiring;
    protected float cooldown;
    protected float timeSinceLastFire;

    [SerializeField] protected List<GameObject> bulletPrefabPool;
    protected int bulletIndex;
    private void Awake()
    {
        canFire = true;
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

        if (weaponData.numOfShots > 1)
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
        for (int i = 0; i < weaponData.numOfShots; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(cooldown);
        }
        SetTimeSinceLastFired();
    }

    private void SetTimeSinceLastFired()
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
        Debug.Log("Yipp1");
        if (bulletIndex >= numOfPrefabs)
        {
            bulletIndex = 0;
        }

        bulletPrefabPool[bulletIndex].transform.position = gameObject.transform.position;
        if (player.core.Movement.facingDir == -1)
        {
            bulletPrefabPool[bulletIndex].transform.Rotate(0f, 180f, 0f);
        }
        bulletPrefabPool[bulletIndex].SetActive(true);
        Debug.Log("Yipp2");
        bulletIndex++;
    }
}
