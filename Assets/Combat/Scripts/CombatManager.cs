using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using SD.Grids;
using SD.Characters;
using SD.Combat.WeaponArts;

namespace SD.Combat
{
    /// <summary>
    /// A class that handles the turn logic and initiation of combat.
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        private const int CELL_SIZE = 1;
        public const int GRID_SIZE = 10;
        private Grid<PathNode> _grid;
        public Grid<PathNode> Grid => _grid;

        public static CombatManager Instance;
        private bool _combatActive = true;

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CreatureCodex _creatureCodex;
        [SerializeField] private ItemCodex _itemCodex;
        
        [Space]
        
        [SerializeField] private BattlefieldBuilder _battlefield;
        [SerializeField] private CombatInterface _interface;

        [Space]

        [SerializeField] private Combatant _prefab;

        #region - Combatants -
        public Combatant CurrentActor { get; private set; }
        public List<Combatant> Combatants { get; private set; } = new();
        public List<Combatant> PlayerCombatants { get; private set; } = new();
        public List<Combatant> EnemyCombatants { get; private set; } = new();
        #endregion

        private int _currentRound;

        #region - Testing -
        [Header("Testing")]
        [SerializeField] private Button _endCombatButton;
        [SerializeField] private GameObject _blood;
        #endregion


        private Coroutine _delayCoroutine;

        private void ForTestingOnly()
        {
            _playerData.PlayerStats.EquipWeapon(_itemCodex.GetWeapon("Sword"));
            _playerData.PlayerStats.WeaponArts.AddRange(_playerData.WeaponArts);
        }

        private void Awake()
        {
            Instance = this;

            CreateGrid();

            _endCombatButton.onClick.AddListener(OnVictory);
        }

        private IEnumerator Start()
        {
            while (gameObject.scene != SceneManager.GetActiveScene()) yield return null;

            _battlefield.BuildGrid();
            PlaceCombatants();

            Combatants.Sort(); // Sort by initiative

            _interface.AddPortraits();

            yield return new WaitForSeconds(1.5f);

            _currentRound = 1;

            OnStartTurn(Combatants[0]);
        }

        private void OnDestroy()
        {
            _endCombatButton.onClick.RemoveAllListeners();
        }

        #region - Grid -
        private void CreateGrid()
        {
            _grid = new Grid<PathNode>(GRID_SIZE, GRID_SIZE, CELL_SIZE, Vector2.zero, 
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        }

        public PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }

        public PathNode GetNode(Vector3 worldPosition)
        {
            return _grid.GetGridObject(worldPosition);
        }

        public Vector3 GetNodePosition(int x, int y)
        {
            return _grid.GetNodePosition(x, y);
        }
        #endregion

        /// <summary>
        /// Place combatants in their starting positions on either side of the map.
        /// </summary>
        private void PlaceCombatants()
        {
            //Select group randomly based on current terrain
            var terrain = WorldMap.GetNode(_playerData.WorldPos.x, _playerData.WorldPos.y).Terrain.ToString();
            var units = _creatureCodex.GetSquad(terrain);

            // Spawn enemies based on chosen encounter
            for (int i = 0; i < units.Length; i++)
            {
                var statBlock = _creatureCodex.GetCreatureByName(units[i]);
                var enemy = Instantiate(_prefab, transform);
                var weapon = _itemCodex.GetWeapon(statBlock.Weapon);
                enemy.SetInitialValues(statBlock, weapon);

                Combatants.Add(enemy);
                EnemyCombatants.Add(enemy);
            }

            ForTestingOnly();

            // Spawn player
            var player = Instantiate(_prefab, transform);
            player.name = "Player";
            player.SetInitialValues(_playerData.Sprite, _playerData.PlayerStats);
            player.PlayerControlled = true;
            Combatants.Add(player);
            PlayerCombatants.Add(player);

            // Spawn player companions
            for (int i = 0; i < 2; i++)
            {
                var newPlayer = Instantiate(_prefab, transform);
                newPlayer.name = $"Player {i + 1}";
                newPlayer.SetInitialValues(SpriteHelper.GetSprite("creatures/knight"), _playerData.PlayerStats);
                newPlayer.gameObject.AddComponent<CombatantController>();
                Combatants.Add(newPlayer);
                PlayerCombatants.Add(newPlayer);
            }

            for (int i = 0; i < Combatants.Count; i++)
            {
                while (true)
                {
                    int y;
                    int x = Random.Range(0, GRID_SIZE - 1);

                    if (Combatants[i].IsPlayer) y = Random.Range(0, 2);
                    else y = Random.Range(GRID_SIZE - 3, GRID_SIZE - 1);

                    var node = GetNode(x, y);
                    if (node.IsWalkable && node.Occupant == Occupant.None)
                    {
                        Combatants[i].SetNode(node);
                        break;
                    }
                }
            }
        }

        #region - Turn Handling -
        private void OnStartTurn(Combatant combatant)
        {
            //Debug.Log("Turn start for " + combatant.gameObject.name);
            CurrentActor = combatant;
            CurrentActor.OnTurnStart(_currentRound > 1);
            _interface.OnNewActor();
        }

        private void OnNextTurn()
        {
            if (!_combatActive) return;

            int index = Combatants.IndexOf(CurrentActor) + 1;

            if (index >= Combatants.Count)
            {
                _currentRound++;
                index = 0;
            }

            OnStartTurn(Combatants[index]);
        }

        public void EndTurn(Combatant combatant)
        {
            if (combatant != CurrentActor) return;
            CurrentActor.onTurnEnd?.Invoke();
            _interface.ClearInput();
            OnNextTurn();
        }
        #endregion

        #region - Weapon Art Delay -
        /// <summary>
        /// Used to add delay between two segments of a Weapon Art.
        /// </summary>
        public void DelayWeaponArt(WeaponArt art, Combatant combatant)
        {
            if (_delayCoroutine != null) StopCoroutine(_delayCoroutine);
            _delayCoroutine = StartCoroutine(DelayCoroutine(art, combatant));
        }

        private IEnumerator DelayCoroutine(WeaponArt art, Combatant combatant)
        {
            while (combatant.IsActing) yield return null;
            art.OnComplete(combatant);
        }
        #endregion

        public bool CheckNode(PathNode node, out Combatant combatant)
        {
            combatant = null;
            foreach (var c in Combatants)
            {
                if (c.Node == node)
                {
                    combatant = c;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculates chance to hit based on terrain and status effect variables.
        /// </summary>
        /// <returns>True if the attack hits.</returns>
        public bool AttackHits(Combatant attacker, Combatant target)
        {
            // OP Auto-hit
            if (attacker.HasEffect<Focused>()) return true;

            float chanceToHit = 0.8f; // - 0.05f * attacker.Weapon.Tier

            // Bonus to hit if attacker is in a Mountain tile
            if (attacker.Node.Terrain == TerrainType.Mountain) chanceToHit += 0.2f;

            // Penalty to hit if target is in a Forest tile
            if (target.Node.Terrain == TerrainType.Forest) chanceToHit -= 0.2f;

            // Apply Penalty if target is Hard ;)
            if (target.HasEffect<Hardened>()) chanceToHit -= 0.25f;

            // Lastly check for terrain between the two
            var points = Bresenham.PlotLine(attacker.Node.X, attacker.Node.Y, target.Node.X, target.Node.Y);
            var nodes = Pathfinding.ConvertToNodes(_grid, points);
            nodes.Remove(attacker.Node);
            nodes.Remove(target.Node);

            foreach (var node in nodes)
            {
                if (node.Terrain == TerrainType.Mountain || node.Terrain == TerrainType.Forest)
                {
                    chanceToHit /= 2;
                    break;
                }
            }

            var roll = Random.value;
            bool attackHits = roll <= chanceToHit; // Roll under
            Debug.Log($"Chance to hit: {chanceToHit}, Roll: {roll}");
            if (!attackHits) Debug.Log("Attack miss!");
            return attackHits;
        }

        public void OnDamageTaken(Combatant combatant)
        {
            Instantiate(_blood, combatant.transform.position, Quaternion.identity);
        }

        public void OnCombatantDefeated(Combatant combatant)
        {
            Combatants.Remove(combatant);
            if (PlayerCombatants.Contains(combatant))
            {
                PlayerCombatants.Remove(combatant);
            }
            else if (EnemyCombatants.Contains(combatant))
            {
                EnemyCombatants.Remove(combatant);
            }
            Destroy(combatant.gameObject, 2.0f);

            if (PlayerCombatants.Count == 0)
            {
                OnDefeat();
            }
            else if (EnemyCombatants.Count == 0)
            {
                OnVictory();
            }
        }

        private void OnVictory()
        {
            _combatActive = false;
            _interface.Invoke(nameof(_interface.OnCombatEnd), 2.5f);

        }

        private void OnDefeat()
        {
            _combatActive = false;
            _interface.Invoke(nameof(_interface.OnCombatEnd), 2.5f);
        }
    }
}