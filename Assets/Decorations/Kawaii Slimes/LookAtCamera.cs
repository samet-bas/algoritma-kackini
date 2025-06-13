using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 dir = cam.transform.position - transform.position;
        dir.y = 0; // sadece yatay döndür, yukarı/aşağı bakmasın
        transform.rotation = Quaternion.LookRotation(-dir);
    }
}