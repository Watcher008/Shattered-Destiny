using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Creature Codex")]
    public class CreatureCodex : ScriptableObject
    {
        [System.Serializable]
        private class Codex { public StatBlock[] Creatures; }

        private Dictionary<string, StatBlock> _creatures;

        public void Init()
        {
            _creatures = new Dictionary<string, StatBlock>();

            var textAsset = Resources.Load("Codices/CreatureCodex") as TextAsset;
            StringReader reader = new StringReader(textAsset.text);
            string json = reader.ReadToEnd();

            Codex codex = JsonUtility.FromJson<Codex>(json);

            foreach(var entry in codex.Creatures)
            {
                _creatures.Add(entry.Name, entry);
            }
        }

        public StatBlock GetCreatureByName(string name)
        {
            if (_creatures.ContainsKey(name)) return _creatures[name];
            return null;
        }
    }
}