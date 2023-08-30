using UnityEngine;

namespace SD.PathingSystem
{
    [CreateAssetMenu(fileName = "New Terrain", menuName = "Scriptable Objects/Terrain Type")]
    public class TerrainType : ScriptableObject
    {
        [SerializeField] private RuleTile _tile;

        [SerializeField] private float _movementModifier = 1;

        [field: SerializeField] public int MovementPenalty { get; private set; }
        [field: SerializeField] public int ExhaustionCost { get; private set; }

        [SerializeField] private bool _canTravelOnFoot = true;

        public RuleTile Tile => _tile;
        public float MovementModifier => _movementModifier;

        public bool CanTravelOnFoot => _canTravelOnFoot;
    }
}