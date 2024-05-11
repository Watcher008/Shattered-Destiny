using SD.Combat.WeaponArts;
using SD.Grids;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace SD.Combat
{
    /// <summary>
    /// A class that connects the GUI and combatant actions.
    /// </summary>
    public class CombatInterface : MonoBehaviour
    {
        /* So my current idea is that since each time a player turn starts, I have to click move/attack
         * just to show which character is active (I can add more indicators in the future) but I think I should
         * do whaat Fire Emblem does and have the blue movement and red attack highlights already present to 
         * streamline the process
         * 
         * 
         * TO DO:
         * Auto-end turn after current actor has acted and they have neither AP nor Movement remaining
         * Set Move, Sprint, Attack, and Weapon Art Buttons to interactable based on player AP/Move remaining
         */

        [SerializeField] private CombatPortrait _portrait;
        [SerializeField] private RectTransform _portraitParent;

        [Header("Buttons")]
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private Button _restButton;
        [SerializeField] private Button _moveButton;
        [SerializeField] private Button _sprintButton;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _weaponArtButton;
        [SerializeField] private Transform _weaponArtParent;
        private Button[] _weaponArtButtons;
        private TMP_Text[] _weaponArtText;

        [Header("Overlay")]
        [SerializeField] private Tilemap _overlay;
        [SerializeField] private RuleTile _blueHighlight; // movement
        [SerializeField] private RuleTile _redHighlight; // attacking
        [SerializeField] private RuleTile _greenHighlight; // buffs

        [Header("Combat End")]
        [SerializeField] private GameObject _combatEndPanel;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _flavorText;

        private InputAction mousePosition;
        private Camera cam;
        private int UILayer;

        private Combatant CurrentActor => CombatManager.Instance.CurrentActor;
        private WeaponArt _currentArt;

        private enum Action
        {
            None,
            Move,
            Attack,
            WeaponArt
        }

        private Action _action;
        private Action CurrentAction
        {
            get => _action;
            set
            {
                _currentArt = null;
                _overlay.ClearAllTiles();
                HideAllWeaponArts();
                if (_action == value || value == Action.None)
                {
                    // sets to none when clicking same button
                    _action = Action.None;

                }
                else
                {
                    _action = value;
                }
            }
        }

        private void Awake()
        {
            UILayer = LayerMask.NameToLayer("UI");
            _combatEndPanel.SetActive(false);

            _endTurnButton.onClick.AddListener(OnEndTurnSelected);
            _restButton.onClick.AddListener(OnRestSelected);
            _moveButton.onClick.AddListener(OnMoveSelected);
            _sprintButton.onClick.AddListener(OnSprintSelected);
            _attackButton.onClick.AddListener(OnAttackSelected);
            _weaponArtButton.onClick.AddListener(OnWeaponArtSelected);

            _weaponArtButtons = _weaponArtParent.GetComponentsInChildren<Button>(true);
            _weaponArtText = new TMP_Text[_weaponArtButtons.Length];
            for (int i = 0; i < _weaponArtButtons.Length; i++)
            {
                _weaponArtText[i] = _weaponArtButtons[i].GetComponentInChildren<TMP_Text>();
            }

            cam = Camera.main;
            var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
            mousePosition = input.actions["Mouse Position"];
            input.actions["LMB"].performed += OnMouseClick;
        }

        private void OnDestroy()
        {
            _endTurnButton.onClick.RemoveAllListeners();
            _restButton.onClick.RemoveAllListeners();
            _moveButton.onClick.RemoveAllListeners();
            _sprintButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
            _weaponArtButton.onClick.RemoveAllListeners();

            var obj = GameObject.FindGameObjectWithTag("PlayerInput");
            if (obj != null && obj.TryGetComponent(out PlayerInput input))
            {
                input.actions["LMB"].performed -= OnMouseClick;
            }
        }

        #region - Mouse Click -
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (_action == Action.None) return;
            if (IsPointerOverUIElement()) return; // Can cause issues if a button is over a tile as well

            // Raycast to mouse position
            Ray ray = cam.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
            if (!Physics.Raycast(ray, out var hit)) return;

            var node = CombatManager.Instance.GetNode(new Vector3(hit.point.x, hit.point.z, 0));

            switch (CurrentAction)
            {
                case Action.Move:
                    OnMoveToNode(node);
                    break;
                case Action.Attack:
                    OnAttackTarget(hit, node);
                    ToggleRestButton();
                    break;
                case Action.WeaponArt:
                    if (_currentArt == null) return;
                    // I feel like I probably need to make a check first?
                    _currentArt.OnUse(CurrentActor, node);
                    ToggleRestButton();
                    break;
            }

            CurrentAction = Action.None;
        }

        private bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayer)
                    return true;
            }
            return false;
        }

        //Gets all event system raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
        #endregion

        #region - Public -
        public void AddPortraits()
        {
            // Do this after the combatants are sorted
            foreach (var combatant in CombatManager.Instance.Combatants)
            {
                var portrait = Instantiate(_portrait, _portraitParent);
                portrait.SetCombatant(combatant);
            }
        }

        /// <summary>
        /// Called at the start of a new unit's turn.
        /// </summary>
        public void OnNewActor()
        {
            ToggleRestButton();
        }

        /// <summary>
        /// Called at the end of a unit's turn.
        /// </summary>
        public void ClearInput()
        {
            CurrentAction = Action.None;
        }

        public void OnCombatEnd()
        {
            _combatEndPanel.SetActive(true);
            bool victory = CombatManager.Instance.EnemyCombatants.Count == 0;

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
        #endregion

        private void HighlightArea(PathNode from, int range, RuleTile tile)
        {
            var nodes = Pathfinding.GetArea(from, range);
            if (nodes == null) return;

            // I'm going to have to add some more logic here, don't show movement for occupied nodes
            // Don't highlight nodes for attacking if occupied by allies, etc.
            foreach (var node in nodes)
            {
                if (node.Occupant == Occupant.Player) continue;

                var pos = new Vector3Int(node.X, node.Y, 0);
                _overlay.SetTile(pos, tile);
            }
        }

        private bool IgnoreSelection()
        {
            if (!CurrentActor.PlayerControlled) return false;
            if (!CurrentActor.IsPlayer) return true;
            if (CurrentActor.IsActing) return true;
            return false;
        }

        private void ToggleRestButton()
        {
            _restButton.interactable = CurrentActor.CanRest;
        }

        private void OnEndTurnSelected()
        {
            if (IgnoreSelection()) return;

            CurrentAction = Action.None;
            CombatManager.Instance.EndTurn(CurrentActor);
        }

        /// <summary>
        /// Current actor chooses to rest.
        /// </summary>
        private void OnRestSelected()
        {
            if (IgnoreSelection()) return;
            if (!CurrentActor.CanRest) return;
            CurrentAction = Action.None;
            CurrentActor.OnRest();
        }

        private void OnSprintSelected()
        {
            if (IgnoreSelection()) return;
            CurrentAction = Action.None;
            CurrentActor.OnSprint();
            ToggleRestButton();
        }

        #region - Movement -
        private void OnMoveSelected()
        {
            if (IgnoreSelection()) return;

            CurrentAction = Action.Move;
            if (CurrentAction == Action.None) return;

            // Get the movable area of the unit
            var area = Pathfinding.GetMovementRange(CurrentActor.Node, CurrentActor.MovementRemaining, Occupant.Player);

            foreach (var node in area)
            {
                if (node.Occupant != Occupant.None || !node.IsWalkable) continue;

                var pos = new Vector3Int(node.X, node.Y, 0);
                _overlay.SetTile(pos, _blueHighlight);
            }
        }

        private void OnMoveToNode(PathNode node)
        {
            // Check for valid node
            if (node == null) return;

            // Find path
            var path = Pathfinding.FindNodePath(CurrentActor.Node, node, false, Occupant.Player);
            if (path == null) return;

            if (path[0] == CurrentActor.Node) path.RemoveAt(0);
            if (path.Count > CurrentActor.MovementRemaining) return; // valid path but not within immediate move range

            while (path.Count > CurrentActor.MovementRemaining) path.RemoveAt(path.Count - 1); // cut down to max Move
            CurrentActor.Move(path);
        }
        #endregion

        #region - Basic Attack -
        private void OnAttackSelected()
        {
            if (IgnoreSelection()) return;

            CurrentAction = Action.Attack;
            if (CurrentAction == Action.None) return;


            HighlightArea(CurrentActor.Node, CurrentActor.AttackRange, _redHighlight);
        }

        private void OnAttackTarget(RaycastHit hit, PathNode node)
        {
            // Check if they clicked onto a character sprite
            if (hit.collider.TryGetComponent(out IDamageable target)) CurrentActor.Attack(target);
            //if (hit.collider.TryGetComponent(out Combatant target)) CurrentActor.Attack(target);
            else if (node != null) // else check if they clicked on a node
            {
                for (int i = CombatManager.Instance.EnemyCombatants.Count - 1; i >= 0; i--)
                {
                    if (CombatManager.Instance.EnemyCombatants[i].Node == node)
                    {
                        CurrentActor.Attack(CombatManager.Instance.EnemyCombatants[i] as IDamageable);
                        break;
                    }
                }
            }
        }
        #endregion

        #region - Weapon Arts -
        private void OnWeaponArtSelected()
        {
            if (IgnoreSelection()) return;

            CurrentAction = Action.WeaponArt;
            if (CurrentAction == Action.None) return;

            // Display a new panel with buttons for each weapon art
            for (int i = 0; i < CurrentActor.WeaponArts.Count; i++)
            {
                int index = i;
                _weaponArtButtons[index].interactable = CurrentActor.ActionPoints >= CurrentActor.WeaponArts[index].Cost;
                _weaponArtButtons[index].gameObject.SetActive(true);
                _weaponArtText[index].text = CurrentActor.WeaponArts[index].name;

                _weaponArtButtons[index].onClick.AddListener(delegate
                {
                    BeginWeaponArtTargeting(CurrentActor.WeaponArts[index]);
                });
            }
        }

        private void BeginWeaponArtTargeting(WeaponArt art)
        {
            //HideAllWeaponArts();
            _currentArt = art;
            _overlay.ClearAllTiles();
            HighlightArea(CurrentActor.Node, art.Range, _redHighlight);

            // Pressing a button for a weapon art should then switch to targeting mode
            // Do I just highlight all nodes within range? That would be easiest...
            // Use red for attacks, green for buffs

        }

        private void HideAllWeaponArts()
        {
            for (int i = 0; i < _weaponArtButtons.Length; i++)
            {
                _weaponArtButtons[i].onClick.RemoveAllListeners();
                _weaponArtButtons[i].gameObject.SetActive(false);
            }
        }
        #endregion
    }
}