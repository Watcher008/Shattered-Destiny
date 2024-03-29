using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Stand Behind Me")]
    public class StandBehindMe : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                // Only target allied units
                if (target.IsPlayer != combatant.IsPlayer) return;

                int x = Mathf.Abs(target.Node.X - combatant.Node.X);
                int y = Mathf.Abs(target.Node.Y - combatant.Node.Y);

                // Where the allied unit will be pushed
                var newNode = CombatManager.Instance.GetNode(node.X + x, node.Y + y);
                if (newNode == null) return; // Fails if would be pushed out of bounds
                
                // Loop until node is valid or is null
                while(!NodeIsValid(combatant, newNode))
                {
                    newNode = CombatManager.Instance.GetNode(newNode.X + x, newNode.Y + y);
                    if (newNode == null) return; // Fails if would be pushed out of bounds
                }

                combatant.ForceMove(node);
                target.ForceMove(newNode);
                target.AddEffect(new Reinforced());

                combatant.SpendActionPoints(_actionPointCost);
            }
        }

        private bool NodeIsValid(Combatant combatant, PathNode node)
        {
            // Outside map bounds
            if (node == null) return false;
            // Can swap places with user
            if (combatant.Node == node) return true;
            // Cannot move into node of other units
            if (node.Occupant != Occupant.None) return false;
            return true;
        }
    }
}