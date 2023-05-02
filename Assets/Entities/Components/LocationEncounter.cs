using UnityEngine;
using SD.EventSystem;
using SD.LocationSystem;

namespace SD.ECS
{
    public class LocationEncounter : ComponentBase
    {
        private LocationData location;

        private NodeCollisionListener collisionListener;

        [SerializeField] private PlayerLocationReference playerLocation;
        [SerializeField] private GameEvent playerLocationEncounterEvent;

        public void SetLocation(LocationData location)
        {
            this.location = location;
            entity.GetComponentBase<EntityRenderer>().SetSprite(location.type.sprite);
            entity.GetComponentBase<GridPosition>().SetPosition(location.x, location.y);
        }

        public override void Register(Entity entity)
        {
            base.Register(entity);
            collisionListener = entity.GetComponentBase<NodeCollisionListener>();
            collisionListener.onEntityCollision += OnLocationVisited;
        }

        private void SetLocationValues()
        {
            if (entity == null) Debug.Log("entity is null");
            if (entity.GetComponentBase<EntityRenderer>() == null) Debug.Log("render null");
            if (location == null) Debug.Log("Location null");
            if (location.type == null) Debug.Log("type null");
            entity.GetComponentBase<EntityRenderer>().SetSprite(location.type.sprite);
            entity.GetComponentBase<GridPosition>().SetPosition(location.x, location.y);
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
                playerLocation.playerLocation = location;
                playerLocationEncounterEvent?.Invoke();
            }
        }
    }
}