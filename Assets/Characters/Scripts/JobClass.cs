using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Job Class")]
    public class JobClass : ScriptableObject
    {
        [field: SerializeField] public string ClassName { get; private set; }
        [field: SerializeField] public AttributeReference[] ClassAttributes { get; private set; }
        [field: SerializeField] public SkillReference[] ClassSkills { get; private set; }
    }
}