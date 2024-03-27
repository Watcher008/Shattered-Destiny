using UnityEngine;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Block")]
    public class Block : WeaponArt
    {
        private const int MODIFIER = 5;
        public override void OnUse(Combatant combatant, Combatant target)
        {
            combatant.Block = combatant.GetAttributeBonus(Characters.Attributes.Physicality)  * MODIFIER;
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}