using UnityEngine;
using UnityEngine.EventSystems;

namespace SD.Inventories
{
    public class ItemGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Inventory _inventory;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _elementParent;

        public Inventory Inventory => _inventory;
        public RectTransform Rect => _rect;
        public RectTransform ElementParent => _elementParent;

        public void SetValues(Inventory inventory)
        {
            _inventory = inventory;
            _inventory.onContentsChanged += RefreshDisplay;

            AdjustGridSize();
            RefreshDisplay();
        }

        private void OnDestroy()
        {
            _inventory.onContentsChanged -= RefreshDisplay;
        }

        private void RefreshDisplay()
        {
            InventoryManager.Instance.RefreshDisplay(this);
        }

        private void AdjustGridSize()
        {
            _rect.sizeDelta = new Vector2(_inventory.Dimensions.x * InventoryManager.CELL_SIZE, 
                _inventory.Dimensions.y * InventoryManager.CELL_SIZE);

            _rect.anchorMin = Vector2.zero;
            _rect.anchorMax = Vector2.zero;
        }

        public Vector2Int GetGridPosition(Vector2 mousPosition)
        {
            return new Vector2Int(
                // Compare to the bottom left corner of the rect transform
                Mathf.FloorToInt((mousPosition.x - _rect.position.x) / InventoryManager.CELL_SIZE),
                Mathf.FloorToInt((mousPosition.y - _rect.position.y) / InventoryManager.CELL_SIZE)
            );
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DragManager.Instance.OnGridHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DragManager.Instance.OnGridHover(null);
        }
    }
}

