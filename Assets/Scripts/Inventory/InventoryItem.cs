using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item Item;
    public int Count;
    public Vector2Int Origin;
    public Vector2Int Size;
    public bool IsRotated;

    public InventoryItem(Item item, Vector2Int origin, int count = 1, bool isRotated = false)
    {
        Item = item;
        Count = count;
        Size = new Vector2Int(item.Width, item.Height);
        Origin = origin;
        IsRotated = isRotated;
    }

    /// <summary>
    /// If the two items can stack.
    /// </summary>
    /// <param name="remainder">The leftover count after stacking the two up to the max stack count.</param>
    /// <returns></returns>
    public bool CanStack(InventoryItem other, out int remainder)
    {
        remainder = 0;
        // Not the same item
        if (other.Item.Name != Item.Name) return false;
        if (Count >= Item.MaxStack) return false;

        remainder = Count + other.Count - Item.MaxStack;
        if (remainder < 0) remainder = 0;
        return true;
    }

    public void TryStack(InventoryItem other)
    {
        if (!CanStack(other, out int remainder)) return;

        int diff = Item.MaxStack - Count;

        int numToAdd = Mathf.Min(other.Count, diff);

        Count += numToAdd;
        other.Count -= numToAdd;
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int origin)
    {
        List<Vector2Int> gridPositions = new List<Vector2Int>();

        if (IsRotated)
        {
            for (int x = 0; x < Size.y; x++)
            {
                for (int y = 0; y < Size.x; y++)
                {
                    gridPositions.Add(origin + new Vector2Int(x, -y));
                }
            }
        }
        else
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    gridPositions.Add(origin + new Vector2Int(x, y));
                }
            }
        }

        return gridPositions;
    }
}
