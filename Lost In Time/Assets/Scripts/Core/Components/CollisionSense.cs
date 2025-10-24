using UnityEngine;
using UnityEngine.UIElements;

public class CollisionSense : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public LayerMask WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }

    public Transform WallCheck {  get => wallCheck; private set => wallCheck = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
    public LayerMask WhatIsWall { get => whatIsWall; private set => whatIsWall = value; }
    public Transform LedgeCheck { get => ledgeCheck; private set => ledgeCheck = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private Transform ledgeCheck;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
    }


    public bool Wall
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }

    public bool WallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }

    void OnDrawGizmos()
    {
        if (core == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
        Debug.DrawRay(wallCheck.position, Vector2.right * core.Movement.facingDir * wallCheckDistance, Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * -core.Movement.facingDir * wallCheckDistance, Color.red);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * core.Movement.facingDir, wallCheckDistance, whatIsWall);
    }
}
