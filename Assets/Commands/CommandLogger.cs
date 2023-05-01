using UnityEngine;

namespace SD.CommandSystem
{
    public class CommandLogger : MonoBehaviour
    {
        /// <summary>
        /// Debugging tool - Raise an assertion alert if command is triggered.
        /// </summary>
        [Header("*Optional")]
        [SerializeField] private CommandBase watchForCommand;

        /// <summary>
        /// Log player command to console.
        /// </summary>
        /// <param name="command">Player command to log.</param>
        /// <param name="successMessage">Execution success result message.</param>
        public void LogCommand(CommandBase command, string successMessage)
        {
            if (watchForCommand != null && command.name.Equals(watchForCommand.name))
            {
                Debug.LogAssertion($"{command.name}, {successMessage}", this);
                return;
            }

            Debug.Log($"{command.name}, {successMessage}", this);
        }
    }
}