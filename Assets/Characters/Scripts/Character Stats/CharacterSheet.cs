namespace SD.CharacterSystem
{
    public class CharacterSheet
    {
        public string Name { get; private set; }
        public CharacterRace Race { get; private set; }
        public ClassArchetype Class { get; private set; }

        public Attribute[] attributes { get; private set; }
        public Skill[] skills { get; private set; }

        public CharacterSheet(string name, CharacterRace race, ClassArchetype job, int[] attributeScores, int[] skillScores)
        {
            Name = name;
            Race = race;
            Class = job;

            //Attributes
            attributes = new Attribute[attributeScores.Length];
            foreach (var bonus in Race.RacialAttributeBonuses)
            {
                attributeScores[bonus.attribute.ID] += bonus.value;
            }
            foreach (var bonus in Class.ClassAttributes)
            {
                attributeScores[bonus.attribute.ID] += bonus.value;
            }
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i] = new Attribute(attributeScores[i]);
            }

            //Skills
            skills = new Skill[skillScores.Length];
            foreach (var bonus in Race.RacialSkillBonuses)
            {
                skillScores[bonus.skill.ID] += bonus.value;
            }
            foreach (var bonus in Class.ClassSkills)
            {
                skillScores[bonus.skill.ID] += bonus.value;
            }
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i] = new Skill(skillScores[i]);
            }
        }
    }
}

