namespace SD.Grids
{
    public class PathNode
    {
        private readonly Grid<PathNode> _grid;
        public int X { get; private set; }
        public int Y { get; private set; }

        //the movement cost to move from the start node to this node, following the existing path
        public int gCost;
        //the estimated movement cost to move from this node to the end node
        public int hCost; 
        //the current best guess as to the cost of the path
        public int fCost => gCost + hCost;

        public PathNode cameFromNode;

        public Occupant Occupant { get; private set; } // the creature/object occupying this node
        public bool IsWalkable { get; private set; } = true; //if this node can be traversed at all
        public float MovementModifier { get; private set; } = 0.0f;

        public TerrainType Terrain { get; private set; }

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            _grid = grid;
            X = x;
            Y = y;
        }

        public void SetOccupant(Occupant occupant)
        {
            Occupant = occupant;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetTerrain(TerrainType terrain)
        {
            Terrain = terrain;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public void SetMovementCost(float cost)
        {
            MovementModifier = cost;
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