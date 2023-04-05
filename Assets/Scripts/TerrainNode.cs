using UnityEngine;

public class TerrainNode : MonoBehaviour
{
    public enum TerrainType { Land, Road, RoughTerrain, Mountain, Water }
    [field: SerializeField] public TerrainType terrainType { get; private set; }
    [field: SerializeField] public Territory territory { get; private set; }
}
public enum Territory { Land_1, Land_2, Land_3, Land_4 }