using UnityEngine;

namespace SD.Pathfinding
{
    public class PathNode
    {
        private Grid<PathNode> grid;
        public int x { get; private set; }
        public int y { get; private set; }

        public int gCost; //the movement cost to move from the start node to this node, following the existing path
        public int hCost; //the estimated movement cost to move from this node to the end node
        public int fCost; //the current best guess as to the cost of the path
        public PathNode cameFromNode;

        public bool isOccupied { get; private set; } = false; //true if there is another creature occupying the node
        public bool isWalkable { get; private set; } = true; //if this node can be traversed at all
        public int movementCost { get; private set; } = 1; //cost to move into this tile
        public TerrainType terrain { get; private set; }

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            isOccupied = false;
            isWalkable = true;
            movementCost = 1;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
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
            movementCost = Mathf.Clamp(cost, 0, int.MaxValue);
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetTerrain(TerrainType terrain)
        {
            this.terrain = terrain;
            movementCost = Mathf.Clamp(terrain.MovementPenalty, 0, int.MaxValue);
            grid.TriggerGridObjectChanged(x, y);
        }
    }
}