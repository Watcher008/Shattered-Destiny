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

        public bool CanPlaceItem(InventoryItem item, Vector2Int gridPos)
        {
            // First check for stacking
            var node = _grid.GetGridObject(gridPos.x, gridPos.y);
            if (node == null) return false;

            if (node.Item != null && node.Item.CanStack(item, out _)) return true;

            // then check the rest
            List<Vector2Int> gridPositions = item.GetGridPositionList(gridPos);
            foreach(var pos in gridPositions)
            {
                // Outside bounds of grid
                if (!IsValidPosition(pos)) return false;
                // Something else is placed here
                if (!_grid.GetGridObject(pos.x, pos.y).CanPlaceItem(item)) return false;
            }

            return true;
        }

        public bool TryPlaceItem(InventoryItem item, Vector2Int gridPos)
        {
            if (!CanPlaceItem(item, gridPos)) return false;


            var node = _grid.GetGridObject(gridPos.x, gridPos.y);
            if (node.Item != null && node.Item.CanStack(item, out var remainder))
            {
                node.Item.TryStack(item);
                return item.Count == 0;
            }

            item.Origin = gridPos;

            List<Vector2Int> gridPositions = item.GetGridPositionList(gridPos);
            foreach (var pos in gridPositions)
            {
                _grid.GetGridObject(pos.x, pos.y).SetItem(item);
            }

            onContentsChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Returns true if the given positio is within the grid bounds.
        /// </summary>
        private bool IsValidPosition(Vector2Int position)
        {
            if (position.x < 0) return false;
            if (position.x >= _dimensions.x) return false;
            if (position.y < 0) return false;
            if (position.y >= _dimensions.y) return false;
            return true;
        }

        public void RemoveItemAt(Vector2Int origin, bool IsRotated, bool detachObject = true)
        {
            var item = _grid.GetGridObject(origin.x, origin.y).Item;
            if (item == null) return;

            List<Vector2Int> gridPositions = item.GetGridPositionList(origin);
            foreach(var pos in gridPositions)
            {
                _grid.GetGridObject(pos.x, pos.y).SetItem(null);
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

            // should I not be trying to rotate the item here?

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