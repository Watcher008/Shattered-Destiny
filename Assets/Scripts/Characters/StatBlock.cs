using SD.Combat.WeaponArts;
using UnityEngine;

namespace SD.Characters
{
    /// <summary>
    /// A class to handle static character stats.
    /// These values will be copied over to the Combatant class during combat.s
    /// </summary>
    [CreateAssetMenu(menuName = "Combat/Stat Block")]
    public class StatBlock : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _maxHealth;
        [SerializeField] private byte _movement;
        [SerializeField] private byte _initiative;

        [SerializeField] private byte[] _attributes = new byte[4];

        [Space]

        [SerializeField] private byte _map;
        [SerializeField] private byte _sap;
        [SerializeField] private byte _rap;

        [Space]

        [SerializeField] private WeaponTypes _weapon;
        [SerializeField] private WeaponArt[] _weaponArts;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public byte[] Attributes => _attributes;
        public int MaxHealth => _maxHealth;
        public byte Movement => _movement;
        public byte Initiative => _initiative;

        public byte MAP => _map;
        public byte SAP => _sap;
        public byte RAP => _rap;

        public WeaponTypes Weapon => _weapon;
        public WeaponArt[] WeaponArts => _weaponArts;

        /// <summary>
        /// Returns the 10th place value of the requested attribute, minimum 1.
        /// </summary>
        public int GetAttributeBonus(Attributes attribute)
        {
            var value = Mathf.Clamp(Mathf.RoundToInt(Attributes[(int)attribute] / 10), 1, 10);
            return value;
        }
    }
}