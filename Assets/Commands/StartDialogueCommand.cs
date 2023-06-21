using PixelCrushers.DialogueSystem;
using SD.EventSystem;
using UnityEngine;

namespace SD.CommandSystem
{
    /// <summary>
    /// A command that triggers dialogue.
    /// </summary>
    [CreateAssetMenu(menuName = "Command System/Start Dialogue Command")]
    public class StartDialogueCommand : CommandBase
    {
        [SerializeField] private GameEvent dialogueStartEvent;
        private string conversationTitle;
        public void StartConversation(string name)
        {
            conversationTitle = name;
            ExecuteCommand();
        }

        protected override bool ExecuteCommand()
        {
            return InitiateDialogue();
        }

        private bool InitiateDialogue()
        {
            if (conversationTitle == null || conversationTitle == string.Empty) return false;

            dialogueStartEvent?.Invoke();
            DialogueManager.StartConversation(conversationTitle);
            conversationTitle = string.Empty;
            return true;
        }
    }
}

