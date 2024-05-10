using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Sword/Cleave")]
    public class Cleave : WeaponArt
    {
        private const int RANGE = 1;
        private const int DAMAGE_MOD = 3;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out IDamageable target))
            {
                int dmg = DAMAGE_MOD * combatant.GetAttributeBonus(Attributes.Physicality);

                var nodes = Pathfinding.GetArea(combatant.Node, RANGE);
                foreach (var areaNode in nodes)
                {
                    if (CombatManager.Instance.CheckNode(areaNode, out IDamageable nextTarget))
                    {
                        if (nextTarget is Combatant unit && unit.IsPlayer == combatant.IsPlayer) continue;

                        combatant.DealDamage(dmg, nextTarget);
                    }
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}