using UnityEngine;

namespace SD.CommandSystem
{
    /// <summary>
    /// A general command that triggers events but doesn't have specific functionality of its own.
    /// </summary>
    [CreateAssetMenu(menuName = "Command System/Continue Game Command")]
    public class ContinueGameCommand : CommandBase
    {
        protected override bool ExecuteCommand()
        {
            return LoadGame();
        }

        /// <summary>
        /// Initializes gameplay.
        /// </summary>
        /// <returns>Command success result.</returns>
        private bool LoadGame()
        {
            // Placeholder - todo - more to come
            return true;
        }
    }
}