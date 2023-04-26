using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Character Preset")]
    public class CharacterPreset : ScriptableObject
    {
        [SerializeField] private string Name;
        [SerializeField] private Professions Profession;

        public Character GetCharacter()
        {
            return new Character(Name, Profession);
        }
    }
}
