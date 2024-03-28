using SD.Characters;
using UnityEngine;
using SD.Inventories;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private EquipmentSlot[] equipmentSlots;

    private void Awake()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].onItemDropped += TryEquipDroppedItem;
        }

        _playerData.onEquipmentChanged += RefreshDisplay;
    }

    private void OnEnable()
    {
        RefreshDisplay();
    }

    private void OnDestroy()
    {
        _playerData.onEquipmentChanged -= RefreshDisplay;
    }

    public void RefreshDisplay()
    {
        ClearDisplay();

        for (int i = 0; i < _playerData.PlayerEquip.CurrentEquipment.Length; i++)
        {
            if (_playerData.PlayerEquip.CurrentEquipment[i] == null) continue;

            var item = _playerData.PlayerEquip.CurrentEquipment[i];
            var element = DragManager.Instance.GetDragElement(item, equipmentSlots[i].Rect);
            element.EquippedSlot = equipmentSlots[i];
        }
    }

    private void ClearDisplay()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            // Each equipment slot has an image and TMP_Text GameObject children
            if (equipmentSlots[i].Rect.childCount > 2)
            {
                // UI_Elements set as children are placed as first siblings
                Destroy(equipmentSlots[i].Rect.GetChild(0).gameObject);
            }
        }
    }

    private void TryEquipDroppedItem(EquipmentType slot, InventoryElement element)
    {
        if (_playerData.PlayerEquip.TryEquipItem(slot, element.Item, out var oldEquipment))
        {
            element.Item.IsRotated = false;
            DragManager.Instance.OnDraggedItemEquipped();

            Debug.LogWarning("Need to add old equipment to inventory");
        }
    }
}
