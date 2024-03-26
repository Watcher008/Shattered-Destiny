using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatPortrait : MonoBehaviour
{
    private const float DESTROY_DELAY = 2.0f;

    private Combatant _combatant;

    [SerializeField] private Image _frame; // to indicate current turn
    [SerializeField] private Image _background; // to indicate hover target
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _image;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _actionBar;

    private TMP_Text _healthText, _actionText;

    public void SetCombatant(Combatant combatant)
    {
        _combatant = combatant;
        _combatant.onTurnStart += OnTurnStart;
        _combatant.onTurnEnd += OnTurnEnd;

        _combatant.onHealthChange += OnHealthChange;
        _combatant.onActionPointChange += OnActionPointChange;

        _combatant.onMouseEnter += OnHoverEnter;
        _combatant.onMouseExit += OnHoverExit;

        // later include portrait, health, ap, etc.
        _nameText.text = combatant.gameObject.name;

        _healthText = _healthBar.GetComponentInChildren<TMP_Text>();
        _actionText = _actionBar.GetComponentInChildren<TMP_Text>();

        OnHealthChange();
        OnActionPointChange();
    }

    private void OnDestroy()
    {
        _combatant.onTurnStart -= OnTurnStart;
        _combatant.onTurnEnd -= OnTurnEnd;

        _combatant.onHealthChange -= OnHealthChange;
        _combatant.onActionPointChange -= OnActionPointChange;

        _combatant.onMouseEnter -= OnHoverEnter;
        _combatant.onMouseExit -= OnHoverExit;
    }

    private void OnHealthChange()
    {
        _healthBar.fillAmount = (float)_combatant.Health / _combatant.MaxHealth;
        _healthText.text = $"{_combatant.Health}/{_combatant.MaxHealth}";

        if (_combatant.Health <= 0) Destroy(gameObject, DESTROY_DELAY);
    }

    private void OnActionPointChange()
    {
        _actionBar.fillAmount = (float)_combatant.ActionPoints / _combatant.MaxActionPoints;
        _actionText.text = $"{_combatant.ActionPoints}/{_combatant.MaxActionPoints}";
    }

    private void OnHoverEnter()
    {
        _background.color = Color.yellow;
    }

    private void OnHoverExit()
    {
        _background.color = Color.white;
    }

    private void OnTurnStart()
    {
        _frame.color = Color.yellow;
    }

    private void OnTurnEnd()
    {
        _frame.color = Color.white;
    }
}
