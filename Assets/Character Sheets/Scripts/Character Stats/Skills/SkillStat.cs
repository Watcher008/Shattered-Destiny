using JetBrains.Annotations;
using System.Collections.Generic;

namespace SD.CharacterSystem
{
    public class SkillStat : StatBase
    {
        public Skill Type { get; private set; }
        public SkillStat(Skill type, int baseValue)
        {
            Type = type; 
            BaseValue = baseValue;
            CalculateXPToNextLevel();
            modifiers = new List<int>();
        }
    }
}