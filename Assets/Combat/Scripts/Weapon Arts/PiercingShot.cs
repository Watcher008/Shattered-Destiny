using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Piercing Shot")]
    public class PiercingShot : WeaponArt
    {
        public override void OnUse(Combatant combatant, Combatant target)
        {
            float missChance = 0.2f;

            int dmg = combatant.GetAttributeBonus(Characters.Attributes.Physicality);
            if (dmg == 0) dmg = 1;
            dmg *= 8;

            // Get all targets in line
            List<Combatant> targetList = new List<Combatant>();

            if (combatant.Node.X == target.Node.X) // Up or Down
            {
                if (combatant.Node.Y < target.Node.Y) // Up
                {
                    for (int i = combatant.Node.Y + 1; i < CombatManager.GRID_SIZE; i++)
                    {
                        var node = Pathfinding.instance.GetNode(combatant.Node.X, i);
                        if (CombatManager.Instance.CheckNode(node, out var newTarget))
                        {
                            targetList.Add(newTarget);
                        }
                    }    
                }
                else // Down
                {
                    for (int i = combatant.Node.Y - 1; i >= 0; i--)
                    {
                        var node = Pathfinding.instance.GetNode(combatant.Node.X, i);
                        if (CombatManager.Instance.CheckNode(node, out var newTarget))
                        {
                            targetList.Add(newTarget);
                        }
                    }
                }
            }
            else if (combatant.Node.Y == target.Node.Y) // Left or right
            {
                if (combatant.Node.X < target.Node.X) // Right
                {
                    for (int i = combatant.Node.X + 1; i < CombatManager.GRID_SIZE; i++)
                    {
                        var node = Pathfinding.instance.GetNode(i, combatant.Node.Y);
                        if (CombatManager.Instance.CheckNode(node, out var newTarget))
                        {
                            targetList.Add(newTarget);
                        }
                    }
                }
                else // Left
                {
                    for (int i = combatant.Node.X - 1; i >= 0; i--)
                    {
                        var node = Pathfinding.instance.GetNode(i, combatant.Node.Y);
                        if (CombatManager.Instance.CheckNode(node, out var newTarget))
                        {
                            targetList.Add(newTarget);
                        }
                    }
                }
            }

            // Reverse because I need to iterate backwards but they should hit in order
            targetList.Reverse();

            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (Random.value <= missChance) continue;

                combatant.DealDamage(dmg, targetList[i]);
                dmg = Mathf.RoundToInt(dmg * 0.8f);
                if (dmg <= 0) break;
            }

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}