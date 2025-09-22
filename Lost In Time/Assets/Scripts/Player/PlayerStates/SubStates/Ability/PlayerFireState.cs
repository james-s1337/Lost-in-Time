using UnityEngine;

public class PlayerFireState : PlayerAbility
{
    public PlayerFireState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
}
