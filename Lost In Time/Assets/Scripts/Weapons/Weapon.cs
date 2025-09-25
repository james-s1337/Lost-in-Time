using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weapon { get; private set; }
    private Animator anim;
    private Player player;

    [SerializeField] private List<WeaponType> weapons; 

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        weapon.SetPlayer(player);
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
    }
}

/* Weapon Index
 * 0 = Pistol
 * 
 * 
 * 
 * 
*/
