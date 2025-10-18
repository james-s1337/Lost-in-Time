using System.Collections;
using UnityEngine;

public class Pistol : Ranged
{
    protected override void Awake()
    {
        base.Awake();   
        weaponType = WeaponCatalogue.Pistol;
    }
}
