using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Button _showInventoryButton;
    [SerializeField] private Button _showWeaponsButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _miscPanel;
    [SerializeField] private GameObject _weaponsPanel;
    [SerializeField] private GameObject _tooltipPanel;

    private void Awake()
    {
        _showInventoryButton.onClick.AddListener(ShowInventoryPanel);
        _showWeaponsButton.onClick.AddListener(ShowWeaponsPanel);

        ShowInventoryPanel();
    }

    private void OnDestroy()
    {
        _showInventoryButton.onClick.RemoveAllListeners();
        _showWeaponsButton.onClick.RemoveAllListeners();
    }

    private void ShowInventoryPanel()
    {
        _inventoryPanel.SetActive(true);
        _miscPanel.SetActive(true);

        _weaponsPanel.SetActive(false);
        _tooltipPanel.SetActive(false);

        _showInventoryButton.interactable = false;
        _showWeaponsButton.interactable = true;
    }

    private void ShowWeaponsPanel()
    {
        _inventoryPanel.SetActive(false);
        _miscPanel.SetActive(false);

        _weaponsPanel.SetActive(true);
        _tooltipPanel.SetActive(true);

        _showInventoryButton.interactable = true;
        _showWeaponsButton.interactable = false;
    }
}
