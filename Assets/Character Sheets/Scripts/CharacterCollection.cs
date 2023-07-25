using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Character Collection")]
    public class CharacterCollection : ScriptableObject
    {
        public delegate void OnCollectionChangeCallback();
        public OnCollectionChangeCallback onCollectionChanged;

        [SerializeField] private List<Character> characters = new List<Character>();
        public List<Character> Characters => characters;

        public void Add(Character character)
        {
            characters.Add(character);
            onCollectionChanged?.Invoke();
        }

        public void Remove(Character character)
        {
            characters.Remove(character);
            onCollectionChanged?.Invoke();
        }
    }
}