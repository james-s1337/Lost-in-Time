using System.Collections;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 velocity { get; private set; }
    public int facingDir { get; private set; }
    private Vector2 workspace;

    private bool isGettingKnocked = false;
    private float knockbackTime = 0.2f;

    // Player only
    private CameraFollowObject cameraFollowObject;
    private float fallYSpeedDampChangeThreshold;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponentInParent<Rigidbody2D>();

        facingDir = 1;
    }

    private void Start()
    {
        if (GetComponentInParent<Player>())
        {
            cameraFollowObject = GameObject.FindGameObjectWithTag("Camera Follow Object").GetComponent<CameraFollowObject>();
            fallYSpeedDampChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
        }
    }

    public void SetPlayerGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }

    public void LogicUpdate()
    {
        velocity = rb.linearVelocity;
        CheckCameraDampingChange();
    }

    #region Set Functions

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    // For dashing/recoil
    public void SetVelocity(float v, Vector2 angle, int direction)
    {
        // v = speed
        angle.Normalize();
        workspace.Set(angle.x * v * direction, angle.y * v);
        SetFinalVelocity();
    }
    public void SetVelocity(float v, Vector2 angle)
    {
        // v = speed
        angle.Normalize();
        workspace.Set(angle.x * v, angle.y * v);
        SetFinalVelocity();
    }

    public void SetVelocityX(float v)
    {
        workspace.Set(v, velocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float v)
    {
        workspace.Set(velocity.x, v);
        SetFinalVelocity();
    }

    // Recoil + Knockback
    public void AddForce(Vector2 direction, float force)
    {
        SetVelocityZero();
        isGettingKnocked = true;

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        StartCoroutine(SetGettingKnockedFalse());
    }

    private IEnumerator SetGettingKnockedFalse()
    {
        yield return new WaitForSeconds(knockbackTime);
        isGettingKnocked = false;
        SetVelocityZero();
    }

    private void SetFinalVelocity()
    {
        if (isGettingKnocked)
        {
            return;
        }

        rb.linearVelocity = workspace;
        velocity = workspace;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDir)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingDir *= -1;
        rb.transform.Rotate(0.0f, 180.0f, 0.0f);

        if (cameraFollowObject)
        {
            cameraFollowObject.CallTurn();
        } 
    }

    void CheckCameraDampingChange()
    {
        if (!cameraFollowObject)
        {
            return;
        }
        // Falling past a threshold
        if (rb.linearVelocityY < fallYSpeedDampChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        // Standing still/moving up
        if (rb.linearVelocityY >= 0f && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.lerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }
    #endregion
}
