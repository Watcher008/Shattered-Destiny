using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Staff/Magic Bolt")]
    public class MagicBolt : WeaponArt
    {
        private const int MODIFIER = 5;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out IDamageable target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    int dmg = combatant.GetAttributeBonus(Attributes.Intelligence) * MODIFIER;
                    combatant.DealDamage(dmg, target);
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}