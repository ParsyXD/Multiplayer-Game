using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Movement : NetworkBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float maxGroundDistance = 1.5f;
    public float gravityChangeRotationTransition = 10;
    public float gravity = 2;
    public Transform cameraTransform;
    public bool isGrounded;
    public Transform checkTransform;
    public Camera camera;


    private Rigidbody rb;
    private Vector3 moveDirection;
    Vector3 downDirection;
    public bool isOnShip = false;
    public Transform ship;
    private Vector3 lastShipPosition;
    //NetworkIdentity identity;

    private float rotationX = 0;

    private void Start()
    {
        if (hasAuthority) 
        {
            camera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        rb = GetComponent<Rigidbody>();

    }

    

    void Update()
    {
        if (hasAuthority) 
        {
                    // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        // Camera Rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        downDirection = -transform.up; // Use the player's downward vector
    
         // Align player rotation with ground
        AlignWithGround();
        }
 
    }

    void FixedUpdate()
    {
        if (hasAuthority) 
        {
        // Apply Movement
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
    }

    void AlignWithGround()
    {
        RaycastHit hit;
        float maxGroundDistance = 3.0f; // Adjust this value as needed.

        if (Physics.Raycast(checkTransform.position, downDirection, out hit, maxGroundDistance))
        {
            isGrounded = true;
            Vector3 groundNormal = hit.normal;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, gravityChangeRotationTransition * Time.deltaTime);

            // Use the ground normal as the force direction
            rb.AddForce(groundNormal.normalized * -gravity, ForceMode.Acceleration);

        if (hit.transform.gameObject.CompareTag("Ship"))
        {
            isOnShip = true;
            ship = hit.transform;
            if (transform.parent != ship.transform)
            {
                transform.SetParent(ship.transform);
            }
        }

        if (!hit.transform.gameObject.CompareTag("Ship"))
        {
            isOnShip = false;
            ship = null;
            if (transform.parent != null) transform.SetParent(null);
        }

        }
        else isGrounded = false;
    }




    void OnDrawGizmos()
    {
        // Draw a Gizmo in the Scene view to visualize the raycast
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(checkTransform.position, checkTransform.position + downDirection * maxGroundDistance);
    }

}



#warning if tag planet, point of gravity is middle of planet (transform.position)
#warning fix gravity to be only in direction of object