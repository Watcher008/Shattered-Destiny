using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Skill")]
    public class SkillSO : StatSO
    {
        [Header("Skill Properties")]
        [SerializeField] private SkillType skillType;
        public SkillType Type => skillType;
    }
}

public enum SkillType { Physical, Social, Survival }