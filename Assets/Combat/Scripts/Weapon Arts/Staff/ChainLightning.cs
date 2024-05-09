using System.Collections.Generic;
using UnityEngine;
using SD.Characters;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Staff/Chain Lightning")]
    public class ChainLightning : WeaponArt
    {
        private const int MODIFIER = 3;
        private const int RANGE = 2;

        public override void OnUse(Combatant combatant, PathNode node)
        {
            if (CombatManager.Instance.CheckNode(node, out var target))
            {
                // If initial misses, it ends, if initial hits, they all get hit
                if (!CombatManager.Instance.AttackHits(combatant, target)) target = null;

                var hitTargets = new List<Combatant>();
                float damageModifier = 1.0f;

                while (target != null)
                {
                    //Debug.Log("This hit chance is based on combatant positio to target, not previous hit position.");
                    //bool hits = CombatManager.Instance.AttackHits(combatant, target);
                    //if (!hits) break; // Chain ends on a miss

                    int dmg = combatant.GetAttributeBonus(Attributes.Intelligence) * MODIFIER;
                    dmg = Mathf.RoundToInt(dmg * damageModifier);

                    combatant.DealDamage(dmg, target);
                    damageModifier += 0.2f; // +20% dmg per hit

                    hitTargets.Add(target);
                    var oldNode = target.Node; // cache before nulling target
                    target = null; // Set to null so loop breaks 

                    // Try to find next target
                    var nodes = Pathfinding.GetArea(oldNode, RANGE);
                    foreach(var newNode in nodes)
                    {
                        if (CombatManager.Instance.CheckNode(newNode, out var nextTarget))
                        {
                            if (hitTargets.Contains(nextTarget)) continue;
                            target = nextTarget;
                            break;
                        }
                    }
                }

                if (hitTargets.Count > 0) hitTargets[hitTargets.Count - 1].AddEffect(new Stun());
                combatant.SpendActionPoints(_actionPointCost);
            }
        }
    }
}