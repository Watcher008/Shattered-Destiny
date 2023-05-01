using UnityEngine;

/// <summary>
/// Disables the GameObject on awake.
/// </summary>
public class DisableOnAwake : MonoBehaviour
{
    private void Awake() => gameObject.SetActive(false);
}