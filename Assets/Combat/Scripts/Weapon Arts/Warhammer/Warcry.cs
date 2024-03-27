using UnityEngine;
using SD.Grids;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Warcry")]
    public class Warcry : WeaponArt
    {
        private const int RANGE = 2;

        public override void OnUse(Combatant combatant, Combatant target)
        {
            if (target.IsPlayer != combatant.IsPlayer) return;

            var nodes = Pathfinding.instance.GetArea(target.Node, RANGE);
            foreach (var node in nodes)
            {
                if (CombatManager.Instance.CheckNode(node, out var nextTarget))
                {
                    if (nextTarget.IsPlayer != combatant.IsPlayer) continue;
                    nextTarget.AddEffect(new Empowered());
                    nextTarget.AddEffect(new Hardened());
                }
            }
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}