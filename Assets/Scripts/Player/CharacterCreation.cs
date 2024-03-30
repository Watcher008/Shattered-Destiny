using SD.Characters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    [Space]

    [SerializeField] private TMP_InputField _nameInput;

    [SerializeField] private Image _playerModel;
    [SerializeField] private Button _cycleLeftButton;
    [SerializeField] private Button _cycleRightButton;
    [SerializeField] private Button _finishButton;

    [Header("Background")]
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private Toggle _prefab;

    private int _spriteIndex;
    private Sprite[] _sprites;
    private readonly string[] _spritePaths =
    {
        "creatures/base",
        "creatures/knight",
        "creatures/archer",
        "creatures/barbarian",
        "creatures/wizard",
        "creatures/cleric",
    };

    private readonly Dictionary<string, Vector2Int> _testBackgrounds = new()
    {
        {"Background 1", new Vector2Int(75, 26)},
        {"Background 2", new Vector2Int(78, 29) }
    };

    private void Awake()
    {
        _sprites = new Sprite[_spritePaths.Length];
        for (int i = 0; i < _spritePaths.Length; i++)
        {
            _sprites[i] = SpriteHelper.GetSprite(_spritePaths[i]);
        }

        _playerModel.sprite = _sprites[0];

        _cycleLeftButton.onClick.AddListener(CycleSpriteLeft);
        _cycleRightButton.onClick.AddListener(CycleSpriteRight);
        _finishButton.onClick.AddListener(OnComplete);

        AddBackgrounds();
    }

    private void OnDestroy()
    {
        _cycleLeftButton.onClick.RemoveAllListeners();
        _cycleRightButton.onClick.RemoveAllListeners();
        _finishButton.onClick.RemoveAllListeners();
    }

    private void AddBackgrounds()
    {
        for (int i = _toggleGroup.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_toggleGroup.transform.GetChild(i).gameObject);
        }

        foreach(var pair in _testBackgrounds)
        {
            var toggle = Instantiate(_prefab, _toggleGroup.transform);
            toggle.GetComponentInChildren<TMP_Text>().text = pair.Key;
            toggle.group = _toggleGroup;
        }
    }

    private void CycleSpriteLeft()
    {
        _spriteIndex--;
        if (_spriteIndex < 0) _spriteIndex = _sprites.Length - 1;
        _playerModel.sprite = _sprites[_spriteIndex];
    }

    private void CycleSpriteRight()
    {
        _spriteIndex++;
        if (_spriteIndex >= _sprites.Length) _spriteIndex = 0;
        _playerModel.sprite = _sprites[_spriteIndex];
    }

    private void OnComplete()
    {
        // Save player name to PlayerData
        _playerData.Name = _nameInput.text;
        // Save player sprite to PlayerData
        _playerData.Sprite = _sprites[_spriteIndex];

        // Set world position based on background
        // This has got to be the worst possible way to do this
        var background = _toggleGroup.GetFirstActiveToggle();
        var s = background.GetComponentInChildren<TMP_Text>().text;
        _playerData.WorldPos = _testBackgrounds[s];

        // Load World Map
    }
}
