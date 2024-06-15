using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SD.Grids;
using SD.Characters;
using SD.Combat.WeaponArts;
using SD.Combat.Effects;

namespace SD.Combat
{
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

        public void OnMouseEnter() => onMouseEnter?.Invoke();
        public void OnMouseExit() => onMouseExit?.Invoke();
        #endregion

        #region - General -
        public bool PlayerControlled = false;
        private bool _isPlayer;
        public bool IsPlayer
        {
            get
            {
                if (HasEffect(StatusEffects.CHARMED)) return !_isPlayer;
                return _isPlayer;
            }
        }

        private CharacterSheet _characterSheet;
        private StatBlock _statBlock;
        private WeaponTypes _weapon;

        private List<WeaponArt> _weaponArts = new();
        public List<WeaponArt> WeaponArts => _weaponArts;

        private byte[] _activeEffects = new byte[(int)StatusEffects.count];


        private PathNode _currentNode;
        public PathNode Node => _currentNode;
        public PathNode GetNode() => _currentNode;

        // Prevents further input until current action has resolved
        public bool IsActing { get; private set; }
        // Prevents the use of the Rest action if they've taken any other action during their turn
        public bool CanRest { get; private set; }

        private Coroutine _movementCoroutine;
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
        public int AttackRange
        {
            get
            {
                if (_weapon == WeaponTypes.Bow || _weapon == WeaponTypes.Staff) return 10;
                return 1;
            }
        }

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

        public int Block;
        #endregion

        #region - Init -
        public void SetInitialValues(StatBlock stats)
        {
            gameObject.name = stats.Name;

            _isPlayer = false;
            _statBlock = stats;
            _weapon = _statBlock.Weapon;

            Health = MaxHealth;
            ActionPoints = stats.SAP;

            GetComponentInChildren<SpriteRenderer>().sprite = stats.Sprite;
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
        #endregion

        public void OnTurnStart(bool regainAP)
        {
            // Do not regain AP on first round
            if (!HasEffect(StatusEffects.STUNNED) && regainAP)
            {
                // Regain action points, up to max
                ActionPoints = Mathf.Clamp(ActionPoints + _refreshedActionPoints, 0, MaxActionPoints);
                onActionPointChange?.Invoke();
            }

            if (HasEffect(StatusEffects.CURSED))
            {
                int index = UnityEngine.Random.Range(0, (int)StatusEffects.CURSED);
                AddEffect((StatusEffects)index);
            }

            // Regain all movement
            MovementRemaining = _movement;
            if (HasEffect(StatusEffects.HURRIED)) MovementRemaining += _movement; // 2x if Hurried

            CanRest = true;
            Block = 0;

            for (int i = 0; i < _activeEffects.Length; i++)
            {
                if (_activeEffects[i] > 0)
                {
                    _activeEffects[i]--;
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

        #region - Status Effects -
        public bool HasEffect(StatusEffects effect)
        {
            return _activeEffects[(int)effect] > 0;
        }

        public void AddEffect(StatusEffects effect, byte duration = 1)
        {
            if (effect is StatusEffects.DAZED)
            {
                ActionPoints = 0;
            }
            else if (_activeEffects[(int)effect] < duration)
            {
                _activeEffects[(int)effect] = duration;
            }
        }
        #endregion

        #region - Health -
        public void TakeDamage(int damage)
        {
            float damageMultiplier = 1.0f;
            if (HasEffect(StatusEffects.VULNERABLE)) damageMultiplier += 0.25f;
            if (HasEffect(StatusEffects.REINFORCED)) damageMultiplier -= 0.25f;
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

        #region - Movement -
        public void Move(List<PathNode> path)
        {
            if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(FollowPath(path));
        }

        /// <summary>
        /// Forces the unit to the given node. Does not cost Movement.
        /// </summary>
        public void ForceMove(PathNode to)
        {
            if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(MoveDirect(to));
        }

        private IEnumerator FollowPath(List<PathNode> path)
        {
            IsActing = true;
            while (path.Count > 0)
            {
                var next = path[0];
                path.RemoveAt(0);

                var moveCost = 1 + next.MovementCost;
                if (HasEffect(StatusEffects.SLOWED)) moveCost *= 2;
                // This shouldn't happen, but let's just make sure
                if (MovementRemaining < moveCost) break;

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

                MovementRemaining -= moveCost;
                yield return null;
            }
            IsActing = false;
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
        #endregion

        #region - Actions - 
        public void OnRest()
        {
            // Take no action, regain AP, and end turn.
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

        public void PerformBasicAttack(IDamageable target)
        {
            Debug.Log("add func for getting name");
            //CombatLog.Log($"{gameObject.name} attacks {target.gameObject.name}.");
            StartCoroutine(Stab(target.GetNode()));

            float dmg = 0;
            switch (_weapon)
            {
                case WeaponTypes.Sword:
                    dmg = 1.0f * GetAttributeBonus(Attributes.Physicality);
                    break;
                case WeaponTypes.Shield:
                    dmg = 0.5f * GetAttributeBonus(Attributes.Physicality);
                    break;
                case WeaponTypes.Warhammer:
                    dmg = 2.0f * GetAttributeBonus(Attributes.Physicality);
                    break;
                case WeaponTypes.Bow:
                    dmg = 1.0f * GetAttributeBonus(Attributes.Physicality);
                    break;
                case WeaponTypes.Staff:
                    dmg = 1.5f * GetAttributeBonus(Attributes.Intelligence);
                    break;
                case WeaponTypes.Book:
                    dmg = 0.5f * GetAttributeBonus(Attributes.Intelligence);
                    break;
                default:
                    dmg = 1.0f * GetAttributeBonus(Attributes.Physicality);
                    break;
            }

            DealDamage(Mathf.RoundToInt(dmg), target);
            SpendActionPoints();
        }

        public void DealDamage(int dmg, IDamageable target)
        {
            if (target is Combatant combatant)
            {
                // I don't know why I was checking this...
                //Debug.Log("Is combatant");
            }

            float damageMultiplier = 1.0f;
            if (HasEffect(StatusEffects.WEAKENED)) damageMultiplier -= 0.25f;
            if (HasEffect(StatusEffects.EMPOWERED)) damageMultiplier += 0.25f;

            if (CombatManager.Instance.NodeHasEffect<Effect_Barrier>(Node, out var areaEffect))
            {
                // +50% bonus if targeting a unit outside the effect
                if (!areaEffect.Area.Contains(target.GetNode())) damageMultiplier += 0.5f;
            }

            dmg = Mathf.RoundToInt(dmg * damageMultiplier);
            target.TakeDamage(dmg);
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

        public int GetAttributeBonus(Attributes attribute)
        {
            if (_characterSheet != null) return _characterSheet.GetAttributeBonus(attribute);
            return _statBlock.GetAttributeBonus(attribute);
        }

        public int CompareTo(Combatant other)
        {
            // Compares Combatants based on initiative to be placed in battle order.
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
    }
}