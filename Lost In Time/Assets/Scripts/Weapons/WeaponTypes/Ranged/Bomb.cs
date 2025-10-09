using UnityEngine;

public class Bomb : Ranged
{
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Mine;
    }
}
