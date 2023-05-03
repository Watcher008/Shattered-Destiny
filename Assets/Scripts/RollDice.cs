using UnityEngine;

public static class RollDice
{
    public static int Roll(DiceCombo combo)
    {
        int value = combo.baseValue;

        int rolledValue = 0;
        for (int i = 0; i < combo.diceCount; i++)
        {
            rolledValue += Random.Range(1, combo.diceValue);
        }

        if (combo.add) value += rolledValue;
        else value -= rolledValue;

        return value;
    }

    public static int Roll(int value)
    {
        return Random.Range(1, value);
    }
}
