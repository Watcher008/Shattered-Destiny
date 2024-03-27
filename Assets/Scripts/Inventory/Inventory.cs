using System.Collections.Generic;
using UnityEngine;
using SD.Grids;

namespace SD.Inventories 
{
    public class Inventory
    {
        public delegate void OnContentsChanged();
        public OnContentsChanged onContentsChanged;

        private Vector2Int _dimensions;
        private Grid<InventoryNode> _grid;

        public Vector2Int Dimensions => _dimensions;
        public Grid<InventoryNode> Grid => _grid;

        public Inventory(Vector2Int dimensions)
        {
            _dimensions = dimensions;
            _grid = new Grid<InventoryNode>(_dimensions.x, _dimensions.y, 75f, Vector3.zero,
                (Grid<InventoryNode> g, int x, int y) => new InventoryNode(g, x, y));
        }

        public bool IsValidPosition(Vector2Int position)
        {
            if (position.x < 0) return false;
            if (position.x >= _dimensions.x) return false;
            if (position.y < 0) return false;
            if (position.y >= _dimensions.y) return false;
            return true;
        }

        public bool CanPlaceItem(InventoryItem item, Vector2Int origin)
        {
            List<Vector2Int> gridPositions = item.GetGridPositionList(origin);

            foreach(var pos in gridPositions)
            {
                // Outside bounds of grid
                if (!IsValidPosition(pos))
                {
                    //Debug.Log("Position is not valid!");
                    return false;
                }
                // Something else is placed here
                if (!_grid.GetGridObject(pos.x, pos.y).CanPlaceItem())
                {
                    //Debug.Log("Position is occupied!");
                    return false;
                }
            }

            return true;
        }

        public bool TryPlaceItem(InventoryItem item, Vector2Int origin)
        {
            //Debug.Log("Trying to place item at " + origin);
            if (!CanPlaceItem(item, origin)) return false;
            //Debug.Log("Placing item at " + origin);
            item.Origin = origin;

            List<Vector2Int> gridPositions = item.GetGridPositionList(origin);
            foreach (var pos in gridPositions)
            {
                _grid.GetGridObject(pos.x, pos.y).item = item;
            }

            onContentsChanged?.Invoke();
            return true;
        }

        public void RemoveItemAt(Vector2Int origin, bool IsRotated, bool detachObject = true)
        {
            var item = _grid.GetGridObject(origin.x, origin.y).item;
            if (item == null) return;

            List<Vector2Int> gridPositions = item.GetGridPositionList(origin);
            foreach(var pos in gridPositions)
            {
                _grid.GetGridObject(pos.x, pos.y).item = null;
            }

            onContentsChanged?.Invoke();
        }

        public bool TryFitItem(InventoryItem item)
        {
            // Go through height backwards to always try to place top left first
            // Also try to check all positions without rotationg first
            for (int y = _grid.GetHeight() - 1; y >= 0; y--)
            {
                for (int x = 0; x < _grid.GetWidth(); x++)
                {
                    var origin = new Vector2Int(x, y);
                    if (TryPlaceItem(item, origin)) return true;
                }
            }

            for (int y = _grid.GetHeight() - 1; y >= 0; y--)
            {
                for (int x = 0; x < _grid.GetWidth(); x++)
                {
                    var origin = new Vector2Int(x, y);
                    if (TryPlaceItem(item, origin)) return true;
                }
            }

            return false;
        }
    }
}