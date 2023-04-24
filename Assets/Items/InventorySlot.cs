namespace SD.InventorySystem
{
    public class InventorySlot
    {
        public ItemBase item;
        public int count;

        public InventorySlot(ItemBase item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}