using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraController2D : MonoBehaviour
{
    public float dragSpeed = 1.0f;
    public float zoomSpeed = 5.0f;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    public Tilemap map; // 👈 Gán tilemap trong Inspector
    private Bounds mapBounds;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
        if (map != null)
        {
            mapBounds = map.localBounds;
        }
    }

    void Update()
    {
        // 👉 Nếu chuột đang trên UI thì không xử lý drag
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        HandleMouseDrag();
        HandleMouseZoom();
        HandleTouchInput();
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentMousePos;
            Camera.main.transform.position += difference;

            ClampCameraPosition(); // 👈 Giới hạn vị trí sau kéo
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

            ClampCameraPosition(); // 👈 Giới hạn sau khi zoom
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 prevTouchZero = touchZero.position - touchZero.deltaPosition;
            Vector2 prevTouchOne = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (prevTouchZero - prevTouchOne).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Camera.main.orthographicSize -= difference * Time.deltaTime * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

            ClampCameraPosition(); // 👈 Giới hạn sau khi zoom bằng touch
        }
    }

    void ClampCameraPosition()
    {
        Camera cam = Camera.main;
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float minX = mapBounds.min.x + horzExtent;
        float maxX = mapBounds.max.x - horzExtent;
        float minY = mapBounds.min.y + vertExtent;
        float maxY = mapBounds.max.y - vertExtent;

        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        cam.transform.position = pos;
    }
}
