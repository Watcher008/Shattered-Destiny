using System;
using System.Collections.Generic;
using UnityEngine;

namespace SD.EventSystem
{
    [CreateAssetMenu(menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        public static event Action<GameEvent> AnyEventRaised;
        public bool EventEnabled = true;

        // Hash set to insure events are unique.
        private readonly HashSet<GameEventListener> _eventListeners = new HashSet<GameEventListener>();

        // Register and Deregister events.
        public void RegisterListener(GameEventListener gameEventListener) => _eventListeners.Add(gameEventListener);
        public void DeregisterListener(GameEventListener gameEventListener) => _eventListeners.Remove(gameEventListener);

        /// <summary>
        /// Invoke and log the event.
        /// </summary>
        public void Invoke()
        {
            if (!EventEnabled) return;

            AnyEventRaised?.Invoke(this);

            foreach (var globalEventListener in _eventListeners)
            {
                globalEventListener.RaiseEvent();
            }
        }

        // Enabled toggle
        public void EnableEvent() => EventEnabled = true;
        public void DisableEvent() => EventEnabled = false;
    }
}