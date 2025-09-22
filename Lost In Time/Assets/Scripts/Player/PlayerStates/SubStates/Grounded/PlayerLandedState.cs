using UnityEngine;
using UnityEngine.Windows;

public class PlayerLandedState : PlayerGrounded
{
    public PlayerLandedState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Debug.Log("Landed on ground");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (InputX != 0)
            {
                stateMachine.ChangeState(player.playerRunningState);
            }
            else // if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }
}
