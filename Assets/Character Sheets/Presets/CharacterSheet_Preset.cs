using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Character Sheet")]
    public class CharacterSheet_Preset : ScriptableObject
    {
        [SerializeField] private string _name;

        [Space]

        [SerializeField] private CharacterRace _race;
        [SerializeField] private ClassArchetype _class;

        [Space]

        [Header("Attributes")]
        [SerializeField]
        private AttributeReference[] _attributes = new AttributeReference[6];

        public string Name => _name;
        public CharacterRace Race => _race;
        public ClassArchetype Class => _class;
        public AttributeReference[] Attributes => _attributes;

        private void OnValidate()
        {
            for (int i = 0; i < _attributes.Length; i++)
            {
                if (_attributes[i].attribute != (Attribute)i) _attributes[i].attribute = (Attribute)i;
            }
        }
    }
}