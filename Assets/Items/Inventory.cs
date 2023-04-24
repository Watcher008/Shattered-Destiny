using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.InventorySystem
{
    public class Inventory
    {
        public List<InventorySlot> items;

        public Inventory()
        {
            items = new List<InventorySlot>();
        }

        public void AddItem(ItemBase newItem, int count = 1)
        {
            while (count > 0)
            {
                var slot = FindSlot(newItem, true);
                if (slot != null)
                {
                    //limit amount added to this slot by the remaining room left in the stack, and the remaining count
                    int amountToAdd = Mathf.Clamp(slot.item.MaxStackCount - slot.count, 0, count);

                    count -= amountToAdd;
                    slot.count += amountToAdd;
                }
                else
                {
                    var newCount = Mathf.Clamp(count, 0, newItem.MaxStackCount);

                    var newSlot = new InventorySlot(newItem, newCount);
                    items.Add(newSlot);

                    count -= newCount;
                }
            }
        }

        public void RemoveItem(ItemBase oldItem, int count)
        {
            var slot = FindSlot(oldItem);

            while (count > 0 && slot != null)
            {
                int countToRemove = Mathf.Clamp(count, 0, slot.count);

                count -= countToRemove;
                slot.count -= countToRemove;

                if (slot.count <= 0) items.Remove(slot);

                slot = FindSlot(oldItem);
            }
        }


        private InventorySlot FindSlot(ItemBase item, bool isOpen = false)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == item)
                {
                    //Only return a slot that can be added to
                    if (isOpen && items[i].count >= items[i].item.MaxStackCount) continue;

                    return items[i];
                }
            }
            return null;
        }
    }
}