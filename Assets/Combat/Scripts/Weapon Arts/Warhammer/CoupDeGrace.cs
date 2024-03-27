using SD.Grids;
using UnityEngine;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Coup De Grace")]
    public class CoupDeGrace : WeaponArt
    {
        private const float THRESHOLD = 0.35f;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                if ((float)target.Health / target.MaxHealth <= THRESHOLD)
                {
                    combatant.DealDamage(int.MaxValue, target);
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}