using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 3f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 70f;
    public Rect touchArea = new Rect(0.3f, 0.3f, 0.4f, 0.4f); // Orta %40 alan
    
    [Header("Zoom Settings")]
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomStep = 1f; // Her buton tıklamasında yakınlaşma/uzaklaşma miktarı

    private float _currentX = 225f;
    private float _currentY = 30f;
    public float distanceFromTarget = 20f;

    public bool canRotate = true;
    
    void Update()
    {
        if (target == null || IsPointerOverUI()) return;
        if (canRotate)
        {
            HandleRotationInput();
                    // UpdateCameraPosition artık her frame'de çağrılıyor
                    UpdateCameraPosition();
        }
        
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject() || 
               (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
    }

    bool IsInTouchArea(Vector2 position)
    {
        Vector2 normalizedPos = new Vector2(position.x / Screen.width, position.y / Screen.height);
        return touchArea.Contains(normalizedPos);
    }

    void HandleRotationInput()
    {
        // Dokunmatik kontrol
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (IsInTouchArea(touch.position) && touch.phase == TouchPhase.Moved)
            {
                _currentX += touch.deltaPosition.x * rotationSpeed * 0.02f;
                _currentY -= touch.deltaPosition.y * rotationSpeed * 0.02f;
                _currentY = Mathf.Clamp(_currentY, minVerticalAngle, maxVerticalAngle);
            }
        }
        // Fare kontrol
        else if (Input.GetMouseButton(0) && IsInTouchArea(Input.mousePosition))
        {
            _currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            _currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            _currentY = Mathf.Clamp(_currentY, minVerticalAngle, maxVerticalAngle);
        }
    }

    public void ZoomIn()
    {
        distanceFromTarget -= zoomStep;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minDistance, maxDistance);
        // Zoom yapınca hemen kamera pozisyonunu güncelle
        UpdateCameraPosition();
    }

    public void ZoomOut()
    {
        distanceFromTarget += zoomStep;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minDistance, maxDistance);
        // Zoom yapınca hemen kamera pozisyonunu güncelle
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        if (target == null) return;
        
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        Vector3 position = rotation * new Vector3(0f, 0f, -distanceFromTarget) + target.position;
        
        transform.rotation = rotation;
        transform.position = position;
    }
}