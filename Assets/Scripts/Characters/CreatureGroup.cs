using System.Collections.Generic;
using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Combat/Group", fileName = "New Group")]
    public class CreatureGroup : ScriptableObject
    {
        [System.Serializable]
        public class UnitSize
        {
            public StatBlock Unit;
            public int Min = 1;
            public int Max = 1;
        }

        [SerializeField] private TerrainType[] _terrain;
        [SerializeField] private UnitSize[] _units;

        public TerrainType[] Terrain => _terrain;

        /// <summary>
        /// Returns a random list of units with counts within listed ranges.
        /// </summary>
        /// <returns></returns>
        public List<StatBlock> GetUnits()
        {
            var list = new List<StatBlock>();

            foreach (var unit in _units)
            {
                int count = Random.Range(unit.Min, unit.Max + 1);
                for (int i = 0; i < count; i++)
                {
                    list.Add(unit.Unit);
                }
            }

            return list;
        }
    }
}