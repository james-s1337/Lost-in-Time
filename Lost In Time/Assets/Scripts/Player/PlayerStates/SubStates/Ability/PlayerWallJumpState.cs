using UnityEngine;

public class PlayerWallJumpState : PlayerAbility
{
    private int wallJumpDirection;
    protected bool FireInput;
    protected bool MeleeInput;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.playerJumpingState.ResetJumps();
        player.core.Movement.SetVelocity(player.characterStats.baseJumpPower + player.characterStats.baseSpeed / 2, 
            charData.wallJumpAngle, wallJumpDirection);
        player.core.Movement.CheckIfShouldFlip(wallJumpDirection);
        player.playerJumpingState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        FireInput = player.playerInput.fireInput;
        MeleeInput = player.playerInput.meleeInput;

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

        player.anim.SetFloat("yVelocity", core.Movement.velocity.y);

        if (Time.time >= startTime + charData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.core.Movement.facingDir;
        }
        else
        {
            wallJumpDirection = player.core.Movement.facingDir;
        }
    }
}
