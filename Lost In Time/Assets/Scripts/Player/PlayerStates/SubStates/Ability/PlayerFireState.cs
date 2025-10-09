 using UnityEngine;
using UnityEngine.Windows;

public class PlayerFireState : PlayerAbility
{
    private bool FireInput;

    public PlayerFireState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        // Check weapon type
        // If weapon is melee, then play animation, and when animation finishes, then finish ability
        // Else spawn in projectile and end ability immediately
        // player.playerInput.UseFireInput();
        player.weapon.UseWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        FireInput = player.playerInput.fireInput;

        if (!FireInput) 
        {
            isAbilityDone = true;
        }
        else
        {
            player.weapon.UseWeapon();
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }
}
