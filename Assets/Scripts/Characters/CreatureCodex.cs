using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Creature Codex")]
    public class CreatureCodex : ScriptableObject
    {
        [System.Serializable]
        private class Codex 
        { 
            public StatBlock[] Creatures;
            public Squad[] Squads;
        }

        [System.Serializable]
        private class  Squad
        {
            public string Name;
            public string[] Terrain;
            public string[] Units;
        }

        private Dictionary<string, StatBlock> _creatures;
        private Dictionary<string, Squad> _squads;

        public void Init()
        {
            _creatures = new Dictionary<string, StatBlock>();
            _squads = new Dictionary<string, Squad>();

            var textAsset = Resources.Load("Codices/CreatureCodex") as TextAsset;
            StringReader reader = new StringReader(textAsset.text);
            string json = reader.ReadToEnd();

            Codex codex = JsonUtility.FromJson<Codex>(json);

            foreach(var entry in codex.Creatures)
            {
                _creatures.Add(entry.Name, entry);
            }

            foreach(var entry in codex.Squads)
            {
                _squads.Add(entry.Name, entry);
            }
        }

        public string[] GetSquad(string terrain)
        {
            var list = new List<Squad>();
            foreach(var squad in _squads)
            {
                foreach(var location in squad.Value.Terrain)
                {
                    if (location == terrain)
                    {
                        list.Add(squad.Value);
                        Debug.Log("Adding " + squad.Value.Name);
                        break;
                    }
                }
            }
            return list[Random.Range(0, list.Count)].Units;
        }

        public StatBlock GetCreatureByName(string name)
        {
            if (_creatures.ContainsKey(name)) return _creatures[name];
            return null;
        }
    }
}