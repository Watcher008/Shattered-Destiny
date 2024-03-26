using UnityEngine;
using UnityEngine.InputSystem;

namespace SD.PathingSystem
{
    public class WorldMapCursor : MonoBehaviour
    {
        [SerializeField] private GameObject _point;

        private Transform _player;
        private PlayerController _playerController;

        private SpriteRenderer _sprite;

        private float roundedValue = 1.0f;
        private Vector2 mousePos;
        private Camera cam;

        private bool _canInteract = true;

        private void Start()
        {
            cam = Camera.main;
            _sprite = GetComponent<SpriteRenderer>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerController = _player.GetComponent<PlayerController>();

            var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
            input.actions["Mouse Position"].performed += OnMousePosition;
            input.actions["LMB"].performed += OnMouseClick;

            //roundedValue = Pathfinding.instance.GetCellSize();

            WorldMapManager.Instance.onPauseInput += OnPauseInput;
            WorldMapManager.Instance.onResumeInput += OnResumeInput;
        }

        private void OnDestroy()
        {
            WorldMapManager.Instance.onPauseInput -= OnPauseInput;
            WorldMapManager.Instance.onResumeInput -= OnResumeInput;

            var obj = GameObject.FindGameObjectWithTag("PlayerInput");
            if (obj != null && obj.TryGetComponent(out PlayerInput input))
            {
                input.actions["Mouse Position"].performed -= OnMousePosition;
                input.actions["LMB"].performed -= OnMouseClick;
            }
        }

        private void LateUpdate()
        {
            if (!_canInteract) return;

            SetCursorPosition();
            CheckNode();
        }

        private void OnPauseInput()
        {
            ClearLine();
            _canInteract = false;
            _sprite.enabled = false;
        }

        private void OnResumeInput()
        {
            _canInteract = true;
            _sprite.enabled = true;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (!_canInteract) return;

            var node = Pathfinding.instance.GetNode(transform.position);
            if (node == null) return;
            _playerController.SetDestination(node);
        }

        private void OnMousePosition(InputAction.CallbackContext obj)
        {
            mousePos = obj.ReadValue<Vector2>();
        }

        private void SetCursorPosition()
        {
            var pos = cam.ScreenToWorldPoint(mousePos);
            pos.x = Mathf.Round(pos.x * (1 / roundedValue)) * roundedValue;
            pos.y = Mathf.Round(pos.y * (1 / roundedValue)) * roundedValue;
            pos.z = 0;
            transform.position = pos;
        }

        private void CheckNode()
        {
            ClearLine();

            var node = Pathfinding.instance.GetNode(transform.position);
            if (node == null) return;
            //Debug.Log($"{node.Terrain}: {node.MovementModifier}");

            RenderLine();
        }

        private void ClearLine()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void RenderLine()
        {
            var path = Pathfinding.instance.FindVectorPath(transform.position, _player.position);
            if (path == null) return;

            for (int i = 0; i < path.Count; i++)
            {
                if (i >= transform.childCount)
                {
                    Instantiate(_point, path[i], Quaternion.identity, transform);
                }
                else
                {
                    transform.GetChild(i).position = path[i];
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }
}