using System.Collections.Generic;
using System.Diagnostics;

namespace SD.CharacterSystem
{
    public class StatBase
    {
        public int BaseValue { get; protected set; } = 10;
        public int Value => GetModifiedValue();

        public int XP { get; protected set; }
        public int XPToNextLevel { get; protected set; }

        protected List<int> modifiers;

        /// <summary>
        /// Returns the BaseValue of the stat with all modifiers added.
        /// </summary>
        private int GetModifiedValue()
        {
            int modifiedValue = BaseValue;
            modifiers.ForEach(x => modifiedValue += x);
            return modifiedValue;
        }

        /// <summary>
        /// Adds a temporary modifier to the stat.
        /// </summary>
        public void AddModifier(int value)
        {
            if (value == 0) return;
            modifiers.Add(value);
        }

        /// <summary>
        /// Removes an existing modifier from the stat.
        /// </summary>
        public void RemoveModifier(int value)
        {
            if (value == 0) return;
            modifiers.Remove(value);
        }

        public void OnGainXP(int xp)
        {
            if (BaseValue >= 100) return;

            XP += xp;

            while (XP >= XPToNextLevel)
            {
                UnityEngine.Debug.Log("OnGainXP");
                OnLevelUp();
            }
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