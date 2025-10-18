using System.Collections;
using UnityEngine;

public class HighNoon : Reloadable
{
    protected override void Awake()
    { 
        base.Awake();
        weaponType = WeaponCatalogue.Revolver;
    }

    protected override void SpawnBullet()
    {
        base.SpawnBullet();
        bulletsLeft -= 1;
    }
}
