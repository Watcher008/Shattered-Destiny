using UnityEngine;
using SD.Grids;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Push")]
    public class Push : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                var direction = new Vector2Int(target.Node.X - combatant.Node.X, target.Node.Y - combatant.Node.Y);

                // Make sure to not try to push them out of bounds
                var newNode = Pathfinding.instance.GetNode(target.Node.X + direction.x, target.Node.Y + direction.y);
                if (newNode != null)
                {
                    // Is the node occupied? Don't know what happens

                    // Force move to that node
                    Debug.LogWarning("This has not been fully implemented.");
                    if ((int)newNode.Terrain > (int)TerrainType.Road)
                    {
                        target.AddEffect(new Stun());
                    }
                }

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}