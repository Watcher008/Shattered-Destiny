using UnityEngine;

namespace SD.EventSystem
{
    /// <summary>
    /// Logs all events to the console.
    /// </summary>
    public class EventLogger : MonoBehaviour
    {
        [SerializeField] private bool logEvents;
        /// <summary>
        /// Debugging tool - Raise an assertion alert if event is triggered.
        /// </summary>
        [Header("*Optional")]
        [SerializeField] private GameEvent watchForEvent;

        /// <summary>
        /// Register event.
        /// </summary>
        private void OnEnable()
        {
            GameEvent.AnyEventRaised += LogEvent;
        }

        /// <summary>
        /// Deregister event.
        /// </summary>
        private void OnDisable()
        {
            GameEvent.AnyEventRaised -= LogEvent;
        }

        /// <summary>
        /// Log event to console.
        /// </summary>
        /// <param name="gameEvent">GameEvent to log.</param>
        private void LogEvent(GameEvent gameEvent)
        {
            if (!logEvents) return;

            if (watchForEvent != null && gameEvent.name.Equals(watchForEvent.name))
            {
                Debug.LogAssertion(gameEvent.name);
                return;
            }

            Debug.Log(gameEvent.name);
        }
    }
}