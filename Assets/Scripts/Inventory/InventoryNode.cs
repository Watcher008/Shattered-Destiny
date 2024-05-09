using UnityEngine;
using SD.Grids;

namespace SD.Inventories
{
    public class InventoryNode
    {
        public Grid<InventoryNode> grid { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        private InventoryItem _item;
        public InventoryItem Item => _item;

        public InventoryNode(Grid<InventoryNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public bool CanPlaceItem(InventoryItem item)
        {
            if (_item == null) return true;
            return _item.CanStack(item, out _);
        }

        public bool HasItem()
        {
            return _item != null;
        }

        public InventoryItem GetItem()
        {
            return _item;
        }

        public void SetItem(InventoryItem item)
        {
            _item = item;
        }
    }
}