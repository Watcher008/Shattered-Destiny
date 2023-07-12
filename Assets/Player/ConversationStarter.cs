using PixelCrushers.DialogueSystem;
using SD.Primitives;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private StringReference conversationTitle;

    public void StartConversation()
    {
        if (conversationTitle == null) throw new System.Exception("Need to assign StringReference!");
        else if (conversationTitle.Value == string.Empty) throw new System.Exception("Conversation not assigned!");

        //DialogueManager.Set
        DialogueManager.StartConversation(conversationTitle.Value);
    }
}