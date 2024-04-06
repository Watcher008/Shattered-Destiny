using System.Collections.Generic;
using UnityEngine;
using SD.Inventories;
using SD.Combat.WeaponArts;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Data", fileName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public List<WeaponArt> WeaponArts = new List<WeaponArt>();

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private Sprite _sprite;
        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }
        private Vector2Int _worldPosition; 
        public Vector2Int WorldPos
        {
            get => _worldPosition;
            set => _worldPosition = value;
        }

        private CharacterSheet _playerStats;
        private PlayerEquipment _equipment;
        private List<CharacterSheet> _companions = new();
        private Inventory _inventory = new(new Vector2Int(10, 10));

        private int _marchSpeed;
        private int _exhaustion;

        private WeaponTypes _rightHand = WeaponTypes.Sword;
        private WeaponTypes _leftHand = WeaponTypes.Shield;

        public List<CharacterSheet> Companions => _companions;
        public int MarchSpeed
        {
            get => _marchSpeed;
            set => _marchSpeed = value;
        }
        public int Exhaustion
        {
            get => _exhaustion;
            set => _exhaustion = Mathf.Clamp(value, 0, 10);
        }

        public CharacterSheet PlayerStats
        {
            get => _playerStats;
            set => _playerStats = value;
        }
        public Inventory Inventory
        {
            get => _inventory;
            set => _inventory = value;
        }
        public PlayerEquipment PlayerEquip
        {
            get => _equipment;
            set => _equipment = value;
        }

        public WeaponTypes RightHand => _rightHand;
        public WeaponTypes LeftHand => _leftHand;

        public void SetWeapon(WeaponTypes weapon, Hand hand)
        {
            if (weapon == WeaponTypes.None) return;

            // Set weapon
            if (hand == Hand.Right) _rightHand = weapon;
            else _leftHand = weapon;

            // If two handed, empty other hand
            if (IsTwoHanded(weapon))
            {
                if (hand == Hand.Right) _leftHand = WeaponTypes.None;
                else _rightHand = WeaponTypes.None;
            }

            if (_rightHand != _leftHand) return;

            // Handle wielding same weapon twice, default to sword or shield
            if (hand == Hand.Right)
            {
                if (weapon == WeaponTypes.Sword) _leftHand = WeaponTypes.Shield;
                else _leftHand = WeaponTypes.Sword;
            }
            else
            {
                if (weapon == WeaponTypes.Sword) _rightHand = WeaponTypes.Shield;
                else _rightHand = WeaponTypes.Sword;
            }
        }

        private bool IsTwoHanded(WeaponTypes weapon)
        {
            if (weapon == WeaponTypes.Warhammer) return true;
            if (weapon == WeaponTypes.Staff) return true;
            if (weapon == WeaponTypes.Bow) return true;
            return false;
        }


        /// <summary>
        /// Regain health for all party members during rest.
        /// </summary>
        public void OnRest(int value)
        {
            _playerStats.RegainHealth(value);
            for (int i = 0; i < _companions.Count; i++)
            {
                _companions[i].RegainHealth(value);
            }
        }
    }
}

public enum Hand { Right, Left };

public enum WeaponTypes
{
    None,
    Sword,
    Shield,
    Warhammer,
    Bow,
    Staff,
    Book
}