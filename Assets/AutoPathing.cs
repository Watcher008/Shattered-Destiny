using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class AutoPathing : MonoBehaviour, IComponent
    {
        private Pathfinding pathfinding;

        private Entity entity;
        private Position position;
        private Locomotion locomotion;

        private Coroutine movementCoroutine;

        public void AddComponent(Entity entity)
        {
            this.entity = entity;
            entity.components.Add(this);

            pathfinding = Pathfinding.instance;
            position = entity.GetComponent<Position>();
            locomotion = entity.GetComponent<Locomotion>();
        }

        public void RemoveComponent(Entity entity)
        {
            entity.components.Remove(this);
        }

        public void SetAutoPathTarget(int x, int y)
        {
            if (!entity.IsTurn) return;

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

        public void SetPath(List<WorldMapNode> pathNodes)
        {
            if (pathNodes[0].x == position.x && pathNodes[0].y == position.y) pathNodes.RemoveAt(0);

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(FollowNodePath(pathNodes));
        }

        public void CancelAutoPathing()
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        }

        private IEnumerator FollowNodePath(List<WorldMapNode> nodes)
        {
            while (nodes.Count > 0)
            {
                while (!entity.IsTurn) yield return null;

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