using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class Locomotion : MonoBehaviour, IComponent
    {
        [SerializeField] private int movementCost;

        private Entity entity;
        private Position position;

        public void AddComponent(Entity entity)
        {
            this.entity = entity;
            entity.components.Add(this);
            FindPosition();
        }

        private void FindPosition()
        {
            position = entity.GetComponent<Position>();
            if (position == null)
            {
                position = new Position();
                position.AddComponent(entity); //I feel like this is backwards...
            }
        }

        public void RemoveComponent(Entity entity)
        {
            entity.components.Remove(this);
        }

        public bool CanMoveToPosition(Vector2Int direction)
        {
            if (!entity.IsTurn) return false; //can only move on turn

            direction.x += position.x;
            direction.y += position.y;

            if (!Pathfinding.PositionIsValid(direction.x, direction.y)) return false; //not a valid node

            //Will also need to check the node to see if it is being blocked by obstacles or other (hostile) actors

            return true;
        }

        public void MoveEntity(Vector2Int direction)
        {
            if (!CanMoveToPosition(direction)) return;

            direction.x += position.x;
            direction.y += position.y;

            //cost is equal to base movement cost plus the averaged costs of the from node and to node
            int cost = movementCost + (Pathfinding.instance.GetNode(position.x, position.y).movementCost + Pathfinding.instance.GetNode(direction.x, direction.y).movementCost) / 2;
            entity.SpendEnergy(cost);

            position.SetPosition(direction.x, direction.y);
        }
    }
}