using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    public class StatBase
    {
        public int BaseValue { get; protected set; } = 10;
        public int XP { get; protected set; }
        public int XPToNextLevel { get; protected set; }

        public int Value => GetModifiedValue();

        protected List<int> modifiers;

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

        public void OnGainXP(int xp)
        {
            if (BaseValue >= 100) return;

            XP += xp;

            while (XP >= XPToNextLevel) OnLevelUp();
        }

        private void OnLevelUp()
        {
            if (BaseValue >= 100) return;
            BaseValue++;
            XP -= XPToNextLevel;
            CalculateXPToNextLevel();
        }

        protected void CalculateXPToNextLevel()
        {
            XPToNextLevel = BaseValue * 100;
        }
    }
}