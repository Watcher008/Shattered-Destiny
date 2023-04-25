using UnityEngine;

namespace SD.PathingSystem
{
    [CreateAssetMenu(fileName = "New Terrain", menuName = "Scriptable Objects/Terrain Type")]
    public class TerrainType : ScriptableObject
    {
        [field: SerializeField] public RuleTile Tile { get; private set; }



        [field: SerializeField] public int MovementPenalty { get; private set; }
        [field: SerializeField] public bool CanTravelOnFoot { get; private set; } = true;
    }
}