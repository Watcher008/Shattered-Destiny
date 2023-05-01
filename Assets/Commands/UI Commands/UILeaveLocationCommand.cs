using UnityEngine;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/Leave Location Command")]
    public class UILeaveLocationCommand : CommandBase
    {
        protected override bool ExecuteCommand()
        {
            return LeaveLocation();
        }

        /// <summary>
        /// Closes Location Menu and Scene
        /// </summary>
        /// <returns>Command success result.</returns>
        private bool LeaveLocation()
        {
            // Placeholder - todo - more to come
            //Maybe need to check if the player CAN leave? What would be stopping them? Prison?
            return true;
        }
    }
}

