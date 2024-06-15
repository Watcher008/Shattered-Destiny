using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Book/Slow")]
    public class Slow : WeaponArt
    {
        private const byte DURATION = 2;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out Combatant target))
            {
                //if (target.IsPlayer != combatant.IsPlayer) return;

                target.AddEffect(StatusEffects.SLOWED, DURATION);
                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}