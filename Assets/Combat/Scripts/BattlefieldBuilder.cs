using UnityEngine;
using UnityEngine.Tilemaps;
using SD.Grids;

namespace SD.Combat
{
    /// <summary>
    /// A class that handles generating the combat map.
    /// </summary>
    public class BattlefieldBuilder : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _overlay;

        [Tooltip("The minimum distance between trees.")]
        [SerializeField, Range(2, 5)] private int _treeSpacing = 2;

        [SerializeField] private RuleTile _waterTile;
        [SerializeField] private RuleTile _rockTile;

        [Space]

        [SerializeField] private GameObject _forest;
        [SerializeField] private GameObject _mountain;

        // So... when building the battlefield, the following factors need to be taken into account
        // the current world tile of the player, e.g. where the combat is taking place in world space
        // the surrounding tiles of that tile, and the terrain

        // combat initiated on a road should have that road present on the battlefield
        // Refer to the Town example from dawnlike since that's what I'm working with atm.

        public void BuildGrid()
        {
            // Pseudo-randomly place down appropriate floor types

            // If on a road, there should be a road going through the middle of the map

            // The type/density of terrain placement should be based on the world terrain the player is in
            // Forest = more forests, etc.

            // Randomly place down obstacles - trees, rocks, etc.
            var points = Poisson.GeneratePoints(0, _treeSpacing, 
                new Vector2(CombatManager.GRID_SIZE - 1, CombatManager.GRID_SIZE - 1));

            foreach (var point in points)
            {
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.y);

                var node = Pathfinding.instance.GetNode(x, y);
                if (node == null) continue;

                var pos = new Vector3Int(x, 0, y);
                var tilePos = new Vector3Int(x, y);
                var value = Random.value;

                if (value <= 0.25f)
                {
                    _tilemap.SetTile(tilePos, _waterTile);
                    node.SetTerrain(TerrainType.Water);
                }
                else if (value <= 0.625f)
                {
                    Instantiate(_forest, pos, Quaternion.identity, transform);
                    node.SetTerrain(TerrainType.Forest);
                }
                else
                {
                    Instantiate(_mountain, pos, Quaternion.identity, transform);
                    node.SetTerrain(TerrainType.Mountain);
                    _tilemap.SetTile(tilePos, _rockTile);
                }
            }
        }
    }
}