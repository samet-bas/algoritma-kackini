using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float moveStep = 1f;
    public float rotateStep = 15f;
    public float zoomStep = 2f;
    public float verticalRotateStep = 5f;

    public float minZoom = 5f;
    public float maxZoom = 50f;

    public float minVerticalAngle = 20f;
    public float maxVerticalAngle = 80f;

    public Vector2 moveLimitMin = new Vector2(0, 0);
    public Vector2 moveLimitMax = new Vector2(30, 30);

    private float currentZoom;
    private float currentAngle;
    private float verticalAngle;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = target.position;
        moveLimitMin = new Vector2(startPosition.x - 15f, startPosition.z - 15f);
        moveLimitMax = new Vector2(startPosition.x + 15f, startPosition.z + 15f);

        Vector3 offset = transform.position - target.position;
        currentZoom = offset.magnitude;

        Quaternion rot = Quaternion.LookRotation(-offset.normalized);
        currentAngle = rot.eulerAngles.y;
        verticalAngle = rot.eulerAngles.x;
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    // --- PUBLIC UI FUNCTIONS (tek seferlik adımlarla) ---

    public void MoveForward() => Move(Vector3.forward * moveStep);
    public void MoveBackward() => Move(Vector3.back * moveStep);
    public void MoveLeft() => Move(Vector3.left * moveStep);
    public void MoveRight() => Move(Vector3.right * moveStep);

    public void RotateLeft() => currentAngle -= rotateStep;
    public void RotateRight() => currentAngle += rotateStep;

    public void LookUp()
    {
        verticalAngle += verticalRotateStep;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    public void LookDown()
    {
        verticalAngle -= verticalRotateStep;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    public void ZoomIn()
    {
        currentZoom -= zoomStep;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void ZoomOut()
    {
        currentZoom += zoomStep;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    // --- PRIVATE ---

    private void Move(Vector3 localDirection)
    {
        Vector3 moveDir = localDirection.x * transform.right + localDirection.z * transform.forward;
        moveDir.y = 0;
        moveDir.Normalize();

        Vector3 newPosition = target.position + moveDir;
        newPosition.x = Mathf.Clamp(newPosition.x, moveLimitMin.x, moveLimitMax.x);
        newPosition.z = Mathf.Clamp(newPosition.z, moveLimitMin.y, moveLimitMax.y);
        newPosition.y = target.position.y;

        target.position = newPosition;
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(verticalAngle, currentAngle, 0);
        Vector3 direction = rotation * Vector3.back;
        transform.position = target.position + direction * currentZoom;
        transform.LookAt(target);
    }
}
