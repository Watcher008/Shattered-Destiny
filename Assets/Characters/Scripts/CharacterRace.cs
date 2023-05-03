using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Race")]
    public class CharacterRace : ScriptableObject
    {
        [field: SerializeField] public string RaceName { get; private set; }
        [field: SerializeField] public AttributeReference[] RacialAttributes { get; private set; }
        [field: SerializeField] public SkillReference[] RacialSkills { get; private set; }

        [field: SerializeField] public DiceCombo LifeSpan { get; private set; }
    }
}