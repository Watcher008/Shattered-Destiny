using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item Item;
    public Vector2Int Origin;
    public Vector2Int Size;
    public bool IsRotated;

    public InventoryItem(Item weapon, Vector2Int origin, Vector2Int size, bool isRotated = false)
    {
        Item = weapon;
        Origin = origin;
        Size = size;
        IsRotated = isRotated;
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
