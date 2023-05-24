using System.Collections.Generic;

namespace SD.CharacterSystem
{
    public class Attribute : StatBase
    {
        public Attribute(int baseValue)
        {
            BaseValue = baseValue;
            CalculateXPToNextLevel();
            modifiers = new List<int>();
        }
    }
}
