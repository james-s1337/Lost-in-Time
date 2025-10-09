using System.Collections;
using UnityEngine;

public class Rang : Ranged
{
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Boomerang;
    }
    protected override void Update()
    {
        
    }

    public void SetCanFireTrue()
    {
        canFire = true;
    }
}
