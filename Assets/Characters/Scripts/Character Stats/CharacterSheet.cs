using UnityEngine;

namespace SD.CharacterSystem
{
    public class CharacterSheet
    {
        public string Name { get; private set; }
        public CharacterRace Race { get; private set; }
        public ClassArchetype Class { get; private set; }

        public AttributeStat[] Attributes { get; private set; }
        public SkillStat[] Skills { get; private set; }

        public CharacterSheet(string name, CharacterRace race, ClassArchetype job, int[] attributeScores, int[] skillScores)
        {
            Name = name;
            Race = race;
            Class = job;

            //Attributes
            Attributes = new AttributeStat[attributeScores.Length];
            foreach (var bonus in Race.RacialAttributeBonuses)
            {
                attributeScores[bonus.attribute.ID] += bonus.value;
            }
            foreach (var bonus in Class.ClassAttributes)
            {
                attributeScores[bonus.attribute.ID] += bonus.value;
            }
            for (int i = 0; i < Attributes.Length; i++)
            {
                Attributes[i] = new AttributeStat((Attribute)i, attributeScores[i]);
            }

            //Skills
            Skills = new SkillStat[skillScores.Length];
            foreach (var bonus in Race.RacialSkillBonuses)
            {
                skillScores[bonus.skill.ID] += bonus.value;
            }
            foreach (var bonus in Class.ClassSkills)
            {
                skillScores[bonus.skill.ID] += bonus.value;
            }
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i] = new SkillStat((Skill)i, skillScores[i]);
            }
        }

        public void OnGainSkillXP(Skill skill, int xpValue)
        {
            float xpMod = GetSkillXPMod(skill);
            xpValue = Mathf.RoundToInt(xpValue * xpMod);
            Skills[(int)skill].OnGainXP(xpValue);
        }

        private float GetSkillXPMod(Skill skill)
        {
            switch (skill)
            {
                case Skill.AnimalHandling:
                    return GetXPModifier(Attribute.Intelligence);
                case Skill.Athletics:
                    {
                        float str = GetXPModifier(Attribute.Strength);
                        float agi = GetXPModifier(Attribute.Agility);
                        return (str + agi) * 0.5f;
                    }
                case Skill.Barter:
                    {
                        float intl = GetXPModifier(Attribute.Intelligence);
                        float cha = GetXPModifier(Attribute.Charisma);
                        float per = GetXPModifier(Attribute.Perception);
                        return (intl + cha + per) * 0.333f;
                    }
                case Skill.Camping:
                    return GetXPModifier(Attribute.Perception);
                case Skill.Deception:
                    return GetXPModifier(Attribute.Charisma);
                case Skill.Doctor:
                    return GetXPModifier(Attribute.Intelligence);
                case Skill.Gambling:
                    {
                        float intl = GetXPModifier(Attribute.Intelligence);
                        float cha = GetXPModifier(Attribute.Charisma);
                        return (intl + cha) * 0.5f;
                    }
                case Skill.Herbalism:
                    return GetXPModifier(Attribute.Intelligence);
                case Skill.Hunting:
                    {
                        float agi = GetXPModifier(Attribute.Agility);
                        float str = GetXPModifier(Attribute.Strength);
                        float per = GetXPModifier(Attribute.Perception);
                        return (agi + str + per) * 0.333f;
                    }
                case Skill.Intimidation:
                    {
                        float cha = GetXPModifier(Attribute.Charisma);
                        float str = GetXPModifier(Attribute.Strength);
                        return (cha + str) * 0.5f;
                    }
                case Skill.Navigation:
                    {
                        float per = GetXPModifier(Attribute.Perception);
                        float intl = GetXPModifier(Attribute.Intelligence);
                        return (per + intl) * 0.5f;
                    }
                case Skill.Persuasion:
                    return GetXPModifier(Attribute.Charisma);
                case Skill.Stealth:
                    {
                        float agi = GetXPModifier(Attribute.Agility);
                        float per = GetXPModifier(Attribute.Perception);
                        return (agi + per) * 0.5f;
                    }
                default: return 1.0f;
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
    Agility,
    Charisma,
    Intelligence,
    Magic,
    Perception,
    Strength
}

public enum Skill
{
    AnimalHandling,
    Athletics,
    Barter,
    Camping,
    Deception,
    Doctor,
    Gambling,
    Herbalism,
    Hunting,
    Intimidation,
    Navigation,
    Persuasion,
    Stealth,
}