using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Sword/Cleave")]
    public class Cleave : WeaponArt
    {
        private const int RANGE = 1;
        private const int DAMAGE_MOD = 3;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                int dmg = DAMAGE_MOD * combatant.GetAttributeBonus(Attributes.Physicality);

                var nodes = Pathfinding.instance.GetArea(combatant.Node, RANGE);
                foreach (var areaNode in nodes)
                {
                    if (CombatManager.Instance.CheckNode(areaNode, out var nextTarget))
                    {
                        if (nextTarget.IsPlayer != combatant.IsPlayer)
                        {
                            Debug.Log($"Dealing {dmg} Cleave dmg to {nextTarget.gameObject.name}");
                            combatant.DealDamage(dmg, nextTarget);
                        }
                    }
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}