using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SD.Inventories
{
    public class InventoryElement : MonoBehaviour, IPointerDownHandler, IInitializePotentialDragHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _itemCountText;

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private InventoryItem _item;
        private EquipmentSlot _equippedSlot;


        public RectTransform Rect => _rect;
        public InventoryItem Item => _item;
        public EquipmentSlot EquippedSlot
        {
            get => _equippedSlot;
            set => _equippedSlot = value;
        }

        private readonly float dragAlpha = 0.75f;


        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetItem(InventoryItem item)
        {
            _item = item;
            _image.sprite = SpriteHelper.GetSprite(item.Item.Sprite);
            _itemCountText.text = _item.Count == 1 ? string.Empty : _item.Count.ToString();
        }        

        #region - Pointer Events -
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Context menu for splitting half/one from stack
                //UI_ContextMenuManager.Instance.OnItemSelected(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Only allow dragging with LMB
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.alpha = dragAlpha;
                DragManager.Instance.OnBeginDrag(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Only allow dragging with LMB
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                DragManager.Instance.OnDrag(this, eventData.delta);
                //_rect.anchoredPosition += eventData.delta / scaleFactor;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1.0f;
            DragManager.Instance.OnEndDrag(this);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
        #endregion
    }
}