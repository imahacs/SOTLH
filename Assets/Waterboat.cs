using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float speed = 10f; // Forward speed of the boat
    public float rotationSpeed = 100f; // Rotation speed of the boat
    public float tiltAmount = 15f; // Tilt amount when turning
    public Rigidbody rb; // Rigidbody component of the boat

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        float moveForward = Input.GetAxis("Vertical"); // Forward/backward input
        float turn = Input.GetAxis("Horizontal"); // Left/right input

        // Move the boat forward
        rb.AddForce(transform.forward * speed * moveForward);

        // Rotate the boat
        rb.AddTorque(transform.up * rotationSpeed * turn);

        // Tilt the boat when turning
        float tilt = turn * tiltAmount;
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, -tilt);
    }

    void FixedUpdate()
    {
        // Apply constant downward force to simulate buoyancy
        rb.AddForce(Vector3.down * rb.mass * Physics.gravity.magnitude);
    }
}