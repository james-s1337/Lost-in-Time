 using UnityEngine;

public class PlayerGrounded : PlayerState
{
    protected int InputX;
    protected bool JumpInput;
    protected bool FireInput;
    private bool DashInput;

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
        DashInput = player.playerInput.dashInput;
        isGrounded = core.CollisionSenses.Ground;

        if (FireInput)
        {
            player.weapon.UseWeapon();
            // stateMachine.ChangeState(player.playerFireState);
        }

        if (JumpInput && player.playerJumpingState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpingState);
        }
        else if (DashInput && player.playerDashState.CanDash())
        {
            stateMachine.ChangeState(player.playerDashState);
        }
        else if (!isGrounded)
        {
            player.playerInAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.playerInAirState);
        }
    }
}
