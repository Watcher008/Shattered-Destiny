using UnityEngine;
using UnityEngine.Tilemaps;
using SD.Grids;
using System.Net.NetworkInformation;

namespace SD.Combat
{
    /// <summary>
    /// A class that handles generating the combat map.
    /// </summary>
    public class BattlefieldBuilder : MonoBehaviour
    {
        private const int FOREST_MOVE_MOD = 1;
        private const int MOUNTAIN_MOVE_MOD = 1;
        private const int WATER_MOVE_MOD = 2;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _overlay;

        [Tooltip("The minimum distance between trees.")]
        [SerializeField, Range(2, 5)] private int _treeSpacing = 2;

        [SerializeField] private RuleTile _waterTile;
        [SerializeField] private RuleTile _rockTile;

        [Space]

        [SerializeField] private GameObject _forest;
        [SerializeField] private GameObject _mountain;

        [Header("Camps")]
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _trap;
        [SerializeField] private GameObject _tower;

        // So... when building the battlefield, the following factors need to be taken into account
        // the current world tile of the player, e.g. where the combat is taking place in world space
        // the surrounding tiles of that tile, and the terrain

        // combat initiated on a road should have that road present on the battlefield
        // Refer to the Town example from dawnlike since that's what I'm working with atm.

        public void BuildGrid(string[] template)
        {
            for (int y = 0; y < template.Length; y++)
            {
                for (int x = 0; x < template[y].Length; x++)
                {
                    char c = template[x][y];
                    GameObject go = GetObject(c);
                    if (go == null) continue;

                    var node = CombatManager.Instance.GetNode(x, y);
                    if (node == null) continue;

                    var pos = new Vector3Int(node.X, 0, node.Y);
                    Instantiate(go, pos, Quaternion.identity, transform);
                }
            }
        }

        private GameObject GetObject(char c)
        {
            switch (c)
            {
                case '#': return _wall;
                case 'T': return _trap;
                case 'W': return _tower;
                default: return null;
            }
        }

        public void BuildGrid()
        {
            var points = Poisson.GeneratePoints(0, _treeSpacing, 
                new Vector2(CombatManager.GRID_SIZE - 1, CombatManager.GRID_SIZE - 1));

            foreach (var point in points)
            {
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.y);

                var node = CombatManager.Instance.GetNode(x, y);
                if (node == null) continue;

                var value = Random.value;
                if (value <= 0.25f) SetWaterTile(node);
                else if (value <= 0.625f) SetForestTile(node);
                else SetMountainTile(node);
            }
        }

        private void SetForestTile(PathNode node)
        {
            if (node == null) return;

            node.SetTerrain(TerrainType.Forest);
            node.SetMovementCost(FOREST_MOVE_MOD);

            var pos = new Vector3Int(node.X, 0, node.Y);
            Instantiate(_forest, pos, Quaternion.identity, transform);
        }

        private void SetMountainTile(PathNode node)
        {
            if (node == null) return;

            node.SetTerrain(TerrainType.Mountain);
            node.SetMovementCost(MOUNTAIN_MOVE_MOD);

            var tilePos = new Vector3Int(node.X, node.Y);
            _tilemap.SetTile(tilePos, _rockTile);

            var pos = new Vector3Int(node.X, 0, node.Y);
            Instantiate(_mountain, pos, Quaternion.identity, transform);
        }

        private void SetWaterTile(PathNode node)
        {
            if (node == null) return;

            node.SetTerrain(TerrainType.Water);
            node.SetMovementCost(WATER_MOVE_MOD);

            var tilePos = new Vector3Int(node.X, node.Y);
            _tilemap.SetTile(tilePos, _waterTile);
        }
    }
}