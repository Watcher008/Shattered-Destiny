using UnityEngine;
using SD.Grids;
using SD.Combat.Effects;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Staff/Barrier")]
    public class Barrier : WeaponArt
    {
        private const int RANGE = 1;
        private const int DURATION = 3;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (node == null) return;

            if (CombatManager.Instance.AttackHits(combatant, null))
            {
                var nodes = Pathfinding.GetArea(node, RANGE);
                CombatManager.Instance.AddNewAreaEffect(new Effect_Barrier(), DURATION, nodes);
            }

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}