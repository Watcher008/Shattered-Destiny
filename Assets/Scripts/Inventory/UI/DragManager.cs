using UnityEngine;
using UnityEngine.UI;
using SD.Inventories;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    private static Color green = new(0f, 255f, 0f, 0.33f);
    private static Color red = new(255f, 0f, 0f, 0.33f);

    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private InventoryElement _elementPrefab;

    private Image highlightColor;
    private EquipmentManager _equipmentManager;
    //private UI_InspectionPanel _inspectionPanel;

    /*public UI_InspectionPanel InspectionPanel
    {
        get => _inspectionPanel;
        set => _inspectionPanel = value;
    }*/

    // The grid that the mouse is currently over
    private ItemGrid hoverGrid;

    // The grid that the dragElement came from
    private ItemGrid fromDragGrid;

    // The equipment slot that the dragElement came from
    private EquipmentSlot fromEquipSlot;

    // The element currently being dragged
    private InventoryElement dragElement;

    // The offset between the mouse and the dragElement's origin
    private Vector2Int dragOffset;

    // The bottom left corner of the grid the mouse is over
    private Vector2 gridCorner;

    // If the drag item was rotated while dragging it
    private bool itemRotationToggled = false;

    // If the drag item is hovering over an equipslot
    private bool equipSlotHover = false;

    private void Awake()
    {
        Instance = this;
        highlightColor = highlightRect.GetComponent<Image>();
        _equipmentManager = GetComponent<EquipmentManager>();

        var input = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
        input.actions["RMB"].performed += OnRotateItem;
    }

    private void OnDestroy()
    {
        var obj = GameObject.FindGameObjectWithTag("PlayerInput");
        if (obj != null && obj.TryGetComponent(out PlayerInput input))
        {
            input.actions["RMB"].performed -= OnRotateItem;
        }
    }

    #region - Drag Handling -
    public void OnBeginDrag(InventoryElement element)
    {
        dragElement = element;
        itemRotationToggled = false;

        // Unparent before updating displays so the element is not destroyed
        dragElement.transform.SetParent(_canvas.transform, true);
        // Move the element out and to the bottom so it's in front of all other objects
        dragElement.transform.SetAsLastSibling();

        // Dragging an item from an equipment slot
        if (dragElement.EquippedSlot != null)
        {
            //dragElement.UpdateSprite(true);
            fromEquipSlot = dragElement.EquippedSlot;
            //dragElement.Rect.sizeDelta = dragElement.Item.Item.Data.Size * UIManager.CellSize;

            Debug.LogWarning("Unequip Item");
            //_equipmentManager.CharacterEquipment.UnequipItem((int)fromEquipSlot.Slot);
        }
        else
        {
            fromDragGrid = hoverGrid;
            hoverGrid.Inventory.RemoveItemAt(dragElement.Item.Origin, dragElement.Item.IsRotated, false);
        }

        SetDragValues(dragElement);
    }

    public void OnDrag(InventoryElement element, Vector2 delta)
    {
        element.Rect.anchoredPosition += delta * _canvas.scaleFactor;

        highlightRect.gameObject.SetActive(hoverGrid != null || equipSlotHover);
        if (hoverGrid == null) return;

        Vector2Int origin = hoverGrid.GetGridPosition(Input.mousePosition) - dragOffset;

        highlightRect.position = gridCorner + origin * InventoryManager.CELL_SIZE;
        // I don't know why, but the highlight is 1 too low if rotated
        if (element.Item.IsRotated) highlightRect.position += Vector3.up * InventoryManager.CELL_SIZE;

        SetHighlightColor(hoverGrid.Inventory.CanPlaceItem(element.Item, origin));
    }

    public void OnEndDrag(InventoryElement element)
    {
        // Early exit if the element was dropped on an equip/attach slot (and destroyed)
        if (dragElement == null) return;
        dragElement = null;

        highlightRect.gameObject.SetActive(false);

        if (hoverGrid == null) // Item was dropped while not over any notable object
        {
            //Debug.Log("hover grid is null");
            ReturnElementToOrigin(element);
            return;
        }

        var origin = hoverGrid.GetGridPosition(Input.mousePosition) - dragOffset;
        //Debug.Log("Trying to place item at " + origin);
        if (hoverGrid.Inventory.TryPlaceItem(element.Item, origin))
        {
            element.EquippedSlot = null;
            Destroy(element.gameObject);
        }
        else
        {
            ReturnElementToOrigin(element);
        }
        fromDragGrid = null;
    }

    private void SetDragValues(InventoryElement element)
    {
        dragOffset.x = Mathf.FloorToInt((Input.mousePosition.x - element.Rect.position.x) / InventoryManager.CELL_SIZE);
        dragOffset.y = Mathf.FloorToInt((Input.mousePosition.y - element.Rect.position.y) / InventoryManager.CELL_SIZE);

        //If the item is rotated, the origin is above the mouse
        if (element.Item.IsRotated)
        {
            dragOffset.y = Mathf.CeilToInt((Input.mousePosition.y - element.Rect.position.y) / InventoryManager.CELL_SIZE);
        }

        // Set the properties for the highlight box
        highlightRect.sizeDelta = element.Rect.sizeDelta;
        highlightRect.rotation = element.Rect.rotation;
    }

    private void OnRotateItem(InputAction.CallbackContext obj)
    {
        if (dragElement == null) return;
        RotateElement(dragElement);
    }

    /// <summary>
    /// Rotates the current dragElement 90 degrees
    /// </summary>
    private void RotateElement(InventoryElement element)
    {
        itemRotationToggled = !itemRotationToggled;
        bool isRotated = element.Item.IsRotated;
        element.Item.IsRotated = !isRotated;

        // Rotate the element forward or back based on if the item is rotated
        if (isRotated) element.Rect.RotateAround(Input.mousePosition, Vector3.forward, 90f);
        else element.Rect.RotateAround(Input.mousePosition, Vector3.forward, -90f);

        SetDragValues(element);
        SetHighlightSlot(element.Rect);
    }
    #endregion

    #region - Drop Handling -
    /// <summary>
    /// Returns the drag element to its previous position
    /// </summary>
    private void ReturnElementToOrigin(InventoryElement element)
    {
        // Return item to the equipped slot it was in
        if (element.EquippedSlot != null)
        {
            Debug.LogWarning("TryEquipItem");
            //_equipmentManager.CharacterEquipment.TryEquipItem(element.Item.Item, (int)element.EquippedSlot.Slot);
        }
        // Return it to its previous position in the inventory
        //else
        {
            // Item was rotated, return to original rotation
            if (itemRotationToggled) element.Item.IsRotated = !element.Item.IsRotated;

            //Debug.Log("TryPlaceItem");
            fromDragGrid.Inventory.TryPlaceItem(element.Item, element.Item.Origin);
        }
        Destroy(element.gameObject);
    }

    /// <summary>
    /// Call to confirm that the dragged element was equipped
    /// </summary>
    public void OnDraggedItemEquipped()
    {
        Destroy(dragElement.gameObject);
        highlightRect.gameObject.SetActive(false);

        dragElement = null;
        fromDragGrid = null;
        fromEquipSlot = null;
    }
    #endregion

    public InventoryElement GetDragElement(InventoryItem item, RectTransform parent)
    {
        var newElement = Instantiate(_elementPrefab, parent);
        newElement.Rect.sizeDelta = item.Size * InventoryManager.CELL_SIZE;

        newElement.SetItem(item);

        return newElement;
    }

    public InventoryElement GetCompactElement(InventoryItem item, RectTransform parent)
    {
        var newElement = Instantiate(_elementPrefab, parent);
        newElement.Rect.anchoredPosition = Vector2.zero;
        newElement.Rect.sizeDelta = parent.sizeDelta;

        // Set element as first in the list, so frame is in front and always index 0
        newElement.transform.SetAsFirstSibling();

        newElement.SetItem(item);
        newElement.UpdateSprite(false);

        return newElement;
    }

    #region - Pointer Hover -
    /// <summary>
    /// Sets the current grid to whichever the mouse is hovering over.
    /// </summary>
    public void OnGridHover(ItemGrid itemGrid)
    {
        if (hoverGrid != null)
        {
            //IDK, maybe toggle off some sort of UI effect
        }
        hoverGrid = itemGrid;
        if (hoverGrid != null)
        {
            gridCorner.x = hoverGrid.Rect.position.x;
            gridCorner.y = hoverGrid.Rect.position.y;
        }
    }

    public void OnEquipSlotHover(EquipmentSlot slot)
    {
        if (slot != null)
        {
            if (dragElement == null) return;
            equipSlotHover = true;
            SetHighlightSlot(slot.Rect);

            //SetHighlightColor(_equipmentManager.CharacterEquipment.CanEquipItem(dragElement.Item.Item, (int)slot.Slot));
        }
        else
        {
            equipSlotHover = false;
            ResetHighlight();
        }
    }
    #endregion

    #region - Highlight -
    private void SetHighlightColor(bool isValid)
    {
        if (isValid) highlightColor.color = green;
        else highlightColor.color = red;
    }

    private void SetHighlightSlot(RectTransform rect)
    {
        highlightRect.gameObject.SetActive(true);

        // Set the properties for the highlight box
        highlightRect.sizeDelta = rect.sizeDelta;
        highlightRect.rotation = rect.rotation;
        highlightRect.position = rect.position;
    }

    private void ResetHighlight()
    {
        if (dragElement == null) return;
        highlightRect.gameObject.SetActive(false);

        // Set the properties for the highlight box
        highlightRect.sizeDelta = dragElement.Rect.sizeDelta;
        highlightRect.rotation = dragElement.Rect.rotation;
    }
    #endregion
}
