using UnityEngine;
using SD.EventSystem;

namespace SD.ECS
{
    public class GroupEncounter : ComponentBase
    {
        [SerializeField] private string encounterName;

        private NodeCollisionListener collisionListener;

        [SerializeField] private GameEvent playerGroupEncounterEvent;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            collisionListener = entity.GetComponentBase<NodeCollisionListener>();
            collisionListener.onEntityCollision += OnGroupEncountered;
        }

        public override void Unregister()
        {
            collisionListener.onEntityCollision -= OnGroupEncountered;
            base.Unregister();
        }

        private void OnGroupEncountered(Entity thisEntity, Entity otherEntity)
        {
            if (otherEntity == null) return;

            if (otherEntity.CompareTag("Player"))
            {
                Debug.Log("Encountered " + encounterName);
                //Will need to set some reference to this group
                playerGroupEncounterEvent?.Invoke();
            }
        }
    }
}