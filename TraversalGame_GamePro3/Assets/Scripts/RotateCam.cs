using UnityEngine;

public class RotateCam: MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 180f; // Degrees per second

    private float targetAngle;
    private bool isRotating = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        targetAngle = mainCamera.transform.eulerAngles.z;
    }

    void Update()
    {
        if (isRotating)
        {
            RotateCamera();
        }

        // Example keys to rotate (can be replaced with input system or events)
        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartRotation(90f); // Clockwise 90
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartRotation(-90f); // Counterclockwise 90
    }

    public void StartRotation(float angleDelta)
    {
        if (!isRotating)
        {
            targetAngle = Mathf.Round((mainCamera.transform.eulerAngles.z + angleDelta) % 360f);
            isRotating = true;
        }
    }

    private void RotateCamera()
    {
        float currentZ = mainCamera.transform.eulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotationSpeed * Time.deltaTime);
        mainCamera.transform.eulerAngles = new Vector3(0, 0, newZ);

        if (Mathf.Approximately(newZ, targetAngle))
        {
            isRotating = false;
            SetGravityDirection(targetAngle);
        }
    }

    private void SetGravityDirection(float angle)
    {
        // Normalize angle to 0-360
        angle = (angle + 360f) % 360f;

        Vector2 gravityDir = Vector2.down;

        if (Mathf.Approximately(angle, 0f))
            gravityDir = Vector2.down;
        else if (Mathf.Approximately(angle, 90f))
            gravityDir = Vector2.left;
        else if (Mathf.Approximately(angle, 180f))
            gravityDir = Vector2.up;
        else if (Mathf.Approximately(angle, 270f))
            gravityDir = Vector2.right;
        else
        {
            // For non-90-degree multiples
            float radians = angle * Mathf.Deg2Rad;
            gravityDir = new Vector2(-Mathf.Sin(radians), -Mathf.Cos(radians));
        }

        Physics2D.gravity = gravityDir * 9.81f; // Adjust magnitude if needed
    }
}

