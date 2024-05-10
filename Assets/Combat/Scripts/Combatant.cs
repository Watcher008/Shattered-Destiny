using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SD.Grids;
using SD.Characters;
using SD.Combat;
using SD.Combat.WeaponArts;
using SD.Combat.Effects;
using UnityEditor.Experimental.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Combatant : MonoBehaviour, IComparable<Combatant>, IDamageable
{
    #region - Callback Events
    public delegate void OnTurnChanged();
    public OnTurnChanged onTurnStart;
    public OnTurnChanged onTurnEnd;

    public delegate void OnInteraction();
    public OnInteraction onMouseEnter;
    public OnInteraction onMouseExit;

    public delegate void OnStatChange();
    public OnStatChange onHealthChange;
    public OnStatChange onActionPointChange;
    #endregion

    public bool PlayerControlled = false;

    #region - General -
    private bool _isPlayer;
    private PathNode _currentNode;

    private CharacterSheet _characterSheet;
    private StatBlock _statBlock;
    private Weapon _weapon;

    private List<WeaponArt> _weaponArts = new();
    private List<ActiveStatusEffect> _activeEffects = new();

    public bool IsPlayer
    {
        get
        {
            if (HasEffect<Charm>()) return !_isPlayer;

            return _isPlayer;
        }
    }
    public PathNode Node => _currentNode;
    public PathNode GetNode() => _currentNode;
    public List<WeaponArt> WeaponArts => _weaponArts;

    // Prevents further input until current action has resolved
    public bool IsActing { get; private set; }
    // Prevents the use of the Rest action if they've taken any other action during their turn
    public bool CanRest { get; private set; }
    #endregion

    #region - Stats -
    private int _initiative;
    private int _initiativeBonus
    {
        get
        {
             return _characterSheet != null ? _characterSheet.Initiative : _statBlock.Initiative;
        }
    }

    // How many spaces more the unit can move this turn
    public int MovementRemaining { get; private set; }
    private int _movement => _characterSheet != null ? _characterSheet.Movement : _statBlock.Movement;
    public int AttackRange => _weapon == null ? 1 : _weapon.Range;

    public int Health { get; private set; }
    public int MaxHealth => _characterSheet != null ? _characterSheet.MaxHealth : _statBlock.MaxHealth;

    // Action Points
    public int ActionPoints { get; private set; }
    public int MaxActionPoints
    {
        get
        {
            return _characterSheet != null ? _characterSheet.MaxActionPoints : _statBlock.MAP;
        }
    }
    private int _refreshedActionPoints
    {
        get
        {
            return _characterSheet != null ? _characterSheet.RefreshActionPoints : _statBlock.RAP;
        }
    }
    #endregion

    public int Block;

    private Coroutine _movementCoroutine;

    public void OnMouseEnter() => onMouseEnter?.Invoke();
    public void OnMouseExit() => onMouseExit?.Invoke();

    public void SetInitialValues(StatBlock stats, Weapon weapon)
    {
        gameObject.name = stats.Name;

        _isPlayer = false;
        _statBlock = stats;
        _weapon = weapon;

        Health = MaxHealth;
        ActionPoints = stats.SAP;

        GetComponentInChildren<SpriteRenderer>().sprite = SpriteHelper.GetSprite(stats.Sprite);
        gameObject.AddComponent<CombatantController>();

        _initiative = UnityEngine.Random.Range(1, 7) + _initiativeBonus;
    }

    public void SetInitialValues(Sprite sprite, CharacterSheet stats)
    {
        _isPlayer = true;
        _characterSheet = stats;
        _weapon = stats.Weapon;
        _weaponArts.AddRange(stats.WeaponArts);

        Health = stats.Health;
        ActionPoints = stats.StartingActionPoints;

        GetComponentInChildren<SpriteRenderer>().sprite = sprite;

        _initiative = UnityEngine.Random.Range(1, 7) + _initiativeBonus;
    }

    /// <summary>
    /// Sets the initial position of the unit.
    /// </summary>
    public void SetNode(PathNode node)
    {
        _currentNode = node;
        if (_isPlayer) _currentNode.SetOccupant(Occupant.Player);
        else _currentNode.SetOccupant(Occupant.Enemy);
        transform.position = new Vector3(node.X, 0, node.Y);
    }

    public void OnTurnStart(bool regainAP)
    {
        // Do not regain AP on first round
        if (!HasEffect<Stun>() && regainAP)
        {
            // Regain action points, up to max
            ActionPoints = Mathf.Clamp(ActionPoints + _refreshedActionPoints, 0, MaxActionPoints);
            onActionPointChange?.Invoke();
        }

        // Regain all movement
        MovementRemaining = _movement;
        if (HasEffect<Effect_Hurried>()) MovementRemaining += _movement; // 2x if Hurried

        CanRest = true;
        Block = 0;

        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            _activeEffects[i].Duration--;
            if (_activeEffects[i].Duration <= 0)
            {
                _activeEffects.RemoveAt(i);
            }
        }
       
        onTurnStart?.Invoke();
    }

    public void SpendActionPoints(int points = 1)
    {
        ActionPoints -= points;
        CanRest = false;
        onActionPointChange?.Invoke();
    }

    public bool HasEffect<T>() where T : StatusEffects
    {
        foreach(var item in _activeEffects)
        {
            if (item.Effect is T) return true;
        }
        return false;
    }

    public void AddEffect(StatusEffects effect, int duration = 1)
    {
        if (effect is Daze)
        {
            ActionPoints = 0;
            return;
        }
        
        foreach(var item in _activeEffects)
        {
            if(item.Effect == effect)
            {
                // If higher, just set duration to new value
                if (item.Duration < duration) item.Duration = duration;
                return;
            }
        }       

        _activeEffects.Add(new ActiveStatusEffect(effect, duration));
    }

    #region - Health -
    public void TakeDamage(int damage)
    {
        float damageMultiplier = 1.0f;
        if (HasEffect<Vulnerable>()) damageMultiplier += 0.25f;
        if (HasEffect<Effect_Reinforced>()) damageMultiplier -= 0.25f;
        damage = Mathf.RoundToInt(damage * damageMultiplier);

        damage -= Block;
        if (damage <= 0) return;

        CombatManager.Instance.OnDamageTaken(this);

        _characterSheet?.TakeDamage(damage);

        Health -= damage;
        onHealthChange?.Invoke();
        // flash sprite

        if (Health <= 0) OnDeath();
    }

    public void RestoreHealth(int health)
    {
        if (health <= 0) return;

        Health += health;
        onHealthChange?.Invoke();
    }

    private void OnDeath()
    {
        _currentNode.SetOccupant(Occupant.None);
        CombatManager.Instance.OnCombatantDefeated(this);
    }
    #endregion

    public int GetAttribute(Attributes attribute)
    {
        if (_characterSheet != null) return _characterSheet.GetAttribute(attribute);
        else return _statBlock.Attributes[(int)attribute];
    }

    public int GetAttributeBonus(Attributes attribute)
    {
        if (_characterSheet != null) return _characterSheet.GetAttributeBonus(attribute);
        return _statBlock.GetAttributeBonus(attribute);
    }

    #region - Actions - 
    /// <summary>
    /// Take no action, regain AP, and end turn.
    /// </summary>
    public void OnRest()
    {
        ActionPoints = Mathf.Clamp(ActionPoints + _refreshedActionPoints, 0, MaxActionPoints);
        onActionPointChange?.Invoke();
        CombatManager.Instance.EndTurn(this);
    }

    public void OnSprint()
    {
        if (ActionPoints <= 0) return;

        MovementRemaining += 2;
        SpendActionPoints();
    }

    public void Move(List<PathNode> path)
    {
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        _movementCoroutine = StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<PathNode> path)
    {
        IsActing = true;
        while (path.Count > 0)
        {
            var next = path[0];
            path.RemoveAt(0);
            // This shouldn't happen, but let's just make sure
            if (MovementRemaining < next.MovementCost + 1) break;

            var temp = CombatManager.Instance.GetNodePosition(next.X, next.Y);
            var end = new Vector3(temp.x, 0, temp.y);

            float t = 0f;
            float timeToMove = 0.5f;

            while (t < timeToMove)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, end, t / timeToMove);
                yield return null;
            }

            transform.position = end;

            // Abandon current node
            _currentNode.SetOccupant(Occupant.None);
            _currentNode = next;

            if (_isPlayer) _currentNode.SetOccupant(Occupant.Player);
            else _currentNode.SetOccupant(Occupant.Enemy);

            MovementRemaining -= 1 + _currentNode.MovementCost;
            yield return null;
        }
        IsActing = false;
    }

    /// <summary>
    /// Forces the unit to the given node. Does not cost Movement.
    /// </summary>
    /// <param name="to"></param>
    public void ForceMove(PathNode to)
    {
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        _movementCoroutine = StartCoroutine(MoveDirect(to));
    }

    private IEnumerator MoveDirect(PathNode node)
    {
        IsActing = true;
        // Abandon current node
        _currentNode.SetOccupant(Occupant.None);

        var temp = CombatManager.Instance.GetNodePosition(node.X, node.Y);
        var end = new Vector3(temp.x, 0, temp.y);

        float t = 0f;
        float timeToMove = 0.5f;

        while (t < timeToMove)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, end, t / timeToMove);
            yield return null;
        }

        transform.position = end;
        _currentNode = node;
        //SetNode(node);
        if (_isPlayer) _currentNode.SetOccupant(Occupant.Player);
        else _currentNode.SetOccupant(Occupant.Enemy);
        
        IsActing = false;
    }

    public void Attack(IDamageable target)
    {
        Debug.Log("add func for getting name");
        //CombatLog.Log($"{gameObject.name} attacks {target.gameObject.name}.");
        StartCoroutine(Stab(target.GetNode()));

        // Deal damage to target
        var dmg = _weapon != null ? _weapon.Damage : 1;
        dmg += GetDamageBonus();
        DealDamage(dmg, target);

        SpendActionPoints();
    }

    public void DealDamage(int dmg, IDamageable target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log("Is combatant");
        }
        float damageMultiplier = 1.0f;
        if (HasEffect<Weaken>()) damageMultiplier -= 0.25f;
        if (HasEffect<Effect_Empowered>()) damageMultiplier += 0.25f;

        if (CombatManager.Instance.NodeHasEffect<Effect_Barrier>(Node, out var areaEffect))
        {
            // +50% bonus if targeting a unit outside the effect
            if (!areaEffect.Area.Contains(target.GetNode())) damageMultiplier += 0.5f;
        }

        dmg = Mathf.RoundToInt(dmg * damageMultiplier);
        target.TakeDamage(dmg);
    }

    public void DealDamage(int dmg, Combatant target)
    {
        float damageMultiplier = 1.0f;
        if (HasEffect<Weaken>()) damageMultiplier -= 0.25f;
        if (HasEffect<Effect_Empowered>()) damageMultiplier += 0.25f;

        if (CombatManager.Instance.NodeHasEffect<Effect_Barrier>(Node, out var areaEffect))
        {
            // +50% bonus if targeting a unit outside the effect
            if (!areaEffect.Area.Contains(target.Node)) damageMultiplier += 0.5f;
        }

        dmg = Mathf.RoundToInt(dmg * damageMultiplier);

        target.TakeDamage(dmg);
    }

    private int GetDamageBonus()
    {
        if (_weapon == null)
        {
            if (_characterSheet != null) return _characterSheet.GetAttributeBonus(Attributes.Physicality);
            else return Mathf.RoundToInt(_statBlock.Attributes[(int)Attributes.Physicality] / 10);
        }
        else
        {
            if (_characterSheet != null) return _characterSheet.GetAttributeBonus(_weapon.Attribute);
            else return Mathf.RoundToInt(_statBlock.Attributes[(int)_weapon.Attribute] / 10);
        }
    }

    private IEnumerator Stab(PathNode node)
    {
        IsActing = true;
        var start = transform.position;
        var pos = new Vector3(node.X, 0, node.Y);
        var end = (transform.position + pos) / 2;
        float t = 0, timeToAct = 0.25f;

        while (t < timeToAct)
        {
            t += Time.deltaTime;

            if (t >= timeToAct / 2) transform.position = Vector3.Lerp(end, start, t / timeToAct);
            else transform.position = Vector3.Lerp(start, end, t / timeToAct);

            yield return null;
        }
        transform.position = start;

        IsActing = false;
    }
    #endregion

    public int CompareTo(Combatant other)
    {
        // -1 places lower in index, 1 places higher in index
        if (_initiative > other._initiative) return -1;
        else if (_initiative < other._initiative) return 1;
        else // Same initiative
        {
            // First compare initiative bonus
            if (_initiativeBonus > other._initiativeBonus) return -1;
            else if (_initiativeBonus < other._initiativeBonus) return 1;

            // Then give tie breaker to players
            if (IsPlayer && !other.IsPlayer) return -1;
            else if (!IsPlayer && other.IsPlayer) return 1;

            return 0; // Then it just doesn't matter
        }
    }

    #region - Obsolete -
    [Obsolete]
    public void Attack(Combatant target)
    {
        CombatLog.Log($"{gameObject.name} attacks {target.gameObject.name}.");
        StartCoroutine(Stab(target));

        // Deal damage to target
        var dmg = _weapon != null ? _weapon.Damage : 1;
        dmg += GetDamageBonus();
        DealDamage(dmg, target);

        SpendActionPoints();
    }

    [Obsolete]
    private IEnumerator Stab(Combatant target)
    {
        IsActing = true;
        var start = transform.position;
        var end = (transform.position + target.transform.position) / 2;
        float t = 0, timeToAct = 0.25f;

        while (t < timeToAct)
        {
            t += Time.deltaTime;

            if (t >= timeToAct / 2) transform.position = Vector3.Lerp(end, start, t / timeToAct);
            else transform.position = Vector3.Lerp(start, end, t / timeToAct);

            yield return null;
        }
        transform.position = start;

        //CombatManager.Instance.EndTurn(this);
        IsActing = false;
    }
    #endregion
}
