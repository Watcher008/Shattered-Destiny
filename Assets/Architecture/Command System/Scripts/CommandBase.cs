using System.Collections.Generic;
using UnityEngine;
using SD.EventSystem;

namespace SD.CommandSystem
{
    public abstract class CommandBase : ScriptableObject
    {
        /// <summary>
        /// Trigger events after executing command.
        /// </summary>
        [Header("*Optional")]
        [SerializeField] private List<GameEvent> callbackEvents;
        //[SerializeField] private FeedbackCollection _feedbackCollection;

#pragma warning disable 0414
#if UNITY_EDITOR
        // Display notes field in the inspector.
        [Multiline, SerializeField, Space(10)]
        private string developerNotes = "";
#endif
#pragma warning restore 0414

        private CommandLogger commandLogger;

        /// <summary>
        /// Invoke the command execution, log the result, then trigger callbacks.
        /// </summary>
        public void Invoke()
        {
            bool success = ExecuteCommand();

            if (!success)
            {
                LogFailure("Action Failure");
                return;
            }

            LogSuccess();
            InvokeCallbackEvents();
            InvokeFeedback();
        }

        /// <summary>
        /// Log a success message after executing command.
        /// </summary>
        private void LogSuccess() => LogEntry();

        /// <summary>
        /// Log a fail message after failing to execute command.
        /// </summary>
        /// <param name="failReason">Details of why the command execution failed.</param>
        private void LogFailure(string failReason) => LogEntry(false, failReason);

        /// <summary>
        /// Log the command execution to the console window.
        /// </summary>
        /// <param name="success">Execution success result.</param>
        /// <param name="explanation">Execution result details.</param>
        private void LogEntry(bool success = true, string explanation = "")
        {
            if (commandLogger == null) SetupCommandLogger();

            string successMessage = (success) ? "Success" : "Failure";

            if (!explanation.Equals(string.Empty))
                successMessage += $" reason: {explanation}";

            commandLogger?.LogCommand(this, successMessage);
        }

        /// <summary>
        /// Find the command logger on initial load.
        /// </summary>
        private void SetupCommandLogger()
        {
            GameObject actionLogGameObject = GameObject.FindGameObjectWithTag("CommandLog");
            commandLogger = actionLogGameObject.GetComponent<CommandLogger>();
        }

        /// <summary>
        /// Trigger all attached callback events.
        /// </summary>
        private void InvokeCallbackEvents()
        {
            for (int i = 0; i < callbackEvents?.Count; i++)
            {
                callbackEvents?[i].Invoke();
            }
        }

        /// <summary>
        /// Triggers all items in feedback collection.
        /// </summary>
        private void InvokeFeedback()
        {
            /*for (int i = 0; i < _feedbackCollection?.Items.Count; i++)
            {
                _feedbackCollection?.Items[i].Invoke();
            }*/
        }

        /// <summary>
        /// Execute the requested command.
        /// </summary>
        /// <returns>Execution success result.</returns>
        protected abstract bool ExecuteCommand();
    }
}
