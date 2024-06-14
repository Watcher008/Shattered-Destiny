using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Stand Your Ground")]
    public class StandYourGround : WeaponArt
    {
        private const int RANGE = 3;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            Debug.LogWarning("Not Yet Implemented.");
            if (CombatManager.Instance.CheckNode(node, out Combatant target))
            {
                // Get all units within range
                var area = Pathfinding.GetArea(combatant.Node, RANGE);
                foreach(var areaNode in area)
                {
                    if (CombatManager.Instance.CheckNode(areaNode, out Combatant unit))
                    {
                        if (unit.IsPlayer == combatant.IsPlayer)
                        {
                            unit.AddEffect(StatusEffects.REINFORCED);
                            return;
                        }
                        else
                        {
                            // Push 
                        }
                    }
                }
                /*

                int x = Mathf.Abs(target.Node.X - combatant.Node.X);
                int y = Mathf.Abs(target.Node.Y - combatant.Node.Y);

                // Where the allied unit will be pushed
                var newNode = CombatManager.Instance.GetNode(node.X + x, node.Y + y);
                if (newNode == null) return; // Fails if would be pushed out of bounds

                // Loop until node is valid or is null
                while (!NodeIsValid(combatant, newNode))
                {
                    newNode = CombatManager.Instance.GetNode(newNode.X + x, newNode.Y + y);
                    if (newNode == null) return; // Fails if would be pushed out of bounds
                }

                target.ForceMove(newNode);*/

                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}