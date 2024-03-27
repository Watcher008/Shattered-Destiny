using UnityEngine;
using SD.Grids;

namespace SD.Inventories
{
    public class InventoryNode
    {
        public Grid<InventoryNode> grid { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public InventoryItem item;
        public bool IsBlocked;

        public InventoryNode(Grid<InventoryNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public bool CanPlaceItem()
        {
            return item == null;
        }

        public bool HasItem()
        {
            return item != null;
        }

        public InventoryItem GetItem()
        {
            return item;
        }
    }
}