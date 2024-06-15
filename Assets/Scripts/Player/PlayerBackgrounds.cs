using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Background")]
    public class PlayerBackgrounds : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Vector2Int _startingCoords;

        [Header("Reputation/Influence")]
        [Tooltip("Conglomerate, Imperium, Kingdom, Tribes")]
        [SerializeField] private int[] _factionInfluences = new int[4];
        [Tooltip("Conglomerate, Imperium, Kingdom, Tribes")]
        [SerializeField] private int[] _factionReputation = new int[4];

        [Header("Equipment")]
        [SerializeField] private string[] _itemIds;
        [SerializeField] private int[] _itemCounts;

        // Public variables
        public string Name => _name;
        public Vector2Int StartingCoords => _startingCoords;

        // Reputation and Influence
        public int[] FactionInfluence => _factionInfluences;
        public int[] FactionReputation => _factionReputation;

        // Equipment
        public string[] ItemIds => _itemIds;
        public int[] ItemCounts => _itemCounts;
    }
}