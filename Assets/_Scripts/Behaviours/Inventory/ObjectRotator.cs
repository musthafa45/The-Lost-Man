using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 300f;

    private void Update()
    {
        float horizontalInput = InputManager.Instance.GetInputAxisXAndZ().x;
        float verticalInput = InputManager.Instance.GetInputAxisXAndZ().z;

        float horizontalRot = horizontalInput * rotationSpeed * Time.deltaTime;
        float verticalRot = verticalInput * rotationSpeed * Time.deltaTime;

        // Rotate in world space around global axes
        transform.Rotate(Vector3.up, horizontalRot, Space.World);
        transform.Rotate(Vector3.right, -verticalRot, Space.World);
    }
}
