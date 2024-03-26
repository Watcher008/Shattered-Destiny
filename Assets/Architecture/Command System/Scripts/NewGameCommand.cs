using SD.Characters;
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

            // Set player starting position
            _playerData.X = 75;// 78;
            _playerData.Y = 26;// 29;



            return true;
        }
    }
}