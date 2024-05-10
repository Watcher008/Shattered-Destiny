using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Book/Heal")]
    public class Heal : WeaponArt
    {
        private const int MODIFIER = 8;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out Combatant target))
            {
                if (target.IsPlayer != combatant.IsPlayer) return;
                if (target.Health <= 0) return;

                target.RestoreHealth(combatant.GetAttributeBonus(Characters.Attributes.Intelligence) * MODIFIER);

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}