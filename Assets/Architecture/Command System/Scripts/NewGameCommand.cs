using SD.Characters;
using SD.Combat.WeaponArts;
using SD.Inventories;
using UnityEngine;

namespace SD.CommandSystem
{
    /// <summary>
    /// A general command that triggers events but doesn't have specific functionality of its own.
    /// </summary>
    [CreateAssetMenu(menuName = "Command System/New Game Command")]
    public class NewGameCommand : CommandBase
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerWeaponData _weaponData;

        [Space]

        [SerializeField] private WeaponArt[] _startingArts;


        private readonly int[] defaultStats = { 15, 15, 15, 15 };
        private readonly int[] defaultXP = { 0, 0, 0, 0 };

        protected override bool ExecuteCommand()
        {
            return StartNewGame();
        }

        /// <summary>
        /// Initializes gameplay.
        /// </summary>
        /// <returns>Command success result.</returns>
        private bool StartNewGame()
        {
            DateTime.ResetTime();

            // Initialize player data components
            _playerData.PlayerStats = new CharacterSheet(defaultStats, defaultXP, 5, 5, 3, 1);
            _playerData.Inventory = new Inventory(new Vector2Int(8, 10));
            _playerData.PlayerEquip = new PlayerEquipment(_playerData);

            _weaponData.Init();
            for (int i = 0; i < _startingArts.Length; i++)
            {
                _weaponData.LearnArt(_startingArts[i]);
            }

            return true;
        }
    }
}