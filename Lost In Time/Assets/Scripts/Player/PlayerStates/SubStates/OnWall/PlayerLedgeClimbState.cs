using UnityEngine;

public class PlayerLedgeClimbState : PlayerAbility
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPos;
    private Vector2 stopPos;
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.transform.position = stopPos;
        isAbilityDone = true;
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = core.CollisionSenses.DetermineCornerPosition();

        startPos.Set(cornerPosition.x - (core.Movement.facingDir * charData.startOffset.x), cornerPosition.y - charData.startOffset.y);
        stopPos.Set(cornerPosition.x + (core.Movement.facingDir * charData.stopOffset.x), cornerPosition.y + charData.stopOffset.y);

        player.transform.position = startPos;

        // Remove later when animations are added in
        player.transform.position = stopPos;
        isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        core.Movement.SetVelocityZero();
        player.transform.position = startPos;
    }

    public void SetDetectedPosition(Vector2 position)
    {
        detectedPosition = position;
    } 
}
