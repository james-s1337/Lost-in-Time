using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    [SerializeField] CharacterData charData;

    private PlayerStateMachine stateMachine;
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerRunningState playerRunningState { get; private set; }
    public PlayerLandedState playerLandedState { get; private set; }
    public PlayerJumpingState playerJumpingState { get; private set; }
    public PlayerFireState playerFireState { get; private set; }
    public PlayerInAir playerInAirState { get; private set; }

    [SerializeField] int weaponIndex = 0;
    public Weapon weapon { get; private set; }

    private int currentHealth;
    private float timeSinceDamageTaken;
    private float takeDamageCooldown = 0.1f;
    private bool dead;
    void Awake()
    {
        core = GetComponentInChildren<Core>();
        weapon = GetComponentInChildren<Weapon>();
        anim = GetComponentInChildren<Animator>();

        stateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, stateMachine, charData, "idle");
        playerRunningState = new PlayerRunningState(this, stateMachine, charData, "run");
        playerJumpingState = new PlayerJumpingState(this, stateMachine, charData, "inAir");
        playerLandedState = new PlayerLandedState(this, stateMachine, charData, "landed");
        playerFireState = new PlayerFireState(this, stateMachine, charData, "fire");
        playerInAirState = new PlayerInAir(this, stateMachine, charData, "inAir");

        stateMachine.ChangeState(playerIdleState);
        ChangeWeapon(weaponIndex);
    }

    void Start()
    {
        currentHealth = charData.health;
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

    public void ChangeWeapon(int weaponIndex)
    {
        weapon.ChangeWeapon(weaponIndex);
    }

    public void ChangeToRandomWeapon()
    {
        weapon.ChangeWeapon(weapon.GetRandomWeaponIndex());
    }

    // Call these from the animation to call the AnimationTrigger in the states
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void TakeDamage(int damage)
    {
        if (Time.time < timeSinceDamageTaken + takeDamageCooldown)
        {
            return;
        }

        timeSinceDamageTaken = Time.time;
        currentHealth--;

        if (currentHealth <= 0)
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
