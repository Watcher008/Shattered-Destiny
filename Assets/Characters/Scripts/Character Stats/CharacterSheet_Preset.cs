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
        [SerializeField] private AttributeReference[] _attributes = new AttributeReference[5];

        [Header("Skills")]
        [SerializeField] private SkillReference[] _skills = new SkillReference[13];


        private void OnValidate()
        {
            for (int i = 0; i < _attributes.Length; i++)
            {
                if (_attributes[i].attribute.ID != i) Debug.LogError("Attributes in Incorrect Order. Index Error at " + i);
            }

            for (int i = 0; i < _skills.Length; i++)
            {
                if (_skills[i].skill.ID != i) Debug.LogError("Skills in Incorrect Order. Index Error at " + i);
            }
        }
    }
}