using UnityEngine;

namespace SD.InventorySystem
{
    public abstract class ItemBase : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }

        [field: SerializeField] public string ItemName { get; private set; } = "New Item";

        [field: SerializeField] public int MaxStackCount { get; private set; } = 1;

        void SetID(int id, string name, int stackCount)
        {
            ID = id;
            ItemName = name;
            MaxStackCount = stackCount;
        }
    }
}