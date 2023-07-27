using UnityEngine;
using UnityEngine.InputSystem;

namespace SD.PathingSystem
{
    public class GridCursor_WorldMap : MonoBehaviour
    {
        [SerializeField] private WorldNodeReference playerTravelData;
        [SerializeField] private InputActionProperty mousePosition;
        private float roundedValue;
        private Vector2 mousePos;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
            mousePosition.action.performed += OnMousePosition;
            roundedValue = Pathfinding.instance.GetCellSize();
        }

        private void OnDestroy()
        {
            mousePosition.action.performed -= OnMousePosition;
        }

        private void LateUpdate()
        {
            SetCursorPosition();
            CheckNode();
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
            var node = Pathfinding.instance.GetNode(transform.position);
            playerTravelData.NodeReference = node;
            if (node == null) return;

            //Else, create another scriptable object to hold hovered node information
        }
    }
}