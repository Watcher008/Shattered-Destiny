namespace SD.CharacterSystem
{
    public static class SkillCheck
    {
        public static bool OnSkillCheck(int skillValue, int targetValue)
        {
            return skillValue >= targetValue;
        }
    }
}