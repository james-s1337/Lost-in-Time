 using UnityEngine;
using UnityEngine.Windows;

public class PlayerFireState : PlayerAbility
{
    public PlayerFireState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        // Check weapon type
        // If weapon is melee, then play animation, and when animation finishes, then finish ability
        // Else spawn in projectile and end ability immediately
        player.playerInput.UseFireInput();
        player.weapon.UseWeapon();
        isAbilityDone = true;
        Debug.Log("Firing weapon");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }
}
