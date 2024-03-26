using UnityEngine;
using SD.EventSystem;

namespace SD.Primitives
{
    [CreateAssetMenu(menuName = "Primitives/Float")]
    public class FloatReference : ScriptableObject
    {
        [SerializeField] private float _value;
        [SerializeField] private GameEvent onValueChangedEvent;

        public float Value
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

