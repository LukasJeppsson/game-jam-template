    using UnityEngine;

public class SkateBoardControll : MonoBehaviour
{
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnMove(Vector2 MoveInput)
    {
        rb.MoveRotation(Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, MoveInput.y, gameObject.transform.rotation.eulerAngles.z));
    }

    private void CorrectDirection()
    {
        // Preserve vertical velocity (gravity/jumps) but restrict horizontal motion
        Vector3 currentVelocity = rb.linearVelocity;

        // Get horizontal forward direction (ignore any up/down tilt)
        Vector3 forward = transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 1e-6f)
        {
            // Avoid division by zero if forward is nearly vertical
            return;
        }
        forward.Normalize();

        // Extract current horizontal velocity (x,z)
        Vector3 horizontalVel = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        // Project horizontal velocity onto forward direction to keep only forward/back component
        float forwardSpeed = Vector3.Dot(horizontalVel, forward);

        // Prevent moving backwards relative to facing direction: clamp to zero
        if (forwardSpeed < 0f)
        {
            forwardSpeed = 0f;
        }

        Vector3 newHorizontalVel = forward * forwardSpeed;

        // Reassemble velocity preserving vertical component
        rb.linearVelocity = new Vector3(newHorizontalVel.x, currentVelocity.y, newHorizontalVel.z);
    }
}

