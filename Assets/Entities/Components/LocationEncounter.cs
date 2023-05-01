using UnityEngine;
using SD.EventSystem;
using System;

namespace SD.ECS
{
    public class LocationEncounter : ComponentBase
    {
        private NodeCollisionListener collisionListener;

        [SerializeField] private GameEvent playerLocationEncounterEvent;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            collisionListener = entity.GetComponentBase<NodeCollisionListener>();
            collisionListener.onEntityCollision += OnLocationVisited;
        }

        public override void Unregister()
        {
            collisionListener.onEntityCollision -= OnLocationVisited;
            base.Unregister();
        }

        private void OnLocationVisited(Entity thisEntity, Entity otherEntity)
        {
            if (otherEntity == null) return;

            if (otherEntity.CompareTag("Player"))
            {
                playerLocationEncounterEvent?.Invoke();
            }
        }
    }
}