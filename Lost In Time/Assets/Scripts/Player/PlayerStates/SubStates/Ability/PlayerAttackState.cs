using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerAttackState : PlayerAbility
{
    private string notAttackingAnimName = "notAttacking";
    private string shootLightAnimName = "shootLight";
    private string shootHeavyAnimName = "shootHeavy";
    private string meleeAnimName = "melee";

    private string animToSet = "shootLight";

    private int meleeNum = 0;

    private bool canFlip = false;
    private bool isLunging = false;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, CharacterData charData, string animBoolName) : base(player, stateMachine, charData, animBoolName)
    {
        player.attackAnim.SetBool(notAttackingAnimName, true);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
        isLunging = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        canFlip = false;
        
        if (animToSet.Equals(shootLightAnimName) || animToSet.Equals(shootHeavyAnimName))
        {
            // spawn projectile + recoil
            player.weapon.UseWeapon();
        }
        else
        {
            // add a burst of speed if melee
            isLunging = true;
        }
    }

    public override void Enter()
    {
        base.Enter();

        // show and set attack sprite
        player.attackAnim.SetBool(animToSet, true);
        player.attackAnim.SetBool(notAttackingAnimName, false);

        core.Movement.SetVelocityZero();
        canFlip = true;
        isLunging = false;
    }

    public override void Exit()
    {
        base.Exit();

        // hide attack sprite
        player.attackAnim.SetBool(notAttackingAnimName, true);
        player.attackAnim.SetBool(animToSet, false);    
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        if (canFlip)
        {
            if (mouseWorldPos.x - player.transform.position.x >= 0)
            {
                shootingDirection = 1;
            }
            else
            {
                shootingDirection = -1;
            }

            core.Movement.CheckIfShouldFlip(shootingDirection);
            shootDirectionSwitchStart = Time.time;
        }

        if (!isLunging)
        {
            core.Movement.SetVelocityZero();
        }
        else
        {
            core.Movement.SetVelocityX(10 * shootingDirection);
        }
    }

    public void SetAttackType(int type, int weight) // 0 = gun, 1 = melee, weight only for guns (0 = light, 1 = heavy)
    {
        if (type == 1)
        {
            meleeNum += 1;

            if (meleeNum > 3)
            {
                meleeNum = 1;
            }

            animToSet = meleeAnimName + meleeNum.ToString();
            return;
        }
        
        if (weight == 0)
        {
            animToSet = shootLightAnimName;
            return;
        }

        animToSet = shootHeavyAnimName;
    }
}
