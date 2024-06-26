using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Staff/Confuse")]
    public class Confuse : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    target.AddEffect(new Effect_Confused());
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}