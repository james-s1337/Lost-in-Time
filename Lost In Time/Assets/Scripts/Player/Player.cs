using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region Essentials
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    [SerializeField] CharacterData charData;
    public CharacterStats characterStats { get; private set; }
    #endregion

    #region States
    private PlayerStateMachine stateMachine;
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerRunningState playerRunningState { get; private set; }
    public PlayerLandedState playerLandedState { get; private set; }
    public PlayerJumpingState playerJumpingState { get; private set; }
    public PlayerMeleeAttackState playerMeleeState{ get; private set; }
    public PlayerInAir playerInAirState { get; private set; }
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    public PlayerLedgeClimbState playerLedgeClimbState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    #endregion

    [SerializeField] int weaponIndex = 0;
    public Weapon weapon { get; private set; }

    private float timeSinceDamageTaken;
    private float takeDamageCooldown = 0.1f;
    private bool dead;

    public List<ScriptableObject> modifiers = new List<ScriptableObject>();
    void Awake()
    {
        core = GetComponentInChildren<Core>();
        weapon = GetComponentInChildren<Weapon>();
        anim = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, stateMachine, charData, "idle");
        playerRunningState = new PlayerRunningState(this, stateMachine, charData, "run");
        playerJumpingState = new PlayerJumpingState(this, stateMachine, charData, "inAir");
        playerLandedState = new PlayerLandedState(this, stateMachine, charData, "landed");
        playerMeleeState = new PlayerMeleeAttackState(this, stateMachine, charData, "melee");
        playerInAirState = new PlayerInAir(this, stateMachine, charData, "inAir");
        playerWallSlideState = new PlayerWallSlideState(this, stateMachine, charData, "wallSlide");
        playerLedgeClimbState = new PlayerLedgeClimbState(this, stateMachine, charData, "ledgeClimb");
        playerWallJumpState = new PlayerWallJumpState(this, stateMachine, charData, "inAir");

        stateMachine.ChangeState(playerIdleState);
        ChangeWeapon(weaponIndex);

        // TEMP
        UpdateModifiers();
        // TEMP

        /* How to make random weap mod
        WeaponStatModifier n = new WeaponStatModifier();
        WeaponMod mod = new WeaponMod();
        mod.mod = WeaponModifier.Piercing;
        mod.amount = 2;
        n.mods.Add(mod);

        AddModifier(n);
        */
    }

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        core.Movement.SetPlayerGravity(charData.defaultGravity);
        playerInput = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }

        core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
    }

    public void AddModifier(ScriptableObject modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiers();
    }

    public void RemoveModifier(ScriptableObject modifier)
    {
        modifiers.Remove(modifier);
        UpdateRemoveModifier(modifier);
    }

    public PlayerState GetCurrentState()
    {
        return stateMachine.currentState;
    }
    private void UpdateModifiers()
    {
        foreach (var mod in modifiers)
        {
            if (mod is IStatModifier statMod)
            {
                characterStats.AddModifier(statMod);
            }
            else if (mod is IWeaponModifier weapMod)
            {
                weapon.ApplyWeaponModifier(weapMod);
            }
        }
    }

    private void UpdateRemoveModifier(ScriptableObject mod)
    {
        if (mod is IStatModifier statMod)
        {
            characterStats.RemoveModifier(statMod);
        }
        else if (mod is IWeaponModifier weapMod)
        {
            weapon.RemoveWeaponModifier(weapMod);
        }
    }

    public void ChangeWeapon(int weaponIndex)
    {
        weapon.ChangeWeapon(weaponIndex, charData);
    }

    public void ChangeToRandomWeapon()
    {
        weapon.ChangeWeapon(weapon.GetRandomWeaponIndex(), charData);
    }

    // Call these from the animation to call the AnimationTrigger in the states
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void TakeDamage(float damage)
    {
        if (Time.time < timeSinceDamageTaken + takeDamageCooldown)
        {
            return;
        }

        timeSinceDamageTaken = Time.time;
        characterStats.SetTimeSinceLastDamage();
        characterStats.TakeDamage(damage);

        if (characterStats.GetCurrentHP() <= 0)
        {
            // Dead
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("PLayer ded");
        //dead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null)
            {
                return;
            }

            ApplyOnHitEffects(enemy);
        }
    }

    // When enemy hits the player
    private void ApplyOnHitEffects(Enemy enemy)
    {
        if (enemy && enemy.currentHealth > 0)
        {
            if (Random.value < characterStats.burningTouch)
            {
                IBurnable status = enemy.GetComponent<IBurnable>();
                if (status != null)
                {
                    status.ApplyBurn();
                }
            }

            if (Random.value < characterStats.infectiousTouch)
            {
                IPoisonable status = enemy.GetComponent<IPoisonable>();
                if (status != null)
                {
                    status.ApplyPoison();
                }
            }

            if (Random.value < characterStats.freezingTouch)
            {
                IFreezeable status = enemy.GetComponent<IFreezeable>();
                if (status != null)
                {
                    status.ApplyFreeze();
                }
            }

            if (Random.value < characterStats.stickyTouch)
            {
                ISlowable status = enemy.GetComponent<ISlowable>();
                if (status != null)
                {
                    status.ApplySlow();
                }
            }

            if (Random.value < characterStats.spikyTouch)
            {
                IBleedable status = enemy.GetComponent<IBleedable>();
                if (status != null)
                {
                    status.ApplyBleed();
                }
            }
        }
    }
}
