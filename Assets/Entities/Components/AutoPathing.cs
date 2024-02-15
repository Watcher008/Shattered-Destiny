using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class AutoPathing : MonoBehaviour
    {
        private MapCharacter player;

        private Coroutine movementCoroutine;

        [SerializeField] private bool allowAutoPathing;

        public void SetAutoPathTarget(int x, int y)
        {
            if (!allowAutoPathing) return;

            var endNode = Pathfinding.instance.GetNode(x, y);
            if (endNode == null) return;

            if (endNode.Terrain != null && !endNode.Terrain.CanTravelOnFoot)
            {
                Debug.Log("Cannot move to this node.");
                return;
            }

            var path = Pathfinding.instance.FindNodePath(player.Node, endNode);
            SetPath(path);
        }

        private void SetPath(List<PathNode> pathNodes)
        {
            if (pathNodes[0] == player.Node) pathNodes.RemoveAt(0);

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
                // Wait until it's the player's turn to move again
                while (GameManager.CurrentPhase == TurnPhase.NPC_Fast || GameManager.CurrentPhase  == TurnPhase.NPC_Slow)
                {
                    yield return null;
                }
                Debug.LogWarning("I broke this with the recent changes. Don't use it.");

                int dx = nodes[0].X - player.Node.X;
                int dy = nodes[0].Y - player.Node.Y;

                if (player.Move(dx, dy))
                {
                    nodes.RemoveAt(0);
                    GameManager.EndPlayerTurn();
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    // Cannot move into node, stop autopathing
                    break;
                }              
            }
        }
    }
}