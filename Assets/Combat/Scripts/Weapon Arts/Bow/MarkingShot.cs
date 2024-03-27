using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Marking Shot")]
    public class MarkingShot : WeaponArt
    {
        private const int MODIFIER = 3;
        private const int DURATION = 2;

        public override void OnUse(Combatant combatant, Combatant target)
        {
            if (CombatManager.Instance.AttackHits(combatant, target))
            {
                int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
                combatant.DealDamage(dmg, target);
                target.AddEffect(new Vulnerable(), DURATION);
            }

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}