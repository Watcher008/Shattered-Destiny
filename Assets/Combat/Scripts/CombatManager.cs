using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using SD.PathingSystem;
using SD.Characters;
using SD.Combat;

public class CombatManager : MonoBehaviour
{ 
    private const int CELL_SIZE = 1;
    private const int GRID_SIZE = 10;

    public static CombatManager Instance;
    private bool _combatActive = true;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private CreatureCodex _creatureCodex;
    [SerializeField] private ItemCodex _itemCodex;

    [Space]

    [SerializeField] private Combatant _prefab;
    [SerializeField] private CombatPortrait _portrait;

    [Space]

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _overlay;
    [SerializeField] private RectTransform _portraitParent;

    [Tooltip("The minimum distance between trees.")]
    [SerializeField, Range(1, 5)] private int _treeSpacing = 2;

    #region - Combatants -
    private Combatant _currentActor;

    private List<Combatant> _combatants = new ();
    public List<Combatant> PlayerCombatants { get; private set; } = new();
    public List<Combatant> EnemyCombatants { get; private set; } = new();
    #endregion

    [Header("Buttons")]
    [SerializeField] private Button _skipTurnButton;
    [SerializeField] private Button _moveButton;
    [SerializeField] private Button _sprintButton;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _weaponArtButton;

    [Header("Overlay")]
    [SerializeField] private RuleTile _moveHighlight;
    [SerializeField] private RuleTile _attackHighlight;

    [Header("Combat End")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _flavorText;

    #region - Testing -
    [Header("Testing")]
    [SerializeField] private Button _endCombatButton;
    [SerializeField] private Sprite _player;
    [SerializeField] private Sprite _enemy;
    [SerializeField] private GameObject _tree;
    [SerializeField] private GameObject _rock;
    #endregion

    #region - Input -
    private InputAction mousePosition;
    private Camera cam;

    private enum Action
    {
        None,
        Move,
        Attack,
    }

    private Action _action;
    private Action CurrentAction
    {
        get => _action;
        set
        {
            _overlay.ClearAllTiles();
            if (_action == value || value == Action.None)
            {
                _action = Action.None;
            }
            else
            {
                _action = value;
            }
        }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);

        Pathfinding.instance?.Destroy();
        new Pathfinding(GRID_SIZE, GRID_SIZE, CELL_SIZE, Vector2.zero);

        _skipTurnButton.onClick.AddListener(OnWaitSelected);
        _moveButton.onClick.AddListener(OnMoveSelected);
        _sprintButton.onClick.AddListener(OnSprintSelected);
        _attackButton.onClick.AddListener(OnAttackSelected);
        _weaponArtButton.onClick.AddListener(OnWeaponArtSelected);

        _endCombatButton.onClick.AddListener(OnVictory);

        cam = Camera.main;
        var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
        mousePosition = input.actions["Mouse Position"];
        input.actions["LMB"].performed += OnMouseClick; ;
    }

    private IEnumerator Start()
    {
        while (gameObject.scene != SceneManager.GetActiveScene()) yield return null;

        BuildGrid();
        PlaceCombatants();

        _combatants.Sort(); // Sort by initiative

        AddPortraits();

        yield return new WaitForSeconds(1.5f);

        OnStartTurn(_combatants[0]);
    }

    private void OnDestroy()
    {
        Pathfinding.instance?.Destroy();
        _skipTurnButton.onClick.RemoveAllListeners();
        _moveButton.onClick.RemoveAllListeners();
        _sprintButton.onClick.RemoveAllListeners();
        _attackButton.onClick.RemoveAllListeners();
        _weaponArtButton.onClick.RemoveAllListeners();

        _endCombatButton.onClick.RemoveAllListeners();

        var obj = GameObject.FindGameObjectWithTag("PlayerInput");
        if (obj != null && obj.TryGetComponent(out PlayerInput input))
        {
            input.actions["LMB"].performed -= OnMouseClick;
        }
    }

    private void OnMouseClick(InputAction.CallbackContext obj)
    {
        // Raycast to mouse position
        Ray ray = cam.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        if (!Physics.Raycast(ray, out var hit)) return;

        var node = Pathfinding.instance.GetNode(new Vector3(hit.point.x, hit.point.z, 0));

        switch (CurrentAction)
        {
            case Action.Move:
                OnMoveToNode(node);
                break;
            case Action.Attack:
                OnAttackTarget(hit, node);
                break;
        }

        CurrentAction = Action.None;
    }

    #region - Init -
    /// <summary>
    /// Randomly place down the combat map. Should be dependent upon world tile.
    /// </summary>
    private void BuildGrid()
    {
        // Pseudo-randomly place down appropriate floor types

        // If on a road, there should be a road going through the middle of the map

        // Randomly place down obstacles - trees, rocks, etc.
        var points = Poisson.GeneratePoints(0, _treeSpacing, new Vector2(GRID_SIZE - 1, GRID_SIZE - 1));

        foreach(var point in points)
        {
            int x = Mathf.RoundToInt(point.x);
            int y = Mathf.RoundToInt(point.y);

            var node = Pathfinding.instance.GetNode(x, y);
            if (node != null)
            {
                Instantiate(_tree, new Vector3(x, 0, y), Quaternion.identity, transform);
                //node.SetOccupant(Occupant.Object);
            }
        }
    }

    /// <summary>
    /// Place combatants in their starting positions on either side of the map.
    /// </summary>
    private void PlaceCombatants()
    {
        var bandit = _creatureCodex.GetCreatureByName("Bandit");
        // For now just spawn 3 of each, later this should come from the player party and an encounter table

        // Spawn enemies based on chosen encounter
        for (int i = 0; i < 3; i++)
        {
            var newEnemy = Instantiate(_prefab, transform);
            newEnemy.name = $"Enemy {i + 1}";
            var weapon = _itemCodex.GetWeapon(bandit.DefaultWeapon);
            newEnemy.SetInitialValues(_enemy, bandit, weapon);
            _combatants.Add(newEnemy);
            EnemyCombatants.Add(newEnemy);
        }

        _playerData.PlayerStats.EquipWeapon(_itemCodex.GetWeapon("Basic Sword"));

        // Spawn player
        var player = Instantiate(_prefab, transform);
        player.name = "Player";
        player.SetInitialValues(_player, _playerData.PlayerStats);
        _combatants.Add(player);
        PlayerCombatants.Add(player);

        // Spawn player companions
        for (int i = 0; i < 2; i++)
        {
            var newPlayer = Instantiate(_prefab, transform);
            newPlayer.name = $"Player {i + 1}";
            newPlayer.SetInitialValues(_player, _playerData.PlayerStats);
            _combatants.Add(newPlayer);
            PlayerCombatants.Add(newPlayer);
        }

        for (int i = 0; i < _combatants.Count; i++)
        {
            while (true)
            {
                int y;
                int x = Random.Range(0, GRID_SIZE - 1);

                if (_combatants[i].IsPlayer) y = Random.Range(0, 2);
                else y = Random.Range(GRID_SIZE - 3, GRID_SIZE - 1);

                var node = Pathfinding.instance.GetNode(x, y);
                if (node.IsWalkable && node.Occupant == Occupant.None)
                {
                    _combatants[i].SetNode(node);
                    break;
                }
            }
        }
    }

    private void AddPortraits()
    {
        // Do this after the combatants are sorted
        foreach(var combatant in _combatants)
        {
            var portrait = Instantiate(_portrait, _portraitParent);
            portrait.SetCombatant(combatant);
        }
    }
    #endregion

    #region - Turn Handling -
    private void OnStartTurn(Combatant combatant)
    {
        //Debug.Log("Turn start for " + combatant.gameObject.name);
        _currentActor = combatant;
        _currentActor.OnTurnStart();
    }

    private void OnNextTurn()
    {
        if (!_combatActive) return;

        int index = _combatants.IndexOf(_currentActor) + 1;

        if (index >= _combatants.Count) index = 0;

        OnStartTurn(_combatants[index]);
    }

    public void EndTurn(Combatant combatant)
    {
        if (combatant != _currentActor) return;
        _currentActor.onTurnEnd?.Invoke();
        CurrentAction = Action.None;
        OnNextTurn();
    }
    #endregion

    #region - Actions -
    /// <summary>
    /// Current actor chooses to rest.
    /// </summary>
    private void OnWaitSelected()
    {
        if (!_currentActor.IsPlayer) return; // make sure player doesn't skip enemy turn
        else if (_currentActor.IsActing) return;
        CurrentAction = Action.None;
        _currentActor.OnRest();
    }

    private void OnSprintSelected()
    {
        if (!_currentActor.IsPlayer) return;
        else if (_currentActor.IsActing) return;
        _currentActor.OnSprint();
    }

    // Movement
    private void OnMoveSelected()
    {
        if (!_currentActor.IsPlayer) return;
        else if (_currentActor.IsActing) return;

        CurrentAction = Action.Move;
        if (CurrentAction == Action.None) return;

        var range = Pathfinding.instance.GetNodesInRange(_currentActor.Node, _currentActor.MovementRemaining);
        if (range == null) return;

        foreach(var node in range)
        {
            if (node.Occupant != Occupant.None || !node.IsWalkable) continue;

            var pos = new Vector3Int(node.X, node.Y, 0);
            _overlay.SetTile(pos, _moveHighlight);
        }
    }

    private void OnMoveToNode(PathNode node)
    {
        // Check for valid node
        if (node == null) return;

        // Find path
        var path = Pathfinding.instance.FindNodePath(_currentActor.Node, node, false, Occupant.Player);
        if (path == null) return;

        if (path[0] == _currentActor.Node) path.RemoveAt(0);
        if (path.Count > _currentActor.MovementRemaining) return; // valid path but not within immediate move range

        while (path.Count > _currentActor.MovementRemaining) path.RemoveAt(path.Count - 1); // cut down to max Move
        _currentActor.Move(path);
    }

    // Basic Attack
    private void OnAttackSelected()
    {
        if (!_currentActor.IsPlayer) return;
        else if (_currentActor.IsActing) return;

        CurrentAction = Action.Attack;
        if (CurrentAction == Action.None) return;

        var range = Pathfinding.instance.GetNodesInRange(_currentActor.Node, _currentActor.AttackRange);
        if (range == null) return;

        foreach (var node in range)
        {
            if (node.Occupant == Occupant.Player) continue;

            var pos = new Vector3Int(node.X, node.Y, 0);
            _overlay.SetTile(pos, _attackHighlight);
        }
    }

    private void OnAttackTarget(RaycastHit hit, PathNode node)
    {
        // Check if they clicked onto a character sprite
        if (hit.collider.TryGetComponent(out Combatant target)) _currentActor.Attack(target);
        else if (node != null) // else check if they clicked on a node
        {
            for (int i = EnemyCombatants.Count - 1; i >= 0; i--)
            {
                if (EnemyCombatants[i].Node == node)
                {
                    _currentActor.Attack(EnemyCombatants[i]);
                    break;
                }
            }
        }
    }

    private void OnWeaponArtSelected()
    {
        if (!_currentActor.IsPlayer) return;
        else if (_currentActor.IsActing) return;


    }
    #endregion

    public void OnCombatantDefeated(Combatant combatant)
    {
        _combatants.Remove(combatant);
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
        Invoke(nameof(OnCombatEnd), 2.5f);
    }

    private void OnDefeat()
    {
        _combatActive = false;
        Invoke(nameof(OnCombatEnd), 2.5f);
    }

    private void OnCombatEnd()
    {
        _panel.SetActive(true);
        bool victory = EnemyCombatants.Count == 0;

        if (victory)
        {
            _header.text = "VICTORY!";
            _flavorText.text = "Congratulations, noble warrior! Your valor and skill have vanquished the foe, " +
                "bringing glory to your name and peace to the realm!";
        }
        else
        {
            _header.text = "DEFEAT";
            _flavorText.text = "Alas, brave adventurer! Though valiant in battle, the forces of darkness " +
                "proved too formidable. Take heart, for even in defeat, legends are born anew.";
        }
    }
}
