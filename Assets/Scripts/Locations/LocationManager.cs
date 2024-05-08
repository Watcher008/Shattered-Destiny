using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SD.Grids;
using SD.EventSystem;

public class LocationManager : MonoBehaviour
{
    [SerializeField] private Tilemap _locationMap;
    [SerializeField] private GameEvent _enterTownEvent;
    private Dictionary<PathNode, LocationType> _locations;

    private void Start()
    {
        _locations = new Dictionary<PathNode, LocationType>();
        _locationMap.CompressBounds();

        var bounds = _locationMap.cellBounds;
        var width = Mathf.Abs(bounds.xMin) + Mathf.Abs(bounds.xMax);
        var height = Mathf.Abs(bounds.yMin) + Mathf.Abs(bounds.yMax);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);

                var tile = _locationMap.GetTile(pos) as LocationTile;
                if (tile == null) continue;

                var node = WorldMap.GetNode(pos);
                _locations.Add(node, tile.Type);
            }
        }
    }

    public bool IsLocation(PathNode node)
    {
        if (_locations.ContainsKey(node))
        {
            switch (_locations[node])
            {
                case LocationType.Town:
                    Debug.Log("Town");
                    _enterTownEvent?.Invoke();
                    WorldMapManager.Instance.onPauseInput?.Invoke();
                    break;
                case LocationType.Cave:
                    Debug.Log("Cave");
                    break;
                case LocationType.Camp:
                    Debug.Log("Camp");
                    break;
            }
            return true;
        }
        return false;
    }

    
}
