using UnityEngine;

public class WeaponType : MonoBehaviour
{
    protected WeaponCatalogue weaponType;
    protected Player player;
    public virtual void Fire() { }  

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public WeaponCatalogue GetWeaponType()
    {
        return weaponType;
    }
}

public enum WeaponCatalogue
{
    Pistol,
    Burst,
    Mine,
    Boomerang
}
