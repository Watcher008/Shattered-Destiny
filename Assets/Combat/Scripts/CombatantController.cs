using UnityEngine;
using SD.Grids;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

/* I will likely need to implement a Blackboard system to handle group strategy logic
 * Most units will just favor straight damage-dealing so that's no major issue
 * Alternatively I could just handle the logic based on an assigned role of DPS or Support
 * DPS just goes straight for damage, while Support will look to aid allies first before defaulting to DPS
 */

namespace SD.Combat
{
    /// <summary>
    /// A class that handles enemy AI logic during combat.
    /// </summary>
    public class CombatantController : MonoBehaviour
    {
        private Combatant _combatant;
        private Coroutine _waitCoroutine;

        private Occupant _allies
        {
            get
            {
                if (_combatant.IsPlayer) return Occupant.Player;
                return Occupant.Enemy;
            }
        }
        private List<Combatant> Adversaries
        {
            get
            {
                if (_combatant.IsPlayer) return CombatManager.Instance.EnemyCombatants;
                return CombatManager.Instance.PlayerCombatants;
            }
        }

        private void Awake()
        {
            _combatant = GetComponent<Combatant>();
            _combatant.onTurnStart += OnTurnStart;
        }

        private void OnDestroy()
        {
            _combatant.onTurnStart -= OnTurnStart;
        }

        private void OnTurnStart() => EvaluateActions();


        // Just trying to clean up the EvaluateAction function
        private void Foo()
        {
            if (_combatant.ActionPoints == 0 && _combatant.CanRest) _combatant.OnRest();
            else if (FindNearest(out var nearest))
            {
                if (_combatant.ActionPoints == 0)
                {
                    // Cannot move
                    if (_combatant.MovementRemaining == 0) CombatManager.Instance.EndTurn(_combatant);
                    // Am I in range to attack? Then there's no need to move
                    else if (WithinAttackRange(nearest)) CombatManager.Instance.EndTurn(_combatant);
                    // Else, can I move towards the target with remaining movement?
                    // If unit can move, will do so and then re-evaluate
                    else if (!TryMove(nearest)) CombatManager.Instance.EndTurn(_combatant);
                }
                else // Unit DOES have AP to spend
                {
                    // Am I within range to attack? Attack
                    if (TryAttack(nearest)) return;
                    // I'm not in range, do I have Movement to get there? Then get there
                    else if (TryMove(nearest)) return;
                    // Only Sprint if actually able to do something once reaching destination
                    else if (_combatant.ActionPoints > 1)
                    {
                        _combatant.OnSprint();
                        EvaluateActions();
                    }
                    // The unit is not within range to attack, and cannot Move without using last AP to sprint
                    else CombatManager.Instance.EndTurn(_combatant);
                }
            }
            else CombatManager.Instance.EndTurn(_combatant); // all opponents defeated
        }

        private void EvaluateActions()
        {
            // This should only happen if unit was dazed
            if (_combatant.ActionPoints == 0 && _combatant.CanRest)
            {
                _combatant.OnRest();
                return;
            }

            // Find nearest target
            var target = FindNearest();
            if (target == null)
            {
                CombatManager.Instance.EndTurn(_combatant); // all opponents defeated
                return;
            }

            // Only available actions is to Move
            if (_combatant.ActionPoints == 0)
            {
                // Cannot move
                if (_combatant.MovementRemaining == 0)
                {
                    CombatManager.Instance.EndTurn(_combatant);
                    return;
                }

                // Am I in range to attack? Then there's no need to move
                if (WithinAttackRange(target))
                {
                    CombatManager.Instance.EndTurn(_combatant);
                    return;
                }

                // Else, can I move towards the target with remaining movement?
                // If unit can move, will do so and then re-evaluate
                if (!TryMove(target))
                {
                    CombatManager.Instance.EndTurn(_combatant);
                    return;
                }
            }
            else // Unit DOES have AP to spend
            {
                // Am I within range to attack? Attack
                if (WithinAttackRange(target) && TryAttack(target)) return;
                // I'm not in range, do I have Movement to get there? Then get there
                else if (TryMove(target)) return;
                // Only Sprint if actually able to do something once reaching destination
                else if (_combatant.ActionPoints > 1)
                {
                    _combatant.OnSprint();
                    EvaluateActions();
                }
                else
                {
                    // The unit is not within range to attack, and cannot Move without using last AP to sprint
                    CombatManager.Instance.EndTurn(_combatant);
                }
            }
        }

