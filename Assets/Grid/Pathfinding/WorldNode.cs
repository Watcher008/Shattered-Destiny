using System.Collections.Generic;
using UnityEngine;
using SD.LocationSystem;
using SD.ECS;

namespace SD.PathingSystem
{
    public class WorldNode
    {
        public delegate void OnNodeEnteredCallback(Entity newEntity);
        public OnNodeEnteredCallback onNodeEntered;

        private Grid<WorldNode> grid;
        public int x { get; private set; }
        public int y { get; private set; }
        public Vector2 worldPosition { get; private set; }

        public int gCost; //the movement cost to move from the start node to this node, following the existing path
        public int hCost; //the estimated movement cost to move from this node to the end node
        public int fCost //the current best guess as to the cost of the path
        {
            get
            {
                return gCost + hCost;
            }
        }
        public WorldNode cameFromNode;

        public bool isOccupied { get; private set; } = false; //true if there is another creature occupying the node
        public bool isWalkable { get; private set; } = true; //if this node can be traversed at all
        public int movementCost { get; private set; } = 1; //cost to move into this tile

        public bool isExplored = false;

        private bool isVisible = false;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                SetVisibility(value);
            }
        }

        private List<Entity> occupants;

        public TerrainType terrain { get; private set; }
        public LocationData location { get; private set; }

        public WorldNode(Grid<WorldNode> grid, int x, int y, Vector2 pos)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            worldPosition = pos;

            isOccupied = false;
            isWalkable = true;
            movementCost = 1;

            occupants = new List<Entity>();
        }

        public void SetOccupied(bool isOccupied)
        {
            this.isOccupied = isOccupied;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetMoveCost(int cost)
        {
            movementCost = Mathf.Clamp(cost, 1, int.MaxValue);
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetTerrain(TerrainType terrain)
        {
            this.terrain = terrain;
            movementCost = Mathf.Clamp(1 + terrain.MovementPenalty, 1, int.MaxValue);
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetLocation(LocationData location)
        {
            this.location = location;
        }

        public void OccupyNode(Entity entity)
        {
            if (!occupants.Contains(entity)) occupants.Add(entity);
            if (entity.TryGetComponent(out EntityRenderer sprite))
            {
                sprite.ToggleRenderer(isVisible);
            }
            onNodeEntered?.Invoke(entity);
        }

        public void LeaveNode(Entity entity)
        {
            occupants.Remove(entity);
        }

        private void SetVisibility(bool value)
        {
            isVisible = value;

            for (int i = 0; i < occupants.Count; i++)
            {
                if (occupants[i].TryGetComponent(out EntityRenderer sprite))
                {
                    sprite.ToggleRenderer(isVisible);
                }
            }
        }
    }
}