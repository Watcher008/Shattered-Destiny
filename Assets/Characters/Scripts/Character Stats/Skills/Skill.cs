using System.Collections.Generic;

namespace SD.CharacterSystem
{
    public class Skill : StatBase
    {
        public Skill(int baseValue)
        {
            BaseValue = baseValue;
            CalculateXPToNextLevel();
            modifiers = new List<int>();
        }
    }
}