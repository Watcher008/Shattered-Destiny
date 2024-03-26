using SD.EventSystem;
using UnityEngine;

namespace SD.Primitives
{
    [CreateAssetMenu(menuName = "Primitives/String")]
    public class StringReference : ScriptableObject
    {
        [SerializeField] private string _value;
        [SerializeField] private GameEvent onValueChangedEvent;
        [SerializeField] private bool _invokeOnNullValue;

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;

                    if (_invokeOnNullValue) onValueChangedEvent?.Invoke();
                    else if (!string.IsNullOrEmpty(_value)) onValueChangedEvent?.Invoke();
                }
            }
        }
    }
}