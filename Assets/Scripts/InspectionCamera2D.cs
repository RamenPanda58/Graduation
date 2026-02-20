using UnityEngine;

public class InspectionCamera2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    [Header("Pan Settings")]
    public float panSpeed = 1f;

    [Header("Bounds (Optional)")]
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public bool useBounds = false;

    private Camera cam;
    private Vector3 lastMousePosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        ClampPosition();

        // Exit inspection
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<InspectionController>().FinishInspection();
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = lastMousePosition - currentMousePosition;
            transform.position += difference * panSpeed;
        }
    }

    void ClampPosition()
    {
        if (!useBounds) return;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        transform.position = pos;
    }
}