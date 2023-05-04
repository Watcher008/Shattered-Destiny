using UnityEngine;

public static class Dice
{
    public static int Roll(DiceCombo combo)
    {
        return Roll(combo.diceCount, combo.diceSides, combo.modifier);
    }

    public static int Roll(int count, int sides, int modifier)
    {
        int rolledValue = 0;
        for (int i = 0; i < count; i++)
        {
            rolledValue += Random.Range(1, sides);
        }
        rolledValue += modifier;
        return rolledValue;
    }

    public static int Roll(int value)
    {
        return Random.Range(1, value);
    }
}