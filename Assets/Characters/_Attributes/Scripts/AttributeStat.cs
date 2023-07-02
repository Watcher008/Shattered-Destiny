using System.Collections.Generic;

namespace SD.CharacterSystem
{
    public class AttributeStat : StatBase
    {
        public Attribute Type { get; private set; }
        public AttributeStat(Attribute type, int baseValue)
        {
            Type = type;
            BaseValue = baseValue;
            CalculateXPToNextLevel();
            modifiers = new List<int>();
        }
    }
}
