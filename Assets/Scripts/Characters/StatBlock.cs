namespace SD.Characters
{
    /// <summary>
    /// A class to handle static character stats.
    /// These values will be copied over to the Combatant class during combat.s
    /// </summary>
    [System.Serializable]
    public class StatBlock
    {
        public string Name;
        public int[] Attributes;
        public int MaxHealth;
        public int Movement;
        public int Initiative;

        public int MaxActionPoints;
        public int StartingActionPoints;
        public int RefreshActionPoints;

        public string DefaultWeapon;
    }
}