using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJumpingState : PlayerAbility
{
    private int jumpsLeft;
    public PlayerJumpingState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
        jumpsLeft = player.characterStats.baseJumps;
    }
    public override void Enter()
    {
        base.Enter();

        player.playerInput.UseJumpInput();
        core.Movement.SetVelocityY(player.characterStats.baseJumpPower);
        isAbilityDone = true;
        jumpsLeft--;
        player.playerInAirState.SetIsJumping();
    }

    public void ResetJumps() => jumpsLeft = player.characterStats.baseJumps;
    public void DecreaseAmountOfJumpsLeft() => jumpsLeft--;
    public bool CanJump()
    {
        return jumpsLeft > 0;
    }
}
