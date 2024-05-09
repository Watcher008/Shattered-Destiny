using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bow/Ankle Shot")]
    public class AnkleShot : WeaponArt
    {
        private const int MODIFIER = 3;
        private const int DURATION = 2;

        public override void OnUse(Combatant combatant, PathNode targetNode)
        {
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * MODIFIER;
                    combatant.DealDamage(dmg, target);
                    target.AddEffect(new Effect_Slowed(), DURATION);
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}