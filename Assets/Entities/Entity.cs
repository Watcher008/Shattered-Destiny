using System.Collections.Generic;
using UnityEngine;

namespace SD.ECS
{
    public class Entity : MonoBehaviour
    {
        [field: SerializeField] public bool IsSentient { get; private set; }
        [field: SerializeField] public bool BlocksMovement { get; private set; }
        public bool HasBehavior { get; set; }

        private List<ComponentBase> _components;

        private void Awake() => GetExistingComponents();

        private void GetExistingComponents()
        {
            _components = new List<ComponentBase>();
            var comps = GetComponents<ComponentBase>();
            _components.AddRange(comps);

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Register(this);
            }
        }

        public T GetComponentBase<T>() where T : ComponentBase
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType().Equals(typeof(T))) return (T)_components[i];
            }
            return null;
        }

        public void RemoveComponentBase(ComponentBase component)
        {
            _components.Remove(component);
            component.Unregister();
        }
    }
}