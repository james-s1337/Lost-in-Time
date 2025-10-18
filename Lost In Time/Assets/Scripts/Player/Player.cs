using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    [SerializeField] CharacterData charData;
    public CharacterStats characterStats { get; private set; }

    private PlayerStateMachine stateMachine;
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerRunningState playerRunningState { get; private set; }
    public PlayerLandedState playerLandedState { get; private set; }
    public PlayerJumpingState playerJumpingState { get; private set; }
    public PlayerFireState playerFireState { get; private set; }
    public PlayerInAir playerInAirState { get; private set; }

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
        playerFireState = new PlayerFireState(this, stateMachine, charData, "fire");
        playerInAirState = new PlayerInAir(this, stateMachine, charData, "inAir");

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

        if (characterStats.currentHP <= 0)
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
}
