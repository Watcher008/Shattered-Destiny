using SD.Characters;
using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerWeaponData _playerWeapons;
    
    [SerializeField] private Button _rightHandButton;
    [SerializeField] private Button _leftHandButton;

    private Image _rightHandImage;
    private Image _leftHandImage;
    [SerializeField] private Image _rightHandImage02;
    [SerializeField] private Image _leftHandImage02;

    [SerializeField] private Button _cancelSelectButton;
    [SerializeField] private Button[] _weaponButtons;

    [Header("To be moved")]
    [SerializeField] private Button _showInventoryButton;
    [SerializeField] private Button _showWeaponsButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _miscPanel;
    [SerializeField] private GameObject _weaponsPanel;
    [SerializeField] private GameObject _tooltipPanel;

    private readonly Color _occupied = new Color(1.0f, 1.0f, 1.0f, 200.0f / 255.0f);

    private readonly string[] _spritePaths =
    {
        "misc/unarmed",
        "misc/sword",
        "misc/shield",
        "misc/warhammer",
        "misc/bow",
        "misc/staff",
        "misc/book",
    };

    private Hand _hand;

    private void Awake()
    {
        _rightHandImage = _rightHandButton.GetComponent<Image>();
        _leftHandImage = _leftHandButton.GetComponent<Image>();

        UpdateWeaponSprites();

        _rightHandButton.onClick.AddListener(OnSelectRightHand);
        _leftHandButton.onClick.AddListener(OnSelectLeftHand);

        _cancelSelectButton.image.raycastTarget = false;

        _showInventoryButton.onClick.AddListener(ShowInventoryPanel);
        _showWeaponsButton.onClick.AddListener(ShowWeaponsPanel);

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            int index = i;
            _weaponButtons[index].interactable = false;
            _weaponButtons[index].onClick.AddListener(delegate
            {
                // The first item in enum is None - reserved for two-handed weapons
                SetWeapon(index + 1);
            });
        }

        ShowInventoryPanel();
        UpdateWeaponSprites();
    }

    private void OnDestroy()
    {
        _cancelSelectButton.onClick.RemoveAllListeners();
        _rightHandButton.onClick.RemoveAllListeners();
        _leftHandButton.onClick.RemoveAllListeners();
        _showInventoryButton.onClick.RemoveAllListeners();
        _showWeaponsButton.onClick.RemoveAllListeners();

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].onClick.RemoveAllListeners();
        }
    }

    private void ShowInventoryPanel()
    {
        OnCancelSelection();
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

    /// <summary>
    /// Updates the weapon sprites to reflect player's currenlty equipped loadout.
    /// </summary>
    private void UpdateWeaponSprites()
    {
        _rightHandImage.color = Color.white;
        _rightHandImage02.color = Color.white;
        _leftHandImage.color = Color.white;
        _leftHandImage02.color = Color.white;

        var right = _playerWeapons.RightHand == WeaponTypes.None ? _playerWeapons.LeftHand : _playerWeapons.RightHand;
        var left = _playerWeapons.LeftHand == WeaponTypes.None ? _playerWeapons.RightHand : _playerWeapons.LeftHand;

        var rightSprite = SpriteHelper.GetSprite(_spritePaths[(int)right]);
        var leftSprite = SpriteHelper.GetSprite(_spritePaths[(int)left]);

        _rightHandImage.sprite = rightSprite;
        _rightHandImage02.sprite = rightSprite;

        _leftHandImage.sprite = leftSprite;
        _leftHandImage02.sprite = leftSprite;

        // Set lower opacity on empty hand to show that it's occupied by the other hand weapon
        if (right == WeaponTypes.None)
        {
            _rightHandImage.color = _occupied;
            _rightHandImage02.color = _occupied;
        }
        else if (left == WeaponTypes.None)
        {
            _leftHandImage.color = _occupied;
            _leftHandImage02.color = _occupied;
        }
    }

    private void OnSelectRightHand()
    {
        _hand = Hand.Right;

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].interactable = true;
        }
    }

    private void OnSelectLeftHand()
    {
        _hand = Hand.Left;

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].interactable = true;
        }
    }

    private void SetWeapon(int index)
    {
        _playerWeapons.SetWeapon((WeaponTypes)index, _hand);
        UpdateWeaponSprites();
        // Update Weapon Arts

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].interactable = false;
        }
    }

    private void OnCancelSelection()
    {

    }
}
