using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Warcry")]
    public class Warcry : WeaponArt
    {
        private const int RANGE = 2;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                if (target.IsPlayer != combatant.IsPlayer) return;

                var nodes = Pathfinding.GetArea(target.Node, RANGE);
                foreach (var newNode in nodes)
                {
                    if (CombatManager.Instance.CheckNode(newNode, out var nextTarget))
                    {
                        if (nextTarget.IsPlayer != combatant.IsPlayer) continue;
                        nextTarget.AddEffect(new Effect_Empowered());
                        nextTarget.AddEffect(new Hardened());
                    }
                }
                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}