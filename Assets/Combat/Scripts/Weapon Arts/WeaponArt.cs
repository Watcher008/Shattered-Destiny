using UnityEngine;
using SD.Grids;

namespace SD.Combat
{
    public abstract class WeaponArt : ScriptableObject
    {
        [SerializeField] protected int _actionPointCost;
        [SerializeField] protected int _range;
        [SerializeField] private string _description;

        public int Range => _range;

        public abstract void OnUse(Combatant combatant, PathNode node);
    }
}