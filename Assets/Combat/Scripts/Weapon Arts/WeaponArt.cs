using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    public abstract class WeaponArt : ScriptableObject
    {
        [SerializeField] protected int _actionPointCost;
        [SerializeField] protected int _range;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _sprite;

        public int Cost => _actionPointCost;
        public int Range => _range;
        public Sprite Sprite => _sprite;

        public abstract void OnUse(Combatant combatant, PathNode node);

        /// <summary>
        /// Handles timing separation between the start of the action and a second half.
        /// </summary>
        public virtual void OnComplete(Combatant combatant) { }
    }
}