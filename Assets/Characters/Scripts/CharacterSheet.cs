using UnityEngine;

namespace SD.CharacterSystem
{
    public class CharacterSheet : MonoBehaviour
    {
        public string Name { get; private set; }
        public CharacterRace Race { get; private set; }
        public JobClass Class { get; private set; }

        private Attribute[] attributes;
        private Skill[] skills;

        public CharacterSheet(string name, CharacterRace race, JobClass job, int[] attributeScores, int[] skillScores)
        {
            Name = name;
            Race = race;
            Class = job;

            attributes = new Attribute[attributeScores.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].SetBaseValue(attributeScores[i]);
            }

            skills = new Skill[skillScores.Length];
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].SetBaseValue(skillScores[i]);
            }

            foreach(var bonus in Race.RacialAttributes)
            {
                attributes[(int)bonus.attribute].IncreaseBaseValue(bonus.value);
            }

            foreach (var bonus in Race.RacialSkills)
            {
                skills[(int)bonus.skill].IncreaseBaseValue(bonus.value);
            }

            foreach (var bonus in Class.ClassAttributes)
            {
                attributes[(int)bonus.attribute].IncreaseBaseValue(bonus.value);
            }

            foreach (var bonus in Class.ClassSkills)
            {
                skills[(int)bonus.skill].IncreaseBaseValue(bonus.value);
            }
        }

        public int GetAttributeValue(Attributes attribute)
        {
            return attributes[(int)attribute].Value;
        }

        public int GetSkillValue(Skills skill)
        {
            return skills[(int)skill].Value;
        }
    }
}

