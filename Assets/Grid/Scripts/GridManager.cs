using UnityEngine;
using UnityEngine.Tilemaps;

namespace SD.PathingSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Tilemap featureOverlayMap;

        [SerializeField] private TerrainType defaultTerrain;
        [SerializeField] private TerrainType[] terrainTypes;

        private Pathfinding pathfinding;
        private Vector3 offset = Vector2.zero; //the offset between the tilemap and the 
        private void Awake() => CreateNodeGrid();

        private void Start() => SetTerrainMaps();

        private void CreateNodeGrid()
        {
            featureOverlayMap.CompressBounds();
            var bounds = featureOverlayMap.cellBounds;

            var width = bounds.xMax - bounds.xMin;
            var height = bounds.yMax - bounds.yMin;
            //Debug.Log(width + ", " + height);
            
            var cellSize = featureOverlayMap.layoutGrid.cellSize.x;

            offset = new Vector3(cellSize / 2, cellSize / 2);

            pathfinding = new Pathfinding(width, height, cellSize);
        }

        private void SetTerrainMaps()
        {
            featureOverlayMap.CompressBounds();
            foreach (var pos in featureOverlayMap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = featureOverlayMap.CellToWorld(localPlace);
                //if (!featureOverlayMap.HasTile(localPlace)) continue;

                var modifiedPlace = place + offset;
                pathfinding.SetNodeTerrain(modifiedPlace, GetTerrain(featureOverlayMap.GetTile(localPlace)));
            }
        }

        private TerrainType GetTerrain(TileBase tile)
        {
            for (int i = 0; i < terrainTypes.Length; i++)
            {
                if (tile == terrainTypes[i].Tile)
                {
                    return terrainTypes[i];
                }
            }
            return defaultTerrain;
        }

        private void OnValidate()
        {
            var grid = featureOverlayMap.layoutGrid;
            transform.position = Vector3.zero - new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2);
        }
    }
}