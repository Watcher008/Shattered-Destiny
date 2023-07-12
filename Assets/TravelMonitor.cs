using SD.PathingSystem;
using UnityEngine;

namespace SD.ECS
{
    public class TravelMonitor : MonoBehaviour
    {
        private GridPosition _gridPos;

        private void Start()
        {
            _gridPos = GetComponent<GridPosition>();
            _gridPos.onPositionChange += OnPositionChanged;
        }

        private void OnPositionChanged()
        {
            var newNode = Pathfinding.instance.GetNode(_gridPos.x , _gridPos.y);

            Debug.Log("Entering node " + newNode.x + ", " + newNode.y);
        }
    }
}

