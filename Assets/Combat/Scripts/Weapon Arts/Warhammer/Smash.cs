using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Smash")]
    public class Smash : WeaponArt
    {
        private const int MODIFIER = 8;

        public override void OnUse(Combatant combatant, Combatant target)
        {
            int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
            combatant.DealDamage(dmg, target);

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}