using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Piercing Shot")]
    public class PiercingShot : WeaponArt
    {
        private const int MODIFIER = 8;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            int dmg = combatant.GetAttributeBonus(Characters.Attributes.Physicality) * MODIFIER;

            var points = Bresenham.PlotLine(combatant.Node.X, combatant.Node.Y, node.X, node.Y);
            var nodes = Pathfinding.ConvertToNodes(node.grid, points);
            nodes.Remove(combatant.Node);

            foreach (var newNode in nodes)
            {
                CombatManager.Instance.CheckNode(newNode, out IDamageable target);
                
                if (target == null) continue;// Ignore empty nodes, allies can be hit

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