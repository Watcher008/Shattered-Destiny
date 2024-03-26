using UnityEngine;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Block")]
    public class Block : WeaponArt
    {
        public override void OnUse(Combatant combatant, Combatant target)
        {
            combatant.Block += 5;
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}