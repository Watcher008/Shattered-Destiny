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
            _image.sprite = InventoryManager.Instance.GetSprite(item.Weapon.Sprite);
            _itemCountText.text = GetCountText();
        }

        public void UpdateSprite(bool useFull)
        {
            //if (useFull) _image.sprite = _item.Item.Data.Sprite;
            //else _image.sprite = _item.Item.Data.CompactSprite;
        }
        
        private string GetCountText()
        {
            return string.Empty;
            /*if (_item.Item.Data.Category == ItemCategory.RangedWeapon)
            {
                if (TryGetComponent<Firearm>(out var firearm))
                {
                    return firearm.Magazine.Rounds + "/" + firearm.Magazine.Capacity;
                }
            }
            else if (_item.Item.Data.Category == ItemCategory.Attachment)
            {
                if (TryGetComponent<FirearmMagazine>(out var mag))
                {
                    return mag.Rounds + "/" + mag.Capacity;
                }
            }

            return (_item.Item.StackCount == 0) ? string.Empty : _item.Item.StackCount.ToString();*/
        }


        #region - Pointer Events -

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
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