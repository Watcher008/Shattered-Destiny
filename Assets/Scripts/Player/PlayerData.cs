using System.Collections.Generic;
using UnityEngine;
using SD.Inventories;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Data", fileName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
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