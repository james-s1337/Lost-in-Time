using UnityEngine;

public class PlayerWallSlideState : PlayerOnWall
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.core.Movement.SetVelocityY(-player.characterStats.charData.wallSlideVelocity);
    }
}
