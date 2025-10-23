using UnityEngine;

public class PlayerLedgeClimbState : PlayerOnWall
{
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
}
