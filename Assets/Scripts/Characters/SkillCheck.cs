using UnityEngine;

namespace SD.Characters
{
    public static class SkillCheck
    {
        public static int[] scoreToPass = { 7, 8, 9, 13, 17 };
        public static int[] xpOnSuccess = { 50, 75, 100, 125, 150 };
        public static int[] xpOnFail = { 300, 250, 200, 175, 150 };

        public static bool RollSkillCheck(CharacterSheet player, Attributes attribute, CheckDifficulty difficulty)
        {
            int roll = Random.Range(2, 12);
            int bonus = player.GetAttributeBonus(attribute);

            bool success = roll + bonus >= scoreToPass[(int)difficulty];

            if (success) player.GainXP(attribute, xpOnSuccess[(int)difficulty]);
            else player.GainXP(attribute, xpOnFail[(int)difficulty]);

            return success;
        }
    }

    public enum CheckDifficulty
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard,
    }
}