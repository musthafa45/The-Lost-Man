using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 1.5f;
    [SerializeField] private float lerpDuration = 1;

    private Transform characterTransform;
    Vector2 velocity;
    Vector2 frameVelocity;
    private float oldSmoothValue;
    private Torch torch;
    void Start()
    {
        characterTransform = FirstPersonController.Instance.transform;
        torch = Torch.Instance;
        oldSmoothValue = smoothing;
    }

    void Update()
    {
        smoothing = torch.IsActive() ? oldSmoothValue : 0;
        //// Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, lerpDuration / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        //// Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        characterTransform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
