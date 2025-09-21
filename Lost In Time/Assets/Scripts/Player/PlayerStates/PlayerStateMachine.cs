using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;

    public void ChangeState(PlayerState newState) 
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
}
