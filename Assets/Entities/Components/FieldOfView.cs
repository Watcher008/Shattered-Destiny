using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class FieldOfView : ComponentBase
    {
        [SerializeField] private int sightDistance = 5;

        private Position position;

        private List<WorldNode> visibleNodes;

        public override void Register(Entity entity)
        {
            base.Register(entity);

            position = entity.GetComponentBase<Position>();
            position.onPositionChange += UpdateFieldOfView;

            UpdateFieldOfView();
        }

        public override void Unregister()
        {
            position.onPositionChange -= UpdateFieldOfView;
            base.Unregister();
        }

        private void UpdateFieldOfView()
        {
            HidePreviousNodes();

            visibleNodes = Pathfinding.instance.GetNodesInRange_Square(position.x, position.y, sightDistance);

            for (int i = 0; i < visibleNodes.Count; i++)
            {
                visibleNodes[i].IsVisible = true;
                FogOfWar.RevealTile(visibleNodes[i].worldPosition);
            }
        }

        private void HidePreviousNodes()
        {
            if (visibleNodes == null) return;
            for (int i = 0; i < visibleNodes.Count; i++)
            {
                visibleNodes[i].IsVisible = false;
                FogOfWar.HideTile(visibleNodes[i].worldPosition);
            }
        }
    }
}