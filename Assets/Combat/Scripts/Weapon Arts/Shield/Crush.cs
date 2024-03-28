using UnityEngine;
using SD.Grids;

namespace SD.Combat.WeaponArts
{
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Shield/Crush")]
    public class Crush : WeaponArt
    {
        public override void OnUse(Combatant combatant, PathNode node)
        {
            node.SetTerrain(TerrainType.Grassland);
            Debug.LogWarning("Need to tell CombatManager to remove object here.");
            combatant.SpendActionPoints(_actionPointCost);
        }
    }
}