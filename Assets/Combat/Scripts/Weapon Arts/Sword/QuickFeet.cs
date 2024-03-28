using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Sword/Quick Feet")]
    public class QuickFeet : WeaponArt
    {
        private const int DAMAGE_MOD = 5;
        private const int RANGE = 3;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (node.Occupant != Occupant.None) return; // Need to have selected an empty space

            combatant.ForceMove(node);

            // Tells the CombatManager to wait until unit has stopped moving to call OnComplete
            CombatManager.Instance.DelayWeaponArt(this, combatant);
            
            combatant.SpendActionPoints(_actionPointCost);
        }

        public override void OnComplete(Combatant combatant)
        {
            int dmg = DAMAGE_MOD * combatant.GetAttributeBonus(Attributes.Physicality);

            var nodes = Pathfinding.GetArea(combatant.Node, RANGE);
            foreach (var areaNode in nodes)
            {
                if (CombatManager.Instance.CheckNode(areaNode, out var nextTarget))
                {
                    if (nextTarget.IsPlayer != combatant.IsPlayer)
                    {
                        combatant.DealDamage(dmg, nextTarget);
                    }
                }
            }
        }
    }
}