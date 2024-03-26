namespace SD.Characters
{
    public class Stat
    {
        private int _value;
        private int _xp;
        private int _xpToNextlevel;

        public int Value => _value;

        public Stat(int value, int xp = 0)
        {
            _value = value;
            _xp = xp;

            CalculateXPToNextLevel();
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

        private void CalculateXPToNextLevel()
        {
            _xpToNextlevel = 2000 + (_value - 1) * ((32000 - 2000) / (99 - 1));
            //_xpToNextlevel = 2000 + (_value - 1) * 30.6;
        }
    }
}