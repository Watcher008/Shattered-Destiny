using UnityEngine;

namespace SD.Grids
{
    /// <summary>
    /// A class to hold the grid for the world map.
    /// </summary>
    public static class WorldMap
    {
        private static Grid<PathNode> _grid;
        public static Grid<PathNode> Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;
            }
        }

        public static PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }

        public static PathNode GetNode(Vector3 worldPosition)
        {
            return _grid.GetGridObject(worldPosition);
        }

        public static Vector3 GetNodePosition(int x, int y)
        {
            return _grid.GetNodePosition(x, y);
        }
    }
}

