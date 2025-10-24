using UnityEngine;

public class PlayerWallSlideState : PlayerOnWall
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        if (FireInput) {
            player.weapon.UseWeapon();
        }

        player.core.Movement.SetVelocityY(-charData.wallSlideVelocity);
    }
}
