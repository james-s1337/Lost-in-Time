using UnityEngine;

public class Continuous : Ranged
{
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Flamethrower;
    }
}
