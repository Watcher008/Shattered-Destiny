using System.Collections.Generic;
using UnityEngine;
using SD.LocationSystem;
using SD.ECS;

namespace SD.PathingSystem
{
    public class PathNode
    {
        public delegate void OnNodeEnteredCallback(Entity newEntity);
        public OnNodeEnteredCallback onNodeEntered;

        private readonly Grid<PathNode> _grid;
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector2 WorldPosition { get; private set; }

        //the movement cost to move from the start node to this node, following the existing path
        public int gCost;
        //the estimated movement cost to move from this node to the end node
        public int hCost; 
        //the current best guess as to the cost of the path
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public PathNode cameFromNode;

        public bool isOccupied { get; private set; } = false; //true if there is another creature occupying the node
        public bool isWalkable { get; private set; } = true; //if this node can be traversed at all
        public float MovementModifier
        {
            get
            {
                if (Terrain) return Terrain.MovementModifier;
                return 1.0f;
            }
        }

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

        private readonly List<Entity> occupants;

        public TerrainType Terrain { get; private set; }
        public LocationData Location { get; private set; }

        public PathNode(Grid<PathNode> grid, int x, int y, Vector2 pos)
        {
            _grid = grid;
            X = x;
            Y = y;
            WorldPosition = pos;

            isOccupied = false;
            isWalkable = true;

            occupants = new List<Entity>();
        }

        public void SetOccupied(bool isOccupied)
        {
            this.isOccupied = isOccupied;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetTerrain(TerrainType terrain)
        {
            this.Terrain = terrain;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetLocation(LocationData location)
        {
            this.Location = location;
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