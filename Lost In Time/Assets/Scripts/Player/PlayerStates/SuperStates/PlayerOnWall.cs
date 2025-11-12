using UnityEngine;
using UnityEngine.Windows;

public class PlayerOnWall : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;

    protected int InputX;
    protected bool FireInput;
    protected bool JumpInput;
    protected bool DashInput;

    protected bool isTouchingWallBack;
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
        isTouchingWallBack = core.CollisionSenses.WallBack();
    }

    public override void Enter()
    {
        base.Enter();

        player.weapon.SetShootDirection(-1);
    }

    public override void Exit()
    {
        base.Exit();

        player.weapon.SetShootDirection(1);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        InputX = player.playerInput.NormInputX;
        FireInput = player.playerInput.fireInput;
        JumpInput = player.playerInput.jumpInput;
        DashInput = player.playerInput.dashInput;

        if (JumpInput)
        {
            player.playerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.playerWallJumpState);
        }
        else if (DashInput && player.playerDashState.CanDash())
        {
            player.playerDashState.SetDashingFromWall();
            stateMachine.ChangeState(player.playerDashState);
        }
        else if (isGrounded)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
        else if ((!isTouchingWall && !isTouchingWallBack) || (isTouchingWallBack && InputX == core.Movement.facingDir) || (isTouchingWall && InputX != core.Movement.facingDir) || InputX == 0)
        {
            stateMachine.ChangeState(player.playerInAirState);
        }
    }
}
