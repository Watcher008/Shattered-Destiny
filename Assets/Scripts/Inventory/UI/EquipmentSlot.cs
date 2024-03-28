using UnityEngine;
using SD.Inventories;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnItemDropped(EquipmentType slot, InventoryElement element);
    public OnItemDropped onItemDropped;

    [SerializeField] private EquipmentType slot;
    private RectTransform _rect;

    public EquipmentType Slot => slot;
    public RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private void OnDestroy()
    {
        onItemDropped = null;
    }

    // The player has dropped a UI_InventoryElement object on top of the equip slot, try to equip it
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!eventData.pointerDrag.TryGetComponent<InventoryElement>(out var element)) return;
            if (element.Item.Item is not Equipment) return;

            onItemDropped?.Invoke(slot, element);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragManager.Instance.OnEquipSlotHover(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DragManager.Instance.OnEquipSlotHover(null);
    }
}