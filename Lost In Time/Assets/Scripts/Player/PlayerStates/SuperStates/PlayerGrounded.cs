 using UnityEngine;

public class PlayerGrounded : PlayerState
{
    protected int InputX;
    protected bool JumpInput;
    protected bool FireInput;

    private bool isGrounded;
    public PlayerGrounded(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // Check if grounded
        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        player.playerJumpingState.ResetJumps();
        core.Movement.SetPlayerGravity(charData.defaultGravity);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        InputX = player.playerInput.NormInputX;
        JumpInput = player.playerInput.jumpInput;  
        FireInput = player.playerInput.fireInput;
        isGrounded = core.CollisionSenses.Ground;

        if (FireInput)
        {
            player.weapon.UseWeapon();
            if (mouseWorldPos.x - player.transform.position.x >= 0)
            {
                shootingDirection = 1;
            }
            else
            {
                shootingDirection = -1;
            }

            core.Movement.CheckIfShouldFlip(shootingDirection);
            // stateMachine.ChangeState(player.playerFireState);
        }

        if (JumpInput && player.playerJumpingState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else if (!isGrounded)
        {
            player.playerInAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.playerInAirState);
        }
    }
}
