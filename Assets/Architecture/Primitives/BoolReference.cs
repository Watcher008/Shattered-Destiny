using UnityEngine;
using SD.EventSystem;

namespace SD.Primitives
{
    [CreateAssetMenu(menuName = "Primitives/Bool")]
    public class BoolReference : ScriptableObject
    {
        [SerializeField] private bool _value;
        [SerializeField] private GameEvent onValueChangedEvent;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    onValueChangedEvent?.Invoke();
                }
            }
        }
    }
}

