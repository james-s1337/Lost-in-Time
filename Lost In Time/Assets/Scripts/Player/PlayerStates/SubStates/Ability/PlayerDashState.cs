using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerAbility
{
    private int dashDirection;
    private float timeSinceLastDash;
    private float dashVelocity;

    private bool dashingFromWall;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    private bool FireInput;
    private bool JumpInput;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mouseWorldPos.x - player.transform.position.x >= 0)
        {
            dashDirection = 1;
        }
        else
        {
            dashDirection = -1;
        }

        if (dashingFromWall)
        {
            dashDirection = core.Movement.facingDir;
        }

        timeSinceLastDash = Time.time;
        dashVelocity = player.characterStats.baseDashForce + player.characterStats.baseSpeed;

        core.Movement.SetDashVelocity(dashVelocity, Vector2.right * dashDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        FireInput = player.playerInput.fireInput;
        JumpInput = player.playerInput.jumpInput;

        if (Time.time >= timeSinceLastDash + charData.dashTime)
        {
            isAbilityDone = true;
        }
        else if (FireInput)
        {
            isAbilityDone = true;
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.playerLedgeClimbState);
        }
        else if (JumpInput && isTouchingWall)
        {
            player.playerInAirState.StopWallJumpCoyoteTime();
            isTouchingWall = core.CollisionSenses.Wall;
            player.playerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.playerWallJumpState);
        }
        else if (JumpInput && player.playerJumpingState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else if (isTouchingWall)
        {
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        else
        {
            core.Movement.SetDashVelocity(dashVelocity, Vector2.right * dashDirection);
        }   
    }

    public bool CanDash()
    {
        return Time.time >= timeSinceLastDash + charData.dashCooldown + charData.dashTime;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingWall = core.CollisionSenses.Wall;
        isTouchingLedge = core.CollisionSenses.CheckIfTouchingLedge();

        if (isTouchingWall && !isTouchingLedge)
        {
            player.playerLedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public void SetDashingFromWall()
    {
        dashingFromWall = true;
    }

    public override void Exit()
    {
        base.Exit();

        dashingFromWall = false;
    }
}
