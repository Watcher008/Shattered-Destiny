using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Bash")]
    public class Bash : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out IDamageable target))
            {
                int dmg = combatant.GetAttributeBonus(Attributes.Physicality);

                combatant.DealDamage(dmg, target);
                if (target is Combatant c)
                {
                    c.AddEffect(new Stun());
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}