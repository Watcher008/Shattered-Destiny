using UnityEngine;

namespace SD.ECS
{
    public class ComponentBase : MonoBehaviour
    {
        private Entity _entity;
        public Entity Entity
        {
            get
            {
                if (_entity == null)
                {
                    _entity = GetComponent<Entity>();
                }
                return _entity;
            }
            private set
            {
                _entity = value;
            }
        }

        protected virtual void Start()
        {
            Entity = GetComponent<Entity>();
        }
    }
}