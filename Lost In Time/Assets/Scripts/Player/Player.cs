using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    [SerializeField] CharacterData charData;

    private PlayerStateMachine stateMachine;
    void Awake()
    {
        core = GetComponentInChildren<Core>();
        stateMachine = new PlayerStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = GetComponent<PlayerInputManager>();
        anim = GetComponentInChildren<Animator>();
    }
}
