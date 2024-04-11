using SD.Characters;
using SD.Combat.WeaponArts;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerWeaponData _playerWeapons;
    [SerializeField] private InventoryUIManager _manager;
    [SerializeField] private Canvas _canvas;

    [Space]



    [SerializeField] private Button _cancelSelectButton;
    [SerializeField] private Button[] _weaponButtons;

    [Space]


    [Space]

    [SerializeField] private WeaponArtElement _elementPrefab;
    [SerializeField] private RectTransform _rightHandPoolParent;
    [SerializeField] private RectTransform _leftHandPoolParent;

    [SerializeField] private Sprite _lock;
    private readonly Color _occupied = new Color(1.0f, 1.0f, 1.0f, 200.0f / 255.0f);

    [Header("Right Hand")]
    [SerializeField] private Button _rightHandButton;
    [SerializeField] private TMP_Text _rightHandName;
    [SerializeField] private TMP_Text _rightHandTier;

    private Image _rightHandImage;
    [SerializeField] private Image _rightHandImage02;
    [SerializeField] private ActiveWeaponArtElement[] _rightHandWeaponArts;

    [Header("Left Hand")]
    [SerializeField] private Button _leftHandButton;
    [SerializeField] private TMP_Text _leftHandName;
    [SerializeField] private TMP_Text _leftHandTier;

    private Image _leftHandImage;
    [SerializeField] private Image _leftHandImage02;
    [SerializeField] private ActiveWeaponArtElement[] _leftHandWeaponArts;

    [Header("Tooltip")]
    [SerializeField] private TMP_Text _tooltipHeader;
    [SerializeField] private TMP_Text _tooltipText;

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

        _rightHandButton.onClick.AddListener(OnSelectRightHand);
        _leftHandButton.onClick.AddListener(OnSelectLeftHand);

        _cancelSelectButton.image.raycastTarget = false;

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

        UpdateWeapons();
    }

    private void OnDestroy()
    {
        _cancelSelectButton.onClick.RemoveAllListeners();
        _rightHandButton.onClick.RemoveAllListeners();
        _leftHandButton.onClick.RemoveAllListeners();

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].onClick.RemoveAllListeners();
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
        UpdateWeapons();

        for (int i = 0; i < _weaponButtons.Length; i++)
        {
            _weaponButtons[i].interactable = false;
        }
    }

    private void UpdateWeapons()
    {
        UpdateWeaponSprites();
        UpdateWeaponArts();
        UpdateWeaponArtPool();
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

        // Text
        if (_playerWeapons.RightHand == WeaponTypes.None)
        {
            _rightHandName.text = string.Empty;
            _rightHandTier.text = string.Empty;
        }
        else
        {
            _rightHandName.text = _playerWeapons.RightHand.ToString();
            _rightHandTier.text = $"Tier {_playerWeapons.WeaponTiers[(int)_playerWeapons.RightHand] + 1}";
        }
        if (_playerWeapons.LeftHand == WeaponTypes.None)
        {
            _leftHandName.text = string.Empty;
            _leftHandTier.text = string.Empty;
        }
        else
        {
            _leftHandName.text = _playerWeapons.LeftHand.ToString();
            _leftHandTier.text = $"Tier {_playerWeapons.WeaponTiers[(int)_playerWeapons.LeftHand] + 1}";
        }
    }

    private void UpdateWeaponArts()
    {
        // Right Hand
        for (int i = 0; i < _rightHandWeaponArts.Length; i++)
        {
            if (_playerWeapons.RightHandArts == null) // no weapon
            {
                _rightHandWeaponArts[i].SetLocked(_lock);
            }
            else if (i >= _playerWeapons.RightHandArts.Length)
            {
                _rightHandWeaponArts[i].SetLocked(_lock);
            }
            // This will need to become a specific UI element to handle tooltips, drag and drop, etc.
            else if (_playerWeapons.RightHandArts[i] != null)
            {
                _rightHandWeaponArts[i].SetValue(this, Hand.Right, i, _playerWeapons.RightHandArts[i]);
            }
            else
            {
                _rightHandWeaponArts[i].SetValue(this, Hand.Right, i, null);
            }
        }

        // Left Hand
        for (int i = 0; i < _leftHandWeaponArts.Length; i++)
        {
            if (_playerWeapons.LeftHandArts == null) // no weapon
            {
                _leftHandWeaponArts[i].SetLocked(_lock);
            }
            else if (i >= _playerWeapons.LeftHandArts.Length)
            {
                _leftHandWeaponArts[i].SetLocked(_lock);
            }
            else if (_playerWeapons.LeftHandArts[i] != null)
            {
                _leftHandWeaponArts[i].SetValue(this, Hand.Left, i, _playerWeapons.LeftHandArts[i]);
            }
            else
            {
                _leftHandWeaponArts[i].SetValue(this, Hand.Left, i, null);
            }
        }
    }

    private void UpdateWeaponArtPool()
    {
        // Clear parent pools
        for (int i = _rightHandPoolParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_rightHandPoolParent.GetChild(i).gameObject);
        }
        for (int i = _leftHandPoolParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_leftHandPoolParent.GetChild(i).gameObject);
        }

        // Spawn in new element for each art, set parent and values
        if (_playerWeapons.RightHand != WeaponTypes.None)
        {
            // Get full list
            var newList = new List<WeaponArt>();
            newList.AddRange(_playerWeapons.KnownWeaponArts[_playerWeapons.RightHand]);
            // Remove already-equipped arts
            for (int i = 0; i < _playerWeapons.RightHandArts.Length; i++)
            {
                newList.Remove(_playerWeapons.RightHandArts[i]);
            }

            for (int i = 0; i < newList.Count; i++)
            {
                var element = Instantiate(_elementPrefab, _rightHandPoolParent);
                element.SetValue(this, newList[i], _canvas);
            }
        }

        if (_playerWeapons.LeftHand != WeaponTypes.None)
        {
            var newList = new List<WeaponArt>();
            newList.AddRange(_playerWeapons.KnownWeaponArts[_playerWeapons.LeftHand]);
            for (int i = 0; i < _playerWeapons.LeftHandArts.Length; i++)
            {
                newList.Remove(_playerWeapons.LeftHandArts[i]);
            }

            for (int i = 0; i < newList.Count; i++)
            {
                var element = Instantiate(_elementPrefab, _leftHandPoolParent);
                element.SetValue(this, newList[i], _canvas);
            }
        }
    }

    private void OnCancelSelection()
    {

    }

    internal void OnTryEquipArt(ActiveWeaponArtElement element, WeaponArt newArt)
    {
        _playerWeapons.SetArt(newArt, element.Hand, element.Index);
        UpdateWeapons();
    }

    #region - Tooltip -
    public void ShowTooltip(WeaponArt art)
    {
        _tooltipHeader.text = art.name;
        _tooltipText.text = art.Description;
    }

    public void HideTooltip()
    {
        _tooltipHeader.text = string.Empty;
        _tooltipText.text = string.Empty;
    }
    #endregion
}
