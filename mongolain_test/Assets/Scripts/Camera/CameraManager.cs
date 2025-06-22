using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Existing Variables (e.g. target, offset, etc.)

    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    public Transform target;
    public Vector3 offset = new Vector3(10, 10, -10);
    public float followSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    private Camera cam;


    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Apply camera boundaries (clamp position)
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.z = Mathf.Clamp(smoothedPosition.z, minZ, maxZ);

        transform.position = smoothedPosition;
    }

    public void Zoom(float increment)
    {
        float newSize = cam.orthographicSize - increment * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }


}
