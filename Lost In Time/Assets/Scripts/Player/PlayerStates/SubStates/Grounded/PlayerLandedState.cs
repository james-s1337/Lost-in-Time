using UnityEngine;
using UnityEngine.Windows;

public class PlayerLandedState : PlayerGrounded
{
    private bool canTransition;
    public PlayerLandedState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        canTransition = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        canTransition = true;
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
            if (!isAnimationFinished)
            {
                stateMachine.ChangeState(player.playerIdleState);
            }
        }
    }
}
