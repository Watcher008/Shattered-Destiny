using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform _cam;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _cam = Camera.main.transform.parent;
    }

    private void LateUpdate() => _transform.LookAt(_cam);
}
