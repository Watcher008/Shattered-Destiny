using UnityEngine;

namespace SD.Pathfinding
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private TerrainMap[] terrainMaps;
        
        private void Start()
        {
            SetTerrainMaps();
        }

        private void SetTerrainMaps()
        {
            for (int i = 0; i < terrainMaps.Length; i++)
            {
                terrainMaps[i].tileMap.CompressBounds();
                foreach (var pos in terrainMaps[i].tileMap.cellBounds.allPositionsWithin)
                {
                    Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                    Vector3 place = terrainMaps[i].tileMap.CellToWorld(localPlace);
                    if (terrainMaps[i].tileMap.HasTile(localPlace))
                    {
                        Pathfinding.instance.SetNodeTerrain(place, terrainMaps[i].terrain);
                    }
                }
            }
        }
    }
}