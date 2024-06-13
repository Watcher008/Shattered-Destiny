using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Background")]
    public class PlayerBackgrounds : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Vector2Int _startingCoords;

        [Header("Equipment")]
        [SerializeField] private string[] _itemIds;
        [SerializeField] private int[] _itemCounts;

        public string Name => _name;
        public Vector2Int StartingCoords => _startingCoords;
        public string[] ItemIds => _itemIds;
        public int[] ItemCounts => _itemCounts;
    }
}