using System.Collections.Generic;
using UnityEngine;
using SD.Primitives;
using SD.Inventories;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Data", fileName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public delegate void OnEquipmentChanged();
        public OnEquipmentChanged onEquipmentChanged;

        #region - World Map Location
        private int x, y;
        public int X
        {
            get => x;
            set => x = value;
        }
        public int Y
        {
            get => y;
            set => y = value;
        }
        #endregion

        private static int[] defaultStats = { 15, 15, 15, 15 };
        private static int[] defaultXP = {0, 0, 0, 0};

        private CharacterSheet _playerStats = new CharacterSheet(defaultStats, defaultXP, 5, 5, 3, 1);
        private List<CharacterSheet> _companions = new();

        private Inventory _inventory = new(new Vector2Int(10, 10));
        private InventoryItem[] _equipment = new InventoryItem[System.Enum.GetNames(typeof(EquipmentType)).Length];

        private int _travelSpeed;
        private int _exhaustion;

        public CharacterSheet PlayerStats
        {
            get => _playerStats;
            set => _playerStats = value;
        }
        public List<CharacterSheet> Companions => _companions;
        public int TravelSpeed
        {
            get => _travelSpeed;
            set => _travelSpeed = value;
        }
        public int Exhaustion
        {
            get => _exhaustion;
            set => _exhaustion = Mathf.Clamp(value, 0, 10);
        }

        public Inventory Inventory => _inventory;
        public InventoryItem[] Equipment => _equipment;

        #region - Reputation & Influence - 
        [SerializeField] private IntReference[] _factionReputation;
        [SerializeField] private IntReference[] _factionInfluence;
        #endregion


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

        public void SetReputation(Factions faction, int value)
        {
            _factionReputation[(int)faction].Value += value;
        }

        public int GetReputation(Factions faction)
        {
            return _factionReputation[(int)faction].Value;
        }



        #region - Equipment -
        public bool TryEquipItem(EquipmentType slot, Item item)
        {
            Debug.LogWarning("This functionality has not been added yet.");

            // Is it the same item that was already there?

            // is it the correct slot?

            // is there an item already there? Need to unequip it and add to inventory

            return false;
        }
        #endregion
    }
}


public enum Factions
{
    KingdomOfZodia,
    ImperiumVitalis,
    Ath_rakTribes,
    EderanMerchantConglomerate,
}