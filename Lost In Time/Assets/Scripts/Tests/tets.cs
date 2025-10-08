using UnityEngine;
using UnityEngine.InputSystem;

public class tets : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse world position every frame (not in field declaration!)
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Get direction vector from object to mouse
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;

        // Calculate angle in radians
        float angleRad = Mathf.Atan2(direction.y, direction.x);

        // Convert to degrees
        float angleDeg = angleRad * Mathf.Rad2Deg;


        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        transform.position += (Vector3)direction * 1 * Time.deltaTime;
    }
}
