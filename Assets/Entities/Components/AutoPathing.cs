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

        public override void Register(Entity entity)
        {
            base.Register(entity);
            pathfinding = Pathfinding.instance;
            
            actor = entity.GetComponentBase<Actor>();
            position = entity.GetComponentBase<GridPosition>();
            locomotion = entity.GetComponentBase<Locomotion>();
        }

        public void SetAutoPathTarget(int x, int y)
        {
            if (!allowAutoPathing) return;
            if (!actor.IsTurn) return;

            var endNode = pathfinding.GetNode(x, y);
            if (endNode == null) return;

            if (endNode.terrain != null && !endNode.terrain.CanTravelOnFoot)
            {
                Debug.Log("Cannot move to this node.");
                return;
            }

            var path = pathfinding.FindNodePath(position.x, position.y, endNode.x, endNode.y);
            SetPath(path);
        }

        private void SetPath(List<WorldNode> pathNodes)
        {
            if (pathNodes[0].x == position.x && pathNodes[0].y == position.y) pathNodes.RemoveAt(0);

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(FollowNodePath(pathNodes));
        }

        public void CancelAutoPathing()
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        }

        private IEnumerator FollowNodePath(List<WorldNode> nodes)
        {
            while (nodes.Count > 0)
            {
                while (!actor.IsTurn) yield return null;

                var direction = new Vector2Int(nodes[0].x - position.x, nodes[0].y - position.y);

                //position is not valid, exit out of coroutine
                if (!locomotion.CanMoveToPosition(direction)) yield break;

                Action.MovementAction(locomotion, direction);

                nodes.RemoveAt(0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}