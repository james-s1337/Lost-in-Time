using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weapon { get; private set; }
    private Animator anim;
    private Player player;

    [SerializeField] private List<WeaponType> weapons;
    private int currentWeaponIndex;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    #region Melee functions
    public void EnterWeap()
    {
        // Make sprite visible
        gameObject.SetActive(true);
        anim.SetBool("attacking", true);
    }

    public void ExitWeap()
    {
        // Make sprite invisible
        gameObject.SetActive(false);
        anim.SetBool("attacking", false);
    }
    public void OnAnimationTrigger()
    {
        player.AnimationTrigger();
    }

    public void OnAnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }
    #endregion
    public void UseWeapon()
    {
        weapon.Fire();
    }

    public void ChangeWeapon(int weaponIndex)
    {
        weapon = weapons[weaponIndex];
        weapon.SetPlayer(player);
        currentWeaponIndex = weaponIndex;
    }

    public int GetRandomWeaponIndex()
    {
        int newWeaponIndex = Random.Range(0, weapons.Count);
        if (newWeaponIndex == currentWeaponIndex && currentWeaponIndex < weapons.Count-1)
        {
            newWeaponIndex++;
        }
        else if (newWeaponIndex == currentWeaponIndex && currentWeaponIndex > 0)
        {
            newWeaponIndex--;
        }

        return newWeaponIndex;
    }
}

/* Weapon Index
 * 0 = Pistol
 * 1 = Burst
 * 2 = Mine
 * 3 = Boomerang
 * 4 = Revolver
 * 5 = Flamethrower
*/
