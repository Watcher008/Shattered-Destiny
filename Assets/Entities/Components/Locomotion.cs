using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class Locomotion : ComponentBase
    {
        //0 on foot, 100 on horseback?
        [SerializeField] private int travelSpeed = 0;

        private Actor actor;
        private GridPosition position;

        protected override void Start()
        {
            base.Start();

            position = GetComponent<GridPosition>();
            actor = GetComponent<Actor>();
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
            var newNode = Pathfinding.instance.GetNode(direction.x, direction.y);
            int cost = GameManager.pointsToAct + newNode.movementCost - travelSpeed;
            actor.SpendActionPoints(cost);

            position.SetPosition(direction.x, direction.y);

        }
    }
}