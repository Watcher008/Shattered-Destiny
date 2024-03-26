using System.Collections.Generic;
using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Cleave")]
    public class Cleave : WeaponArt
    {
        public override void OnUse(Combatant combatant)
        {
            int dmg = Mathf.RoundToInt(combatant.GetAttribute(Attributes.Physicality) / 20);
            if (dmg == 0) dmg = 1; // minimum value
            dmg *= 3;

            var targets = new List<Combatant>();
            var targetList = combatant.IsPlayer ? CombatManager.Instance.EnemyCombatants : CombatManager.Instance.PlayerCombatants;

            foreach (var target in targetList)
            {
                if (Mathf.Abs(target.Node.X - combatant.Node.X) > 1) continue;
                if (Mathf.Abs(target.Node.Y - combatant.Node.Y) > 1) continue;
                targets.Add(target);
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