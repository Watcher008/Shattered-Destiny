using UnityEngine;
using SD.Primitives;
using PixelCrushers.DialogueSystem;

namespace SD.CommandSystem
{
    /// <summary>
    /// A command that triggers dialogue.
    /// </summary>
    [CreateAssetMenu(menuName = "Command System/Start Dialogue Command")]
    public class StartDialogueCommand : CommandBase
    {
        [SerializeField] private IntReference _kingdomReputation;

        public void StartConversation(string conversation)
        {
            if (string.IsNullOrEmpty(conversation)) return;

            DialogueLua.SetVariable("Kingdom Reputation", _kingdomReputation.Value);

            DialogueManager.StartConversation(conversation);

            ExecuteCommand();
        }

        protected override bool ExecuteCommand()
        {
            return false;
        }
    }
}