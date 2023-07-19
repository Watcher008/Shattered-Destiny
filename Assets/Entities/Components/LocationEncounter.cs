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

        protected override void Start()
        {
            base.Start();

            collisionListener = GetComponent<NodeCollisionListener>();
            collisionListener.onEntityCollision += OnLocationVisited;
        }

        public void SetLocation(LocationData location)
        {
            this.location = location;
            GetComponent<EntityRenderer>().SetSprite(location.type.sprite);
            GetComponent<GridPosition>().SetPosition(location.x, location.y);
        }

        private void SetLocationValues()
        {
            if (Entity == null) Debug.Log("entity is null");
            if (GetComponent<EntityRenderer>() == null) Debug.Log("render null");
            if (location == null) Debug.Log("Location null");
            if (location.type == null) Debug.Log("type null");
            GetComponent<EntityRenderer>().SetSprite(location.type.sprite);
            GetComponent<GridPosition>().SetPosition(location.x, location.y);
        }

        private void OnDestroy()
        {
            collisionListener.onEntityCollision -= OnLocationVisited;
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