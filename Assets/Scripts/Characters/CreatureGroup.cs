using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Combat/Group", fileName = "New Group")]
    public class CreatureGroup : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private TerrainType[] _terrain;
        [SerializeField] private StatBlock[] _units;

        public string Id => _id;
        public TerrainType[] Terrain => _terrain;
        public StatBlock[] Units => _units;
    }
}