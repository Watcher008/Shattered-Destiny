using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Smash")]
    public class Smash : WeaponArt
    {
        private const int MODIFIER = 8;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
                combatant.DealDamage(dmg, target);

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}