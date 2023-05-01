using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SD.EventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        [SerializeField] private UnityEvent callbackEvent;

#if UNITY_EDITOR
#pragma warning disable 0414
        // Display notes field in the inspector.
        [Multiline, SerializeField]
        [FormerlySerializedAs("DeveloperNotes")]
        private string developerNotes = "";
#pragma warning restore 0414
#endif

        // Register and deregister events
        private void Awake() => gameEvent.RegisterListener(this);
        private void OnDestroy() => gameEvent.DeregisterListener(this);

        // Invoke event
        public void RaiseEvent() => callbackEvent.Invoke();
    }
}