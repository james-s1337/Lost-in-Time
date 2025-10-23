using UnityEngine;

public class PlayerDashState : PlayerAbility
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
}
