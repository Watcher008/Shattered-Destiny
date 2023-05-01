using UnityEngine;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/New Game Command")]
    public class UINewGameCommand : CommandBase
    {
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
            return true;
        }
    }
}