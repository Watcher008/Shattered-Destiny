using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class Locomotion : ComponentBase
    {
        //100 on foot, 500 on horseback?
        [SerializeField, Range(100, 500)] private int travelSpeed = 100;

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
            int cost = Mathf.RoundToInt(GameManager.pointsToAct * (1 / newNode.MovementModifier) - travelSpeed);
            actor.SpendActionPoints(cost);

            position.SetPosition(direction.x, direction.y);

        }
    }
}