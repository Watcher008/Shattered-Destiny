using System.Collections.Generic;

namespace SD.Characters
{
    public class Stat
    {
        private int _value;
        private List<int> _modifiers;

        private int _xp;
        private int _xpToNextlevel;

        public int Value => GetValue();

        public Stat(int value, int xp = 0)
        {
            _value = value;
            _xp = xp;
            _modifiers = new List<int>();
            CalculateXPToNextLevel();
        }

        /// <summary>
        /// Returns the modified value of the stat.
        /// </summary>
        private int GetValue()
        {
            int value = _value;
            _modifiers.ForEach(x => value += x);
            return value;
        }

        /// <summary>
        /// Adds a modifier to the stat.
        /// </summary>
        public void AddModifier(int value)
        {
            if (value == 0) return;
            _modifiers.Add(value);
        }

        /// <summary>
        /// Removes a modifier to the stat.
        /// </summary>
        public void RemoveModifier(int value)
        {
            if (_value == 0) return;
            if (!_modifiers.Contains(value)) return;

            _modifiers.Remove(value);
        }

        public void GainXP(int xp)
        {
            if (_value >= 100) return;

            _xp += xp;

            while(_xp >= _xpToNextlevel)
            {
                _xp -= _xpToNextlevel;
                OnLevelUp();
            }
        }

        private void OnLevelUp()
        {
            _value++;

            if (_value >= 100) _xpToNextlevel = 0;
            else CalculateXPToNextLevel();
        }

        /// <summary>
        /// Calculates the XP needed to reach the next level.
        /// </summary>
        private void CalculateXPToNextLevel()
        {
            _xpToNextlevel = 2000 + (_value - 1) * ((32000 - 2000) / (99 - 1));
            //_xpToNextlevel = 2000 + (_value - 1) * 30.6;
        }
    }
}