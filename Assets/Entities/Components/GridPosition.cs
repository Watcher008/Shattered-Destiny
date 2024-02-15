using SD.PathingSystem;
using UnityEngine;

namespace SD.ECS
{
    public class GridPosition : MonoBehaviour
    {
        public delegate void OnPositionChangeCallback();
        public OnPositionChangeCallback onPositionChange;

        private PathNode _node;
        public PathNode Node => _node;

        public int x { get; set; }
        public int y { get; set; }


        public void SetPosition(int x, int y)
        {
            var newNode = Pathfinding.instance.GetNode(x, y);

            transform.position = newNode.WorldPosition;

            _node = newNode;

            onPositionChange?.Invoke();
        }
    }
}