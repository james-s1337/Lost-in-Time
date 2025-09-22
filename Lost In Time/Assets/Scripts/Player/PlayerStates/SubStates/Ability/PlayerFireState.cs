 using UnityEngine;

public class PlayerFireState : PlayerAbility
{
    // Have a variable that contains the weapon data, will update when the player
    // Another variable containing the upgrades for that specific weapon
    private bool canFire = true;
    private float timeSinceLastFire;
    public PlayerFireState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (!canFire)
        {
            isAbilityDone = true;
            return;
        }
        // Check weapon type
        // If weapon is melee, then play animation, and when animation finishes, then finish ability
        // Else spawn in projectile and end ability immediately
        FireProjectile();
        isAbilityDone = true;
        Debug.Log("Firing weapon");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckFireCooldown();
    }

    private void CheckFireCooldown()
    {
        if (!canFire)
        {
            
        }
    }

    private void FireProjectile()
    {
        if (canFire)
        {
            canFire = false;
            timeSinceLastFire = Time.time;
            // Spawn in projectile based on what is in the data for it
        }
    }

    public void ResetWeaponCooldown()
    {
        canFire = true;
    }

    public void ChangeWeapon()
    {
        // Update weapon data variable
        ResetWeaponCooldown();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }
}
