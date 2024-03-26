using UnityEngine;

namespace SD.Combat
{
    public abstract class WeaponArt : ScriptableObject
    {
        [SerializeField] protected int _actionPointCost;
        [SerializeField] private string _description;

        public abstract void OnUse(Combatant combatant);
    }
}