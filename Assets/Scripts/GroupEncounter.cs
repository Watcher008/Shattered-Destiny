using UnityEngine;
using SD.EventSystem;

namespace SD.ECS
{
    public class GroupEncounter : ComponentBase
    {
        [SerializeField] private string encounterName;

        private NodeCollisionListener collisionListener;

        [SerializeField] private GameEvent playerGroupEncounterEvent;

        protected override void Start()
        {
            base.Start();

            collisionListener = GetComponent<NodeCollisionListener>();
            collisionListener.onEntityCollision += OnGroupEncountered;
        }

        private void OnDestroy()
        {
            collisionListener.onEntityCollision -= OnGroupEncountered;
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