using UnityEngine;

public class PlayerMeleeAttackState : PlayerAbility
{
    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }
}
