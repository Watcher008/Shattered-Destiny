using UnityEngine;
using UnityEngine.Tilemaps;
using SD.Grids;
using System.Runtime.CompilerServices;

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

    private void InitializeGrid()
    {
        if (WorldMap.Grid != null) return;
        Debug.LogWarning("Creating world map grid");

        var tilemap = GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();

        var bounds = tilemap.cellBounds;

        var width = Mathf.Abs(bounds.xMin) + Mathf.Abs(bounds.xMax);
        var height = Mathf.Abs(bounds.yMin) + Mathf.Abs(bounds.yMax);

        Vector2 offset = new Vector2(bounds.xMin, bounds.yMin);

        //var origin = offset;
        //origin.x -= width / 2f * CELL_SIZE;
        //origin.y -= height / 2f * CELL_SIZE;

        var grid = new Grid<PathNode>(width, height, CELL_SIZE, offset,
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        WorldMap.Grid = grid;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                var tile = tilemap.GetTile(pos);
                var node = grid.GetGridObject(x, y);

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
