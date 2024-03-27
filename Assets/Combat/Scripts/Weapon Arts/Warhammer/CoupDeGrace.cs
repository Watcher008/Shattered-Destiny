using UnityEngine;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Warhammer/Coup De Grace")]
    public class CoupDeGrace : WeaponArt
    {
        private const float THRESHOLD = 0.35f;

        public override void OnUse(Combatant combatant, Combatant target)
        {
            if ((float)target.Health / target.MaxHealth <= THRESHOLD)
            {
                combatant.DealDamage(int.MaxValue, target);
            }
            
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}