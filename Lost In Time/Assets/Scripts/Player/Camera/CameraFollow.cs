using System.Collections;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 0.2f; // Time for object to rotate from flipping (lerp time) 

    private Player player;
    private bool isFacingRight;
    private bool freezePos;

    private Coroutine coroutine;

    private float offset;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<Player>();
    }

    private void Start()
    {
        if (player.core.Movement.facingDir == 1)
        {
            isFacingRight = true;
            offset = 1f;
        }
        else
        {
            isFacingRight = false;
            offset = -1f;
        }
    }

    private void LateUpdate()
    {
        if (freezePos)
        {
            transform.position = new Vector3(playerTransform.position.x + offset, transform.position.y, 0f);
        }
        else
        {

            transform.position = playerTransform.position + new Vector3(offset, 0f, 0f);
        }
    }

    public void CallTurn()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float startOffset;
        float endOffset;
        if (isFacingRight)
        {
            startOffset = -1f;
            endOffset = 1f;
        }
        else
        {
            startOffset = 1f;
            endOffset = -1f;
        }
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            offset = Mathf.Lerp(startOffset, endOffset, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }

    public void FreezePosition()
    {
        freezePos = true;
    }
    public void UnfreezePosition()
    {
        freezePos = false;
    }
}
