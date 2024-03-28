using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmatureController : MonoBehaviour
{
    private InputAction mousePosition;
    private Transform _transform;
    private Coroutine panCoroutine;
    private Vector2 dragOrigin;
    private bool isPanning;

    [Header("Panning")]
    [SerializeField] private bool updateOrigin;

    [SerializeField] private float _staticSmoothing = 0.1f;
    [SerializeField] private float _dynamicSmoothing = 50f;

    private void OnEnable()
    {
        _transform = transform;
        var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
        mousePosition = input.actions["Mouse Position"];
        input.actions["RMB"].performed += BeginPanCamera;
        input.actions["RMB"].canceled += StopCameraPan;
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
        }
    }

    private void BeginPanCamera(InputAction.CallbackContext obj)
    {
        dragOrigin = mousePosition.ReadValue<Vector2>();
        if (panCoroutine != null) StopCoroutine(panCoroutine);
        panCoroutine = StartCoroutine(PanCamera());
    }

    private void StopCameraPan(InputAction.CallbackContext obj)
    {
        isPanning = false;
    }

    private IEnumerator PanCamera()
    {
        isPanning = true;
        while (isPanning)
        {

            Vector2 difference = dragOrigin - mousePosition.ReadValue<Vector2>();
            float direction = -difference.x * Time.deltaTime;
            
            if (updateOrigin) direction *= _dynamicSmoothing;
            else direction *= _staticSmoothing;

            _transform.RotateAround(_transform.position, Vector3.up, direction);
            
            if (updateOrigin) dragOrigin = mousePosition.ReadValue<Vector2>();
            yield return null;
        }
    }
}
