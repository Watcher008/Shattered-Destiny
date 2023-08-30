using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class FieldOfView : ComponentBase
    {
        [SerializeField] private int sightDistance = 5;

        private GridPosition position;

        private List<PathNode> visibleNodes;

        protected override void Start()
        {
            base.Start();

            position = GetComponent<GridPosition>();
            position.onPositionChange += UpdateFieldOfView;
            Invoke("UpdateFieldOfView", 0.1f);
        }
        private void OnDestroy()
        {
            position.onPositionChange -= UpdateFieldOfView;
        }

        private void UpdateFieldOfView()
        {
            HidePreviousNodes();

            visibleNodes = Pathfinding.instance.GetNodesInRange_Square(position.x, position.y, sightDistance);

            for (int i = 0; i < visibleNodes.Count; i++)
            {
                visibleNodes[i].IsVisible = true;
                FogOfWar.RevealTile(visibleNodes[i].WorldPosition);
            }
        }

        private void HidePreviousNodes()
        {
            if (visibleNodes == null) return;
            for (int i = 0; i < visibleNodes.Count; i++)
            {
                visibleNodes[i].IsVisible = false;
                FogOfWar.HideTile(visibleNodes[i].WorldPosition);
            }
        }
    }
}