using UnityEngine;
using UnityEngine.Windows;

public class PlayerOnWall : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected int InputX;
    public PlayerOnWall(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
        isTouchingWall = core.CollisionSenses.Wall;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        InputX = player.playerInput.NormInputX;

        if (isGrounded)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        else if (!isTouchingWall || InputX != player.core.Movement.facingDir)
        {
            stateMachine.ChangeState(player.playerInAirState);
        }
    }
}
