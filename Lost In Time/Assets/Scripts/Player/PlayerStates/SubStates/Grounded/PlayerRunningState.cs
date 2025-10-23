using UnityEngine;
using UnityEngine.Windows;

public class PlayerRunningState : PlayerGrounded
{
    public PlayerRunningState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!FireInput && Time.time >= shootDirectionSwitchStart + directionSwitchCooldown)
        {
            core.Movement.CheckIfShouldFlip(InputX);
        }
        core.Movement.SetVelocityX(player.characterStats.baseSpeed * InputX);

        if (!isExitingState)
        {
            if (InputX == 0)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }
}
