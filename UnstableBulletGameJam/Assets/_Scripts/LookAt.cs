using UnityEngine;

public class LookAt : MonoBehaviour
{
    // Speed at which the player rotates to face the cursor.
    public float lookSpeed = 10f;

    // Additional angle offset applied to the rotation 
    public float offset;

    private void Update()
    {
        // Get the current player position
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        // Lock the Z-axis to 0 to keep the rotation in topdown 2D.
        targetPosition.z = 0f;

        // Calculate the direction vector from the player to the mouse position.
        Vector3 direction = targetPosition - transform.position;

        // Calculate the target angle in degrees and apply the offset.
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;

        // Smoothly interpolate the current angle towards the target angle over time.
        float rotation = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * lookSpeed);

        // Apply the new rotation to the player object on the Z-axis.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }
}