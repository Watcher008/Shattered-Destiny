using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Skill")]
    public class SkillSO : StatSO
    {
        [Header("Skill Properties")]
        [SerializeField] private SkillType skillType;

        [SerializeField] private AttributeSO[] associatedAttributes;

        public SkillType Type => skillType;
        public AttributeSO[] AssociatedAttributes => associatedAttributes;
    }
}

public enum SkillType { Physical, Social, Survival }