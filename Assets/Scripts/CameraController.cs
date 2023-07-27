using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionProperty mousePosition;
    [SerializeField] private InputActionProperty rightMouseButton;
    [SerializeField] private InputActionProperty mouseScroll;

    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer mapRenderer;
    [SerializeField] private float maxZoom = 4.12f;
    [SerializeField] private float minZoom = 1.0f;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 dragOrigin;

    private Coroutine panCoroutine;
    private bool isPanning;

    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y /2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y /2f;
    }

    private void OnEnable()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        rightMouseButton.action.performed += BeginPanCamera;
        rightMouseButton.action.canceled += i => isPanning = false;
        mouseScroll.action.performed += Zoom;
    }

    private void OnDisable()
    {
        isPanning = false;
        rightMouseButton.action.performed -= BeginPanCamera;
        rightMouseButton.action.canceled -= i => isPanning = false;
        mouseScroll.action.performed -= Zoom;
        if (panCoroutine != null) StopCoroutine(panCoroutine);
    }

    private void BeginPanCamera(InputAction.CallbackContext obj)
    {
        dragOrigin = cam.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
        if (panCoroutine != null) StopCoroutine(panCoroutine);
        panCoroutine = StartCoroutine(PanCamera());
    }

    private void Zoom(InputAction.CallbackContext obj)
    {
        float zoom = obj.ReadValue<Vector2>().y;

        cam.orthographicSize -= zoom * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3 (newX, newY, targetPosition.z);
    }

    private IEnumerator PanCamera()
    {
        isPanning = true;
        while (isPanning)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
            transform.position = ClampCamera(transform.position + difference);
            yield return null;
        }
    }
}
