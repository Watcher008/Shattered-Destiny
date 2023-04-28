using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    private static FogOfWar instance;

    [SerializeField] private Tilemap fogOfWarMap;
    [SerializeField] private TileBase fogTile;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        fogOfWarMap.CompressBounds();
    }

    public static void HideTile(Vector3 worldPosition)
    {
        if (!instance.gameObject.activeSelf) return;

        var pos = instance.fogOfWarMap.WorldToCell(worldPosition);
        instance.fogOfWarMap.SetTile(pos, instance.fogTile);
    }

    public static void RevealTile(Vector3 worldPosition)
    {
        var pos = instance.fogOfWarMap.WorldToCell(worldPosition);
        instance.fogOfWarMap.SetTile(pos, null);
    }
}
