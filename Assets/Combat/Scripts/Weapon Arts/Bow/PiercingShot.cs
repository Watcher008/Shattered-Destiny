using UnityEngine;
using SD.Grids;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Piercing Shot")]
    public class PiercingShot : WeaponArt
    {
        private const int MODIFIER = 8;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            int dmg = combatant.GetAttributeBonus(Characters.Attributes.Physicality) * MODIFIER;

            var points = Bresenham.PlotLine(combatant.Node.X, combatant.Node.Y, node.X, node.Y);
            var nodes = Pathfinding.instance.ConvertToNodes(points);
            nodes.Remove(combatant.Node);

            foreach (var newNode in nodes)
            {
                CombatManager.Instance.CheckNode(newNode, out var target);
                // Ignore empty nodes, allies can be hit
                if (target == null) continue;// || target.IsPlayer == combatant.IsPlayer) continue;

                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    combatant.DealDamage(dmg, target);
                    dmg = Mathf.RoundToInt(dmg * 0.8f);

                    if (dmg <= 0) break; // not needed but pointless to continue
                }
            }
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}