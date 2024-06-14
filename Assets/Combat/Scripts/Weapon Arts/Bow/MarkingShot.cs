using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Marking Shot")]
    public class MarkingShot : WeaponArt
    {
        private const int MODIFIER = 3;
        private const byte DURATION = 2;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out IDamageable target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
                    combatant.DealDamage(dmg, target);

                    if (target is Combatant targetUnit)
                    {
                        targetUnit.AddEffect(StatusEffects.VULNERABLE, DURATION);
                    }
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}