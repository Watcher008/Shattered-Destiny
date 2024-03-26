using System.Collections.Generic;
using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Cleave")]
    public class Cleave : WeaponArt
    {
        public override void OnUse(Combatant combatant, Combatant target)
        {
            int dmg = combatant.GetAttributeBonus(Attributes.Physicality);
            if (dmg == 0) dmg = 1; // minimum value
            dmg *= 3;

            var targets = new List<Combatant>();
            var targetList = combatant.IsPlayer ? CombatManager.Instance.EnemyCombatants : CombatManager.Instance.PlayerCombatants;

            foreach (var unit in targetList)
            {
                if (Mathf.Abs(unit.Node.X - combatant.Node.X) > 1) continue;
                if (Mathf.Abs(unit.Node.Y - combatant.Node.Y) > 1) continue;
                targets.Add(unit);
            }

            for (int i = targets.Count - 1; i >= 0; i--)
            {
                Debug.Log($"Dealing {dmg} to {targets[i].gameObject.name}");
                combatant.DealDamage(dmg, targets[i]);
            }

            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}