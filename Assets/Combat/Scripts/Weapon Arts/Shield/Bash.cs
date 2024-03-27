using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Bash")]
    public class Bash : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                int dmg = combatant.GetAttributeBonus(Attributes.Physicality);

                combatant.DealDamage(dmg, target);
                target.AddEffect(new Stun());

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}