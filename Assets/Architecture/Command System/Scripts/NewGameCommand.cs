using SD.Characters;
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

        private int[] defaultStats = { 15, 15, 15, 15 };
        private int[] defaultXP = { 0, 0, 0, 0 };

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
            // Placeholder - todo - more to come
            DateTime.ResetTime();

            Debug.Log("Setting default values for player character.");
            // Set player starting position
            _playerData.X = 75;// 78;
            _playerData.Y = 26;// 29;

            _playerData.PlayerStats = new CharacterSheet(defaultStats, defaultXP, 5, 5, 3, 1);
            _playerData.Inventory = new Inventory(new Vector2Int(10, 10));
            _playerData.PlayerEquip = new PlayerEquipment(_playerData);

            return true;
        }
    }
}