using UnityEngine;
using UnityEngine.Tilemaps;

namespace SD.PathingSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Tilemap[] tileMaps;
        [SerializeField] private TerrainType[] terrainTypes;

        private Pathfinding pathfinding;
        private Vector3 offset = Vector2.zero; //the offset between the tilemap and the 
        private void Awake() => CreateNodeGrid();

        private void Start() => SetTerrainMaps();

        private void CreateNodeGrid()
        {
            tileMaps[0].CompressBounds();
            var bounds = tileMaps[0].cellBounds;

            var width = bounds.xMax - bounds.xMin;
            var height = bounds.yMax - bounds.yMin;
            Debug.Log(width + ", " + height);
            var cellSize = tileMaps[0].layoutGrid.cellSize.x;

            offset = new Vector3(cellSize / 2, cellSize / 2);

            pathfinding = new Pathfinding(width, height, cellSize);
        }

        private void SetTerrainMaps()
        {
            for (int i = 0; i < tileMaps.Length; i++)
            {
                if (!tileMaps[i].isActiveAndEnabled) continue;

                tileMaps[i].CompressBounds();
                foreach (var pos in tileMaps[i].cellBounds.allPositionsWithin)
                {
                    Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                    Vector3 place = tileMaps[i].CellToWorld(localPlace);
                    if (!tileMaps[i].HasTile(localPlace)) continue;

                    var modifiedPlace = place + offset;
                    if (CompareTile(tileMaps[i].GetTile(localPlace), out TerrainType type))
                    {
                        pathfinding.SetNodeTerrain(modifiedPlace, type);
                    }
                }
            }
        }

        private bool CompareTile(TileBase tile, out TerrainType type)
        {
            type = null;
            for (int i = 0; i < terrainTypes.Length; i++)
            {
                if (tile == terrainTypes[i].Tile)
                {
                    type = terrainTypes[i];
                    return true;
                }
            }
            Debug.LogWarning("type not found for " + tile.name);
            return false;
        }

        private void OnValidate()
        {
            var grid = tileMaps[0].layoutGrid;
            transform.position = Vector3.zero - new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2);
        }
    }
}