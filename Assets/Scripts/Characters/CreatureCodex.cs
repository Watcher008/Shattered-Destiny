using System.Collections.Generic;
using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Creature Codex")]
    public class CreatureCodex : ScriptableObject
    {
        [SerializeField] private CreatureGroup[] _groups;

        public List<StatBlock> GetGroupByTerrain(TerrainType terrain)
        {
            var list = new List<CreatureGroup>();
            foreach(var group in _groups)
            {
                foreach(var ter in group.Terrain)
                {
                    if (ter == terrain)
                    {
                        list.Add(group);
                        break;
                    }
                }
            }
            return list[Random.Range(0, list.Count)].GetUnits();
        }
    }
}