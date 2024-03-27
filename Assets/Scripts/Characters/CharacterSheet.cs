using SD.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace SD.Characters
{
    /// <summary>
    /// A class to handle dynamic character stats.
    /// </summary>
    public class CharacterSheet
    {
        private Stat[] _attributes;

        private int _maxHealth;
        private int _currentHealth;
        private int _movement;
        private int _initiative;

        private int _maxActionPoints;
        private int _startingActionPoints;
        private int _refreshActionPoints;

        private Weapon _weapon;
        private List<WeaponArt> _weaponArts;

        public int MaxHealth => _maxHealth;
        public int Health => _currentHealth;
        public int Movement => _movement;
        public int Initiative => _initiative;

        public int MaxActionPoints => _maxActionPoints;
        public int StartingActionPoints => _startingActionPoints;
        public int RefreshActionPoints => _refreshActionPoints;

        public Weapon Weapon => _weapon;
        public List<WeaponArt> WeaponArts => _weaponArts;


        public CharacterSheet(int[] attributes, int[] attributeXP, int maxHp, int hp, int move, int init, int maxAP = 5, int startAP = 3, int refreshAP = 1)
        {
            _attributes = new Stat[System.Enum.GetNames(typeof(Attributes)).Length];
            for (int i = 0; i < _attributes.Length; i++)
            {
                _attributes[i] = new Stat(attributes[i], attributeXP[i]);
            }

            _maxHealth = maxHp;
            _currentHealth = hp;
            _movement = move;
            _initiative = init;

            _maxActionPoints = maxAP;
            _startingActionPoints = startAP;
            _refreshActionPoints = refreshAP;

            _weaponArts = new();
        }

        /// <summary>
        /// Returns the value of the requested attribute.
        /// </summary>
        public int GetAttribute(Attributes attribute)
        {
            return _attributes[(int)attribute].Value;
        }

        /// <summary>
        /// Returns the 10th place value of the requested attribute, minimum 1.
        /// </summary>
        public int GetAttributeBonus(Attributes attribute)
        {
            var value = Mathf.Clamp(Mathf.RoundToInt(_attributes[(int)attribute].Value / 10), 1, 10);
            return value;
        }

        public void GainXP(Attributes attribute, int xp)
        {
            _attributes[(int)attribute].GainXP(xp);
        }

        public void TakeDamage(int dmg)
        {
            _currentHealth -= dmg;
            if (_currentHealth < 0) _currentHealth = 0;
        }

        public void RegainHealth(int health)
        {
            _currentHealth += health;
            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        }

        public Weapon EquipWeapon(Weapon newWeapon)
        {
            var oldWeapon = _weapon;
            _weapon = newWeapon;
            return oldWeapon;
        }
    }

    public enum Attributes
    {
        Physicality,
        Intelligence,
        Survival,
        Social
    }
}