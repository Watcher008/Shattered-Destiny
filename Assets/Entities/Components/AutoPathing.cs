using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class AutoPathing : ComponentBase
    {
        private Pathfinding pathfinding;

        private Actor actor;
        private GridPosition position;
        private Locomotion locomotion;

        private Coroutine movementCoroutine;

        [SerializeField] private bool allowAutoPathing;

        protected override void Start()
        {
            base.Start();
            pathfinding = Pathfinding.instance;

            actor = GetComponent<Actor>();
            position = GetComponent<GridPosition>();
            locomotion = GetComponent<Locomotion>();
        }

        public void SetAutoPathTarget(int x, int y)
        {
            if (!allowAutoPathing) return;
            if (!actor.IsTurn) return;

            var endNode = pathfinding.GetNode(x, y);
            if (endNode == null) return;

            if (endNode.Terrain != null && !endNode.Terrain.CanTravelOnFoot)
            {
                Debug.Log("Cannot move to this node.");
                return;
            }

            var path = pathfinding.FindNodePath(position.x, position.y, endNode.X, endNode.Y);
            SetPath(path);
        }

        private void SetPath(List<PathNode> pathNodes)
        {
            if (pathNodes[0].X == position.x && pathNodes[0].Y == position.y) pathNodes.RemoveAt(0);

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(FollowNodePath(pathNodes));
        }

        public void CancelAutoPathing()
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        }

        private IEnumerator FollowNodePath(List<PathNode> nodes)
        {
            while (nodes.Count > 0)
            {
                while (!actor.IsTurn)
                {
                    yield return null;
                }

                var direction = new Vector2Int(nodes[0].X - position.x, nodes[0].Y - position.y);

                //position is not valid, exit out of coroutine
                if (!locomotion.CanMoveToPosition(direction)) yield break;

                Action.MovementAction(locomotion, direction);

                nodes.RemoveAt(0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}