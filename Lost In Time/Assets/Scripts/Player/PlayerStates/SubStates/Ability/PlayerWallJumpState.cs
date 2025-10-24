using UnityEngine;

public class PlayerWallJumpState : PlayerAbility
{
    private int wallJumpDirection;
    protected bool FireInput;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.playerJumpingState.ResetJumps();
        player.core.Movement.SetVelocity(player.characterStats.baseSpeed, charData.wallJumpAngle, wallJumpDirection);
        player.core.Movement.CheckIfShouldFlip(wallJumpDirection);
        player.playerJumpingState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        FireInput = player.playerInput.fireInput;

        if (FireInput)
        {
            player.weapon.UseWeapon();
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
