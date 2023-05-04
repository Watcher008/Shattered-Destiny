using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    public class StatBase
    {
        public int BaseValue { get; private set; } = 10;
        public int Value => GetModifiedValue();

        protected List<int> modifiers;

        public StatBase()
        {
            modifiers = new List<int>();
        }

        public void SetBaseValue(int value)
        {
            BaseValue = value;
        }

        public void IncreaseBaseValue(int value)
        {
            BaseValue = Mathf.Clamp(BaseValue + value, 0, 100);
        }

        private int GetModifiedValue()
        {
            int modifiedValue = BaseValue;
            modifiers.ForEach(x => modifiedValue += x);
            return modifiedValue;
        }

        public void AddModifier(int value)
        {
            if (value == 0) return;
            modifiers.Add(value);
        }

        public void RemoveModifier(int value)
        {
            if (value == 0) return;
            modifiers.Remove(value);
        }
    }

    public class Attribute : StatBase
    {
        public Attribute() : base()
        {
            //modifiers = new List<int>();   
        }
    }

    public class Skill : StatBase
    {
        public Skill() : base()
        {
            //modifiers = new List<int>();
        }
    }

    [System.Serializable]
    public class AttributeReference
    {
        public Attributes attribute;
        public int value;
    }

    [System.Serializable]
    public class SkillReference
    {
        public Skills skill;
        public int value;
    }
}

public enum Attributes 
{ 
    Strength, 
    Dexterity, 
    Constitution, 
    Intelligence, 
    Wisdom, 
    Charisma,
}

public enum Skills
{
    Alchemy,
    Appraisal,
    Athletics,
    Barter,
    Carpentry,
    Cooking,
    Cunning,
    Detect_Traps,
    Diplomacy,
    Disarm_Traps,
    Engineering,
    Evasion,
    Herbalism,
    Literacy,
    Medicine,
    Perception,
    Smithing,
    Survival,
    Thievery,
}