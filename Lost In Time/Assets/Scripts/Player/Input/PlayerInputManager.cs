using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private Vector2 movementInput;
    public int NormInputX { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    public bool fireInput { get; private set; }

    void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(movementInput.x); // We only care about the X axis (left-right movement)
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            jumpInputStop = true;
        }
    }
    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    public void UseJumpInput() { jumpInput = false; }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fireInput = true;
        }

        if (context.canceled)
        {
            fireInput = false;
        }
    }
}
