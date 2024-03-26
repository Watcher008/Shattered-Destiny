using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SD.EventSystem;
using SD.Primitives;
using SD.CommandSystem;
using System.Net.NetworkInformation;

public class TownManager : MonoBehaviour
{
    [Header("Architecture")]
    [SerializeField] private GameEvent _leaveTownEvent;
    [SerializeField] private StartDialogueCommand _startDialogueCommand;

    [Header("Town Activities")]
    [SerializeField] private Button _marketButton;
    [SerializeField] private Button _officialsButton;
    [SerializeField] private Button _guildButton;
    [SerializeField] private Button _tavernButton;
    [SerializeField] private Button _questBoardButton;
    [SerializeField] private Button _leaveTownButton;

    [Header("Quick Talk")]
    [SerializeField] private Button _quickTalk01Button;
    [SerializeField] private Button _quickTalk02Button;
    [SerializeField] private Button _quickTalk03Button;

    private void Awake()
    {
        _marketButton.onClick.AddListener(OnMarket);
        _officialsButton.onClick.AddListener(OnOfficials);
        _guildButton.onClick.AddListener(OnGuild);
        _tavernButton.onClick.AddListener(OnTavern);
        _questBoardButton.onClick.AddListener(OnQuestBoard);
        _leaveTownButton.onClick.AddListener(OnLeaveTown);

        _quickTalk01Button.onClick.AddListener(OnQuickTalk01);
    }

    private void OnDestroy()
    {
        _marketButton.onClick.RemoveAllListeners();
        _officialsButton.onClick.RemoveAllListeners();
        _guildButton.onClick.RemoveAllListeners();
        _tavernButton.onClick.RemoveAllListeners();
        _questBoardButton.onClick.RemoveAllListeners();
        _leaveTownButton.onClick.RemoveAllListeners();

        _quickTalk01Button.onClick.RemoveAllListeners();
        _quickTalk02Button.onClick.RemoveAllListeners();
        _quickTalk03Button.onClick.RemoveAllListeners();
    }

    private void OnMarket()
    {
        Debug.Log("OnMarket");
    }

    private void OnOfficials()
    {
        Debug.Log("OnOfficials");
    }

    private void OnGuild()
    {
        Debug.Log("OnGuild");
    }

    private void OnTavern()
    {
        Debug.Log("OnTavern");
    }

    private void OnQuestBoard()
    {
        Debug.Log("OnQuestBoard");
    }

    private void OnQuickTalk01()
    {
        _startDialogueCommand.StartConversation("Test Conversation 1");
    }

    private void OnLeaveTown()
    {
        WorldMapManager.Instance.onResumeInput?.Invoke();
        _leaveTownEvent.Invoke();
    }
}
