using UnityEngine;
using SD.EventSystem;

namespace SD.Primitives
{
    [CreateAssetMenu(menuName = "Primitives/Int")]
    public class IntReference : ScriptableObject
    {
        [SerializeField] private int _value;
        [SerializeField] private GameEvent onValueChangedEvent;

        public int Value
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