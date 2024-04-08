using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Book/Reinforce")]
    public class Reinforce : WeaponArt
    {
        private const int DURATION = 2;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                //if (target.IsPlayer != combatant.IsPlayer) return;

                target.AddEffect(new Effect_Reinforced(), DURATION);
                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}