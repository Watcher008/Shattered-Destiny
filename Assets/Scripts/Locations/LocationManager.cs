using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SD.Grids;
using SD.EventSystem;

public class LocationManager : MonoBehaviour
{
    [SerializeField] private Tilemap _locationMap;
    [SerializeField] private GameEvent _enterTownEvent;
    private List<PathNode> _locations = new List<PathNode>();

    private void Start()
    {
        _locationMap.CompressBounds();

        var bounds = _locationMap.cellBounds;
        var width = Mathf.Abs(bounds.xMin) + Mathf.Abs(bounds.xMax);
        var height = Mathf.Abs(bounds.yMin) + Mathf.Abs(bounds.yMax);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);

                var tile = _locationMap.GetTile(pos);
                if (tile == null) continue;

                var node = Pathfinding.instance.GetNode(pos);
                _locations.Add(node);
            }
        }
    }

    public bool IsLocation(PathNode node)
    {
        if (_locations.Contains(node))
        {
            _enterTownEvent?.Invoke();
            WorldMapManager.Instance.onPauseInput?.Invoke();
            return true;
        }
        return false;
    }
}
