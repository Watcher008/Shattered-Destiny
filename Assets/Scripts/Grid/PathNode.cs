namespace SD.Grids
{
    public class PathNode : IHeapItem<PathNode>
    {
        public Grid<PathNode> grid { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        #region - A* -
        //the movement cost to move from the start node to this node, following the existing path
        public int gCost;
        //the estimated movement cost to move from this node to the end node
        public int hCost; 
        //the current best guess as to the cost of the path
        public int fCost => gCost + hCost;

        public PathNode cameFromNode;
        #endregion

        #region - Heap -
        private int heapIndex;
        public int HeapIndex
        {
            get => heapIndex;
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(PathNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0) compare = hCost.CompareTo(other.hCost);
            return -compare; //want to return 1 if it's lower, so return negative value
        }
        #endregion

        public Occupant Occupant { get; private set; } // the creature/object occupying this node
        public bool IsWalkable { get; private set; } = true; //if this node can be traversed at all
        public int MovementCost { get; private set; } // Additional cost for moving to this node

        public TerrainType Terrain { get; private set; }

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            X = x;
            Y = y;
        }

        public void SetOccupant(Occupant occupant)
        {
            Occupant = occupant;
            grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
            grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetTerrain(TerrainType terrain)
        {
            Terrain = terrain;
            grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetMovementCost(int cost)
        {
            MovementCost = cost;
        }
    }

    public enum Occupant
    {
        None,
        Object,
        Player,
        Enemy,
    }
}