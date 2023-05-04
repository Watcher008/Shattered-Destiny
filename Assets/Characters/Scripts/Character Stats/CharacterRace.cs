using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Race")]
    public class CharacterRace : ScriptableObject
    {
        [field: SerializeField] public string RaceName { get; private set; }
        [field: SerializeField] public CreatureSize Size { get; private set; }

        [Space]

        [Tooltip("Modifier to the rate of gaining experience. Shorter lived races level up faster.")]
        [SerializeField][Range(0.5f, 5.0f)] private float experienceRate;
        public float ExperienceRate => experienceRate;

        [Space] [Space]

        [SerializeField] private AttributeReference[] racialAttributeBonuses;
        public AttributeReference[] RacialAttributeBonuses => racialAttributeBonuses;

        [field: SerializeField] public SkillReference[] RacialSkillBonuses { get; private set; }
        
        [field: SerializeField] public DiceCombo StartingAge { get; private set; }
        
        [field: SerializeField] public DiceCombo LifeSpan { get; private set; }
    }
}

public enum CreatureSize { Tiny, Small, Medium, Large, Huge, Enormous }