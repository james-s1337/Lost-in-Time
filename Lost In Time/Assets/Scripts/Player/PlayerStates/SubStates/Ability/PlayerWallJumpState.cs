using UnityEngine;

public class PlayerWallJumpState : PlayerAbility
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
}
