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

        Debug.Log("Moving");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.CheckIfShouldFlip(InputX);
        core.Movement.SetVelocityX(charData.movementSpeed * InputX);

        if (!isExitingState)
        {
            if (InputX == 0)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }
}
