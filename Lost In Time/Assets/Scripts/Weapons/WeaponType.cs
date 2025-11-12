using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    protected WeaponCatalogue weaponType;
    public WeaponStats weaponStats { get; private set; }
    protected bool canFire;
    protected bool isFiring;
    protected float timeSinceLastFire;
    protected Player player;
    protected CharacterData charData;

    protected WeaponPerk perk;

    protected int shootDirection = 1;
    protected virtual void Awake()
    {
        weaponStats = GetComponent<WeaponStats>();

        canFire = true;

        perk.ChangePerk(new WeaponStatModifier());
        perk.Test(WeaponModifier.Piercing, 3);
        ApplyWeaponModifier(perk.GetModifier());
    }
    public virtual void Fire() { }  

    public void SetPlayer(Player player, CharacterData charData)
    {
        this.player = player;
        this.charData = charData;
    }

    public WeaponCatalogue GetWeaponType()
    {
        return weaponType;
    }

    public virtual void ApplyWeaponModifier(IWeaponModifier mod)
    {
        weaponStats.AddModifier(mod);
    }

    public virtual void RemoveWeaponModifier(IWeaponModifier mod)
    {
        weaponStats.RemoveModifier(mod);
    }

    public void RollNewPerks(WeaponModifier newMod, float amount)
    {
        RemoveWeaponModifier(perk.GetModifier());
        perk.Test(newMod, amount);
        ApplyWeaponModifier(perk.GetModifier());
    }

    public void SetShootDirection(int direction)
    {
        shootDirection = direction;
    }
}

public enum WeaponCatalogue
{
    Pistol,
    EnergyPistol,
    Burst,
    Mine,
    Boomerang,
    Revolver,
    Flamethrower,
    Flintlock,
    Shotgun,
    Crossbow,
    Longsword,
    Katana,
    Spear,
    Dagger,
    Khopesh,
    Cutlass,
    Sniper,
    Cannon,
    IceGun,
}  

public struct WeaponPerk
{
    private WeaponStatModifier modifier;

    public void ChangePerk(WeaponStatModifier modifier)
    {
        this.modifier = modifier;
    }

    public IWeaponModifier GetModifier()
    {
        return modifier;
    }

    //  replace with GenerateNewPerk()
    public void Test(WeaponModifier newMod, float amount)
    {
        modifier.mods.Clear();
        WeaponMod mod = new WeaponMod();

        mod.mod = newMod;
        mod.amount = amount;

        modifier.mods.Add(mod);
    }
}
