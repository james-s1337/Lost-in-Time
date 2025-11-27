 using UnityEngine;

public class PlayerGrounded : PlayerState
{
    protected int InputX;
    protected bool JumpInput;
    protected bool FireInput;
    protected bool MeleeInput;
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
        MeleeInput = player.playerInput.meleeInput;
        DashInput = player.playerInput.dashInput;
        isGrounded = core.CollisionSenses.Ground;

        if (MeleeInput)
        {
            player.playerAttackState.SetAttackType(1, 0);
            stateMachine.ChangeState(player.playerAttackState);
        }
        else if (FireInput && player.weapon.weapon.canFire)
        {
            player.playerAttackState.SetAttackType(0, 0);
            stateMachine.ChangeState(player.playerAttackState);
        }
        else if (JumpInput && player.playerJumpingState.CanJump())
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
