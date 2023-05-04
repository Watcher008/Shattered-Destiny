using UnityEngine;

namespace SD.CommandSystem
{
    /// <summary>
    /// A general command that triggers events but doesn't have specific functionality of its own.
    /// </summary>
    [CreateAssetMenu(menuName = "Command System/Empty Command")]
    public class EmptyCommand : CommandBase
    {
        protected override bool ExecuteCommand()
        {
            return true;
        }
    }
}