        /// <summary>
        /// Returns the nearest player combatant to the enemy.
        /// </summary>
        private Combatant FindNearest()
        {
            int minDist = int.MaxValue;
            if (Adversaries.Count == 0) return null;            

            Combatant nearest = Adversaries[0];

            foreach (var adversary in Adversaries)
            {
                var dist = Pathfinding.GetNodeDistance(_combatant.Node, adversary.Node);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = adversary;
                }
            }
            return nearest;
        }

        private bool FindNearest(out Combatant nearest)
        {
            nearest = null;
            int minDist = int.MaxValue;
            if (Adversaries.Count == 0) return false;

            foreach (var adversary in Adversaries)
            {
                var dist = Pathfinding.GetNodeDistance(_combatant.Node, adversary.Node);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = adversary;
                }
            }
            return nearest;
        }

        /// <summary>
        /// Returns true if the target is within range of basic attack or any weapon art.
        /// </summary>
        private bool WithinAttackRange(Combatant target)
        {
            if (target == null) return false;
            var dist = Pathfinding.GetNodeDistance(_combatant.Node, target.Node);

            foreach (var art in _combatant.WeaponArts)
            {
                // Will need to add a variable to arts for if they are offensive or defensive
                if (dist <= art.Range) return true;
            }

            return (Pathfinding.GetNodeDistance(_combatant.Node, target.Node) <= _combatant.AttackRange);
        }

        private bool TryAttack(Combatant target)
        {
            int dist = Pathfinding.GetNodeDistance(_combatant.Node, target.Node);
            foreach(var art in _combatant.WeaponArts)
            {
                if (_combatant.ActionPoints < art.Cost) continue; // Not enough AP
                if (dist > art.Range) continue; // Out of range

                art.OnUse(_combatant, target.Node);

                if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
                _waitCoroutine = StartCoroutine(WaitToAct());

                return true;
            }
            // Cannot use any Weapon Arts, check Basic Attack
            if (dist <= _combatant.AttackRange)
            {
                _combatant.Attack(target);

                if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
                _waitCoroutine = StartCoroutine(WaitToAct());

                return true;
            }
            return false;
        }

        private bool TryMove(Combatant target)
        {
            // Find the initial path
            var path = Pathfinding.FindNodePath(_combatant.Node, target.Node, true, _allies);
            if (path == null) return false;
            if (path[0] == _combatant.Node) path.RemoveAt(0);

            // Reduce path based on current remaining movement
            while(Pathfinding.GetPathCost(path) > _combatant.MovementRemaining) path.RemoveAt(path.Count - 1);
            // There is no valid node that the unit can move into
            if (path.Count == 0) return false;

            // Remove from end of path until the last tile is not occupied
            while (path[path.Count - 1].Occupant != Occupant.None)
            {
                path.RemoveAt(path.Count - 1);
                if (path.Count == 0) return false;
            }

            _combatant.Move(path);

            if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
            _waitCoroutine = StartCoroutine(WaitToAct(false));
            return true;
        }

        /// <summary>
        /// Wait for the combatant to finish acting and then re-evaluate options.
        /// </summary>
        private IEnumerator WaitToAct(bool addDelay = true)
        {
            while (_combatant.IsActing) yield return null;
            if (addDelay) yield return new WaitForSeconds(0.5f); // Slight delay between actions
            EvaluateActions();
        }
    }
}