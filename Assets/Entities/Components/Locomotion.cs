using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class Locomotion : ComponentBase
    {
        [SerializeField] private int speed;
        [SerializeField] private int movementCost;

        private Actor actor;
        private GridPosition position;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            position = entity.GetComponentBase<GridPosition>();
            actor = entity.GetComponentBase<Actor>();
        }

        public bool CanMoveToPosition(Vector2Int direction)
        {
            if (!actor.IsTurn) return false; //can only move on turn

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

            //cost is equal to base movement cost plus the cost of the node being moved into
            int cost = movementCost + Pathfinding.instance.GetNode(direction.x, direction.y).movementCost - speed;
            actor.SpendEnergy(cost);

            position.SetPosition(direction.x, direction.y);
        }
    }
}