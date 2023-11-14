using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    public bool isBobbingActive = true;
    public float bobFrequency = 2f;
    public float bobAmount = 0.1f;
    public float tiltAngle = 5f;
    public Transform playerTransform; // The player's transform
    public PlayerMovementAdvanced playerMovement; // Reference to the PlayerMovementAdvanced script

    private float verticalTimer = 0f;
    private float horizontalTimer = 0f;
    private float returnTimer = 0f; // Timer for smooth return when not bobbing
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 lastPlayerPosition;
    private bool returningToOriginal = false;

    private float smoothReturnCooldown = 1f; // Cooldown before starting smooth return
    private float smoothReturnDelayTimer = 0f;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        lastPlayerPosition = playerTransform.position;
    }

    private void Update()
    {
        // Check if any movement flags are active, and turn off the head bobbing effect accordingly
        bool movementFlagsActive = playerMovement.sliding || playerMovement.crouching || playerMovement.wallrunning || playerMovement.climbing;
        isBobbingActive = !movementFlagsActive;

        // Calculate the player's movement speed
        float playerSpeed = (playerTransform.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = playerTransform.position;

        if (isBobbingActive)
        {
            // Calculate the vertical head bobbing
            float verticalBob = Mathf.Sin(verticalTimer * bobFrequency) * bobAmount;

            // Calculate the horizontal head bobbing
            float horizontalBob = Mathf.Sin(horizontalTimer * bobFrequency * 2f) * (bobAmount * 0.25f);

            // Calculate the horizontal tilt
            float tilt = Mathf.Sin(horizontalTimer * bobFrequency * 2f) * tiltAngle;

            // Apply the combined head bobbing
            Vector3 newPosition = originalLocalPosition;
            newPosition.y += verticalBob;
            newPosition.x += horizontalBob;
            transform.localPosition = newPosition;

            // Apply the horizontal tilt
            transform.localRotation = originalLocalRotation * Quaternion.Euler(0f, 0f, tilt);

            // Increment the timers based on the player's movement speed
            verticalTimer += Time.deltaTime * playerSpeed;
            horizontalTimer += Time.deltaTime * playerSpeed;

            // Reset the return timer and delay timer
            returnTimer = 0f;
            smoothReturnDelayTimer = 0f;
            returningToOriginal = false;
        }
        else
        {
            // Increment the smooth return delay timer
            smoothReturnDelayTimer += Time.deltaTime;

            if (smoothReturnDelayTimer >= smoothReturnCooldown && !returningToOriginal)
            {
                // Store the current position and rotation as the target for smooth return
                Vector3 targetLocalPosition = transform.localPosition;
                Quaternion targetLocalRotation = transform.localRotation;

                // Smoothly move the camera back to its original position and rotation
                transform.localPosition = Vector3.Lerp(targetLocalPosition, originalLocalPosition, returnTimer);
                transform.localRotation = Quaternion.Slerp(targetLocalRotation, originalLocalRotation, returnTimer);

                // Increment the return timer
                returnTimer += Time.deltaTime * 2f; // Adjust the return speed here

                // Clamp the return timer to prevent overshooting
                returnTimer = Mathf.Clamp01(returnTimer);

                if (returnTimer >= 1f)
                {
                    // Reset the returning flag to prevent multiple return movements
                    returningToOriginal = true;
                }
            }
        }
    }
}