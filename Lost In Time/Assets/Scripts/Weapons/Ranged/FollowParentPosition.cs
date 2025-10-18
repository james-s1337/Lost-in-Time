using UnityEngine;

public class FollowParentPosition : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private Player player;

    void Awake()
    {
        // Store the initial local position and rotation of the child relative to its parent
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        
    }

    private void Start()
    {
        player = transform.root.GetComponent<Player>();
    }

    void LateUpdate()
    {
        // Ensure the child maintains its initial local position relative to the parent
        transform.localPosition = initialLocalPosition;

        // Ensure the child maintains its initial local rotation (relative to its parent)
        if (player.core.Movement.facingDir == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }   
    }
}