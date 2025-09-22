using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJumpingState : PlayerAbility
{
    private int jumpsLeft;
    public PlayerJumpingState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
        jumpsLeft = charData.jumps;
    }
    public override void Enter()
    {
        base.Enter();

        player.playerInput.UseJumpInput();
        core.Movement.SetVelocityY(charData.jumpPower);
        isAbilityDone = true;
        jumpsLeft--;
        player.playerInAirState.SetIsJumping();
        Debug.Log("Jumping");
    }

    public void ResetJumps() => jumpsLeft = charData.jumps;
    public void DecreaseAmountOfJumpsLeft() => jumpsLeft--;
    public bool CanJump()
    {
        return jumpsLeft > 0;
    }
}
