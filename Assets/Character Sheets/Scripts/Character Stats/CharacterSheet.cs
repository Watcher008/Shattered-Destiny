using UnityEngine;

namespace SD.CharacterSystem
{
    public class CharacterSheet
    {
        public string Name { get; private set; }
        public CharacterRace Race { get; private set; }
        public ClassArchetype Class { get; private set; }

        public AttributeStat[] Attributes { get; private set; }

        public CharacterSheet(string name, CharacterRace race, ClassArchetype job, int[] attributeScores)
        {
            Name = name;
            Race = race;
            Class = job;

            //Attributes
            Attributes = new AttributeStat[attributeScores.Length];
            foreach (var bonus in Race.RacialAttributeBonuses)
            {
                attributeScores[(int)bonus.attribute] += bonus.value;
            }
            foreach (var bonus in Class.ClassAttributes)
            {
                attributeScores[(int)bonus.attribute] += bonus.value;
            }
            for (int i = 0; i < Attributes.Length; i++)
            {
                Attributes[i] = new AttributeStat((Attribute)i, attributeScores[i]);
            }
        }

        private float GetXPModifier(Attribute attribute)
        {
            var value = Attributes[(int)attribute].BaseValue;

            if (value < 10) return 0.7f;
            if (value < 20) return 0.8f;
            if (value < 30) return 1.0f;
            if (value < 40) return 1.1f;
            if (value < 50) return 1.2f;
            if (value < 60) return 1.25f;
            if (value < 70) return 1.3f;
            if (value < 80) return 1.4f;
            return 1.5f;
        }
    }
}

public enum Attribute
{
    Physicality,
    Intelligence,
    Social,
    Survival,
}