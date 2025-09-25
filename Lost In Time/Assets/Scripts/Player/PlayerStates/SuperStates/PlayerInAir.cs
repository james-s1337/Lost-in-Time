using UnityEngine;
using UnityEngine.Windows;

public class PlayerInAir : PlayerState
{
    private int InputX;
    private bool JumpInput;
    private bool JumpInputStop;
    private bool FireInput;

    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;
    public PlayerInAir(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("In Air");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        isGrounded = core.CollisionSenses.Ground;
        InputX = player.playerInput.NormInputX;
        JumpInput = player.playerInput.jumpInput;
        JumpInputStop = player.playerInput.jumpInputStop;
        FireInput = player.playerInput.fireInput;

        CheckJumpMultiplier();
        CheckGravity();

        if (FireInput)
        {
            stateMachine.ChangeState(player.playerFireState);
        }
        else if (isGrounded && core.Movement.velocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.playerLandedState);
        }
        else if (JumpInput && player.playerJumpingState.CanJump())
        {
            coyoteTime = false;
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else
        {
            core.Movement.CheckIfShouldFlip(InputX);
            core.Movement.SetVelocityX(charData.movementSpeed * InputX);

            player.anim.SetFloat("yVelocity", core.Movement.velocity.y);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (JumpInputStop)
            {
                core.Movement.SetVelocityY(core.Movement.velocity.y * charData.jumpMult);
                isJumping = false;
            }
            else if (core.Movement.velocity.y <= 0f)
            {
                isJumping = false;
            }

        }
    }

    // Fast fall, bouncy jump
    private void CheckGravity()
    {
        // Falling
        if (core.Movement.velocity.y < 0f)
        {
            core.Movement.SetPlayerGravity(charData.defaultGravity * charData.gravityFallMult);
        }
        else
        {
            core.Movement.SetPlayerGravity(charData.defaultGravity);
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + charData.coyoteTime)
        {
            coyoteTime = false;
            player.playerJumpingState.DecreaseAmountOfJumpsLeft();
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
}
