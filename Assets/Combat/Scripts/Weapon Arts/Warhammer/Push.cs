using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Push")]
    public class Push : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out Combatant target))
            {
                bool stunTarget = true;
                var direction = new Vector2Int(target.Node.X - combatant.Node.X, target.Node.Y - combatant.Node.Y);

                // Make sure to not try to push them out of bounds
                var newNode = CombatManager.Instance.GetNode(target.Node.X + direction.x, target.Node.Y + direction.y);

                if (newNode != null && newNode.Occupant == Occupant.None)
                {
                    // Is the node occupied? Don't know what happens
                    //bool nodeOccupied = CombatManager.Instance.CheckNode(newNode, out _);

                    //if (!nodeOccupied)
                    {
                        target.ForceMove(newNode);
                        if ((int)newNode.Terrain <= (int)TerrainType.Road) stunTarget = false;
                    }
                }

                // stun target if forcing out of map, into occupied tile, or into difficult terrain
                if (stunTarget) target.AddEffect(new Stun());
                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}