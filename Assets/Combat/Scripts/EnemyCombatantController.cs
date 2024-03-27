using UnityEngine;
using SD.Grids;
using System.Collections;

namespace SD.Combat
{
    /// <summary>
    /// A class that handles enemy AI logic during combat.
    /// </summary>
    public class EnemyCombatantController : MonoBehaviour
    {
        private Combatant _combatant;

        private Coroutine _movementCoroutine;

        private void Awake()
        {
            _combatant = GetComponent<Combatant>();
            _combatant.onTurnStart += OnTurnStart;
        }

        private void OnTurnStart()
        {
            // Find nearest target
            var target = FindNearest();

            // Attack if within range
            if (TryAttack(target)) return;

            // Else move closer and then check again
            else if (TryMove(target)) return;

            // Else just wait
            else _combatant.OnRest();
        }

        /// <summary>
        /// Returns the nearest player combatant to the enemy.
        /// </summary>
        private Combatant FindNearest()
        {
            int minDist = int.MaxValue;
            Combatant nearest = CombatManager.Instance.PlayerCombatants[0];

            foreach (var player in CombatManager.Instance.PlayerCombatants)
            {
                var dist = Pathfinding.instance.GetNodeDistance_Path(_combatant.Node, player.Node);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = player;
                }
            }
            return nearest;
        }

        private bool TryAttack(Combatant target)
        {
            if (Pathfinding.instance.GetNodeDistance_Path(_combatant.Node, target.Node) <= _combatant.AttackRange)
            {
                _combatant.Attack(target);
                return true;
            }
            return false;
        }

        private bool TryMove(Combatant target)
        {
            var path = Pathfinding.instance.FindNodePath(_combatant.Node, target.Node, true, Occupant.Enemy);
            if (path == null) return false;

            if (path[0] == _combatant.Node) path.RemoveAt(0);

            // Reduce path down the number of movable tiles
            while (path.Count > _combatant.MovementRemaining) path.RemoveAt(path.Count - 1);

            // Don't let them walk onto the player tile
            if (path[path.Count - 1].Occupant != Occupant.None) path.RemoveAt(path.Count - 1);

            _combatant.Move(path);

            if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(WaitToMove(target));

            return true;
        }

        /// <summary>
        /// Wait for the combatant to finish moving and then check if within range to attack.
        /// </summary>
        private IEnumerator WaitToMove(Combatant target)
        {
            while (_combatant.IsActing) yield return null;

            // Try to attack
            if (TryAttack(target)) yield break;

            // Else just end turn
            else _combatant.OnRest();
        }
    }
}