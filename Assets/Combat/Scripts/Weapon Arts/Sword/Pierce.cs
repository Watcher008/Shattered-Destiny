using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Sword/Pierce")]
    public class Pierce : WeaponArt
    {
        private const int DAMAGE_MOD = 5;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            int dmg = DAMAGE_MOD * combatant.GetAttributeBonus(Attributes.Physicality);

            var points = Bresenham.PlotLine(combatant.Node.X, combatant.Node.Y, node.X, node.Y);
            var nodes = Pathfinding.instance.ConvertToNodes(points);
            nodes.Remove(combatant.Node);

            foreach (var newNode in nodes)
            {
                CombatManager.Instance.CheckNode(newNode, out var target);
                // Ignore empty nodes and allies
                if (target == null || target.IsPlayer == combatant.IsPlayer) continue;
                combatant.DealDamage(dmg, target);
            }
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}