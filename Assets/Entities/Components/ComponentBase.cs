using UnityEngine;

namespace SD.ECS
{
    public class ComponentBase : MonoBehaviour
    {
        public Entity entity { get; private set; }

        public virtual void Register(Entity entity)
        {
            this.entity = entity;
        }

        public virtual void Unregister()
        {
            entity.RemoveComponentBase(this);
            Destroy(this);
        }
    }
}