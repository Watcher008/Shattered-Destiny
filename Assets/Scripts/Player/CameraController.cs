using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputAction mousePosition;

    [SerializeField] private SpriteRenderer mapRenderer;
    
    private Camera cam;

    private float maxZoom = 15.0f;
    private float minZoom = 2.0f;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Coroutine panCoroutine;
    private Vector3 dragOrigin;
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
        cam = GetComponent<Camera>();
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        ClampCamera(GameObject.FindGameObjectWithTag("Player").transform.position);

        var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();

        mousePosition = input.actions["Mouse Position"];

        input.actions["RMB"].performed += BeginPanCamera;
        input.actions["RMB"].canceled += StopCameraPan;
        input.actions["Mouse Scroll"].performed += Zoom;
    }

    private void OnDisable()
    {
        isPanning = false;
        if (panCoroutine != null) StopCoroutine(panCoroutine);

        var obj = GameObject.FindGameObjectWithTag("PlayerInput");
        if (obj != null && obj.TryGetComponent(out PlayerInput input))
        {
            input.actions["RMB"].performed -= BeginPanCamera;
            input.actions["RMB"].canceled -= StopCameraPan;
            input.actions["Mouse Scroll"].performed -= Zoom;
        }
    }

    private void BeginPanCamera(InputAction.CallbackContext obj)
    {
        dragOrigin = cam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        if (panCoroutine != null) StopCoroutine(panCoroutine);
        panCoroutine = StartCoroutine(PanCamera());
    }

    private void StopCameraPan(InputAction.CallbackContext obj)
    {
        isPanning = false;
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
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
            transform.position = ClampCamera(transform.position + difference);
            yield return null;
        }
    }
}
