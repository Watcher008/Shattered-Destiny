using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Headshot")]
    public class Headshot : WeaponArt
    {
        private const int MODIFIER = 6;

        public override void OnUse(Combatant combatant, Combatant target)
        {
            if (CombatManager.Instance.AttackHits(combatant, target))
            {
                int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
                combatant.DealDamage(dmg, target);
            }

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}