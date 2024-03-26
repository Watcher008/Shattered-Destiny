using UnityEngine;
using SD.Characters;

namespace SD.Combat
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Bash")]
    public class Bash : WeaponArt
    {
        public override void OnUse(Combatant combatant)
        {


            // Need target



            combatant.SpendActionPoints(_actionPointCost);
        }

        public void OnUse(Combatant combatant, Combatant target)
        {
            int dmg = Mathf.RoundToInt(combatant.GetAttribute(Attributes.Physicality) / 20);
            if (dmg == 0) dmg = 1; // minimum value

            combatant.DealDamage(dmg, target);

            Debug.Log("Add Stun effect");


            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}