using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Character Database")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<Character> characters = new List<Character>();

        public void SetList(List<Character> allCharacters)
        {
            characters = new List<Character>();
            characters.AddRange(allCharacters);
        }

        public void AddCharacter(Character character)
        {
            characters.Add(character);
        }
    }
}