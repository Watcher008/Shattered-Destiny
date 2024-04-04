using SD.Characters;
using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    
    [SerializeField] private Button _rightHandButton;
    [SerializeField] private Button _leftHandButton;

    private Image _rightHandImage;
    private Image _leftHandImage;

    [SerializeField] private Button _selectionPanel;
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

    private enum Hand { Right, Left };
    private Hand _hand;

    private void Awake()
    {
        _rightHandImage = _rightHandButton.transform.GetChild(0).GetComponent<Image>();
        _leftHandImage = _leftHandButton.transform.GetChild(0).GetComponent<Image>();

        UpdateWeaponSprites();

        _rightHandButton.onClick.AddListener(OnSelectRightHand);
        _leftHandButton.onClick.AddListener(OnSelectLeftHand);
        _selectionPanel.onClick.AddListener(ClosePanel);

        _showInventoryButton.onClick.AddListener(ShowInventoryPanel);
        _showWeaponsButton.onClick.AddListener(ShowWeaponsPanel);

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            int index = i;
            _weaponButtons[index].onClick.AddListener(delegate
            {
                SetWeapon(index);
            });
        }

        ShowInventoryPanel();
    }

    private void OnDestroy()
    {
        _rightHandButton.onClick.RemoveAllListeners();
        _leftHandButton.onClick.RemoveAllListeners();
        _selectionPanel.onClick.RemoveAllListeners();
        _showInventoryButton.onClick.RemoveAllListeners();
        _showWeaponsButton.onClick.RemoveAllListeners();

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].onClick.RemoveAllListeners();
        }
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

    /// <summary>
    /// Updates the weapon sprites to reflect player's currenlty equipped loadout.
    /// </summary>
    private void UpdateWeaponSprites()
    {
        _rightHandImage.sprite = SpriteHelper.GetSprite(_spritePaths[(int)_playerData.RightHand]);
        _leftHandImage.sprite = SpriteHelper.GetSprite(_spritePaths[(int)_playerData.LeftHand]);

        bool twoHanded = _playerData.RightHand == _playerData.LeftHand;

        // Set lower opacity on left hand to show that it's occupied by the right hand weapon
        _leftHandImage.color = twoHanded ? _occupied : Color.white;
        // If right hand has two-handed weapon, have to change that first
        _leftHandButton.interactable = !twoHanded;
    }

    private void OnSelectRightHand()
    {
        _hand = Hand.Right;
        _selectionPanel.gameObject.SetActive(true);

        // This one should be fairly straightforward
        // The only thing that the player can't equip in their right hand is Nothing and the Shield

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].gameObject.SetActive(true);
        }
        _weaponButtons[(int)WeaponTypes.None].gameObject.SetActive(false);
        _weaponButtons[(int)WeaponTypes.Shield].gameObject.SetActive(false);
    }

    private void OnSelectLeftHand()
    {
        _hand = Hand.Left;
        _selectionPanel.gameObject.SetActive(true);

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].gameObject.SetActive(true);
        }

        // Have to equip two-handed weapons through right hand selection
        _weaponButtons[(int)WeaponTypes.Warhammer].gameObject.SetActive(false);
        _weaponButtons[(int)WeaponTypes.Bow].gameObject.SetActive(false);
        _weaponButtons[(int)WeaponTypes.Staff].gameObject.SetActive(false);

        // Can't equip the same as whatever is in the right hand (sword, book)
        _weaponButtons[(int)_playerData.RightHand].gameObject.SetActive(false);
    }

    private void SetWeapon(int index)
    {
        if (_hand == Hand.Right) _playerData.RightHand = (WeaponTypes)index;
        else _playerData.LeftHand = (WeaponTypes)index;

        UpdateWeaponSprites();
        _selectionPanel.gameObject.SetActive(false);
    }

    private void ClosePanel()
    {
        _selectionPanel.gameObject.SetActive(false);
    }
}
