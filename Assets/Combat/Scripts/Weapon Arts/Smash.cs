using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Smash")]
    public class Smash : WeaponArt
    {
        public override void OnUse(Combatant combatant, Combatant target)
        {
            int dmg = combatant.GetAttributeBonus(Attributes.Physicality);
            if (dmg == 0) dmg = 1; // minimum value
            dmg *= 8;

            combatant.DealDamage(dmg, target);

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}