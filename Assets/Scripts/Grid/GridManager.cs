using UnityEngine;
using UnityEngine.Tilemaps;
using SD.PathingSystem;

public class GridManager : MonoBehaviour
{
    private const int CELL_SIZE = 1;

    [SerializeField] private RuleTile[] _tiles;

    private int[] _terrainMovementPenalties =
    {
        5, // Grassland
        0, // Road
        10, // Forest
        25, // Mountain
        100 // Water
    };


    private void Awake() => InitializeGrid();

    private void OnDestroy() => Pathfinding.instance.Destroy();

    private void InitializeGrid()
    {
        var tilemap = GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();

        var bounds = tilemap.cellBounds;

        var width = Mathf.Abs(bounds.xMin) + Mathf.Abs(bounds.xMax);
        var height = Mathf.Abs(bounds.yMin) + Mathf.Abs(bounds.yMax);

        Vector2 origin = new Vector2(bounds.xMin, bounds.yMin);

        new Pathfinding(width, height, CELL_SIZE, origin);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                var tile = tilemap.GetTile(pos);
                var node = Pathfinding.instance.GetNode(x, y);

                if (tile != null)
                {
                    var terrain = GetTerrain(tile);
                    
                    node.SetTerrain(terrain);
                    node.SetMovementCost(_terrainMovementPenalties[(int)terrain]);
                    if (terrain == TerrainType.Water) node.SetWalkable(false);
                }
                else
                {
                    node.SetTerrain(TerrainType.Grassland);
                    node.SetMovementCost(_terrainMovementPenalties[(int)TerrainType.Grassland]);
                }
            }
        }
    }

    private TerrainType GetTerrain(TileBase tile)
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            if (tile == _tiles[i]) return (TerrainType)i;
        }
        return TerrainType.Grassland;
    }
}

public enum TerrainType
{
    Grassland,
    Road,
    Forest,
    Mountain,
    Water
}
