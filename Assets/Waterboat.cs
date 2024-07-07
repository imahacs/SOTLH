using UnityEngine;

public class BoatController : MonoBehaviour
{
    public Rigidbody rb;
    public Transform[] floatPoints; // Points where the boat interacts with water
    public float waterLevel = 0.0f; // Water level in the scene
    public float floatHeight = 2.0f; // Height the boat should float at
    public float bounceDamp = 0.05f; // Damping for the bouncing
    public float waterDrag = 3.0f; // Water drag force
    public float waterAngularDrag = 1.0f; // Water angular drag force

    public float speed = 10f; // Forward speed of the boat
    public float tiltAmount = 5f; // Tilt amount when moving

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.drag = waterDrag;
        rb.angularDrag = waterAngularDrag;
    }

    void FixedUpdate()
    {
        ApplyBuoyancy();
        HandleMovement();
    }

    void ApplyBuoyancy()
    {
        foreach (Transform point in floatPoints)
        {
            // Calculate how deep each point is submerged
            float depth = waterLevel - point.position.y;

            if (depth > 0)
            {
                // Calculate the buoyancy force
                float forceFactor = depth / floatHeight;
                Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
                rb.AddForceAtPosition(uplift, point.position, ForceMode.Acceleration);

                Debug.Log($"Applying buoyancy at {point.name} with force {uplift}");
            }
        }
    }

    void HandleMovement()
    {
        // Get input for forward/backward movement (using Vertical axis for simplicity)
        float moveForward = Input.GetAxis("Vertical");

        // Get input for strafing left/right
        float strafe = Input.GetAxis("Horizontal");

        // Calculate the movement direction based on input
        Vector3 moveDirection = transform.forward * moveForward + transform.right * strafe;
        moveDirection.Normalize(); // Normalize to ensure diagonal movement isn't faster

        // Move the boat forward and backward
        Vector3 forwardMovement = moveDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Tilt the boat when moving
        float tilt = -moveForward * tiltAmount;
        Quaternion tiltRotation = Quaternion.Euler(tilt, rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z);
        rb.MoveRotation(tiltRotation);

        Debug.Log($"Moving forward with input {moveForward} and tilt {tilt}");
    }
}