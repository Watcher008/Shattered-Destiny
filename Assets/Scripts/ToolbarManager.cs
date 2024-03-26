using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class to handle the various buttons and their respective panels on the world map.
/// </summary>
public class ToolbarManager : MonoBehaviour
{
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _journalButton;
    [SerializeField] private Button _skillsButton;
    [SerializeField] private Button _campButton;

    [Space]

    [SerializeField] private UI_CampSelection _camping;

    private void Awake()
    {
        _characterButton.onClick.AddListener(OnCharacterSelected);
        _inventoryButton.onClick.AddListener(OnInventorySelected);
        _journalButton.onClick.AddListener(OnJournalSelected);
        _skillsButton.onClick.AddListener(OnSkillsSelected);
        _campButton.onClick.AddListener(OnCampSelected);
    }

    private void OnDestroy()
    {
        _characterButton.onClick.RemoveAllListeners();
        _inventoryButton.onClick.RemoveAllListeners();
        _journalButton.onClick.RemoveAllListeners();
        _skillsButton.onClick.RemoveAllListeners();
        _campButton.onClick.RemoveAllListeners();
    }

    private void OnCharacterSelected()
    {
        // is the panel open? close it
        // else open it and close all other panels
    }

    private void OnInventorySelected()
    {
        // is the panel open? close it
        // else open it and close all other panels
    }

    private void OnJournalSelected()
    {
        // is the panel open? close it
        // else open it and close all other panels
    }

    private void OnSkillsSelected()
    {
        // is the panel open? close it
        // else open it and close all other panels
    }

    private void OnCampSelected()
    {
        if (_camping.IsOpen)
        {
            // Close panel?
        }
        else
        {
            // Close all other panels
            _camping.OpenPanel();
        }
    }
}
