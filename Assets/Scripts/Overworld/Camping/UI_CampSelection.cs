using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CampSelection : MonoBehaviour
{
    public bool IsOpen => _panel.activeSelf || _camping.IsOpen;

    private UI_Camping _camping;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _makeshiftCampButton;
    [SerializeField] private Button _basicCampButton;
    [SerializeField] private Button _advancedCampButton;
    [SerializeField] private Button _cancelButton;

    private TMP_Text _basicText;
    private TMP_Text _advancedText;

    private readonly string _basicCampString = "Basic Camp Supplies\n({0} available)";
    private readonly string _advanceCampString = "Advanced Camp Supplies\n({0} available)";

    private void Awake()
    {
        _camping = GetComponent<UI_Camping>();

        _basicText = _basicCampButton.GetComponentInChildren<TMP_Text>();
        _advancedText = _advancedCampButton.GetComponentInChildren<TMP_Text>();

        _makeshiftCampButton.onClick.AddListener(OnMakeshiftCampSelected);
        _basicCampButton.onClick.AddListener(OnBasicCampSelected);
        _advancedCampButton.onClick.AddListener(OnAdvancedCampSelected);
        _cancelButton.onClick.AddListener(OnCancel);
    }

    private void OnDestroy()
    {
        _makeshiftCampButton.onClick.RemoveAllListeners();
        _basicCampButton.onClick.RemoveAllListeners();
        _advancedCampButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
    }

    public void OpenPanel()
    {
        _panel.SetActive(true);

        // Will have to add functionality to check player inventory later
        // Can also enable/disable the buttons if count >= 0
        _basicText.text = string.Format(_basicCampString, 0);
        _advancedText.text = string.Format(_advanceCampString, 0);

        WorldMapManager.Instance.onPauseInput?.Invoke();
    }

    private void OnMakeshiftCampSelected()
    {
        _camping.OpenPanel(Camp.Makeshift);
    }

    private void OnBasicCampSelected()
    {
        // Check that the player has at least one basic camp set
        _camping.OpenPanel(Camp.Basic);
    }

    private void OnAdvancedCampSelected()
    {
        // Check that the player has at least one advanced camp set
        _camping.OpenPanel(Camp.Advanced);
    }

    private void OnCancel()
    {
        _panel.SetActive(false);
        WorldMapManager.Instance.onResumeInput?.Invoke();
    }
}
