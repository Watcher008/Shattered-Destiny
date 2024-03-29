using SD.Characters;
using UnityEngine;
using SD.Inventories;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private EquipmentSlot[] equipmentSlots;
    public PlayerData PlayerData => _playerData;

    private void Awake()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].onItemDropped += TryEquipDroppedItem;
        }

        _playerData.PlayerEquip.onEquipmentChanged += RefreshDisplay;
    }

    private void OnEnable()
    {
        RefreshDisplay();
    }

    private void OnDestroy()
    {
        _playerData.PlayerEquip.onEquipmentChanged -= RefreshDisplay;
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
            if (equipmentSlots[i].Rect.childCount > 0)
            {
                Destroy(equipmentSlots[i].Rect.GetChild(0).gameObject);
            }
        }
    }

    private void TryEquipDroppedItem(EquipmentType slot, InventoryElement element)
    {
        if (_playerData.PlayerEquip.TryEquipItem(slot, element.Item, out _))
        {
            element.Item.IsRotated = false;
            DragManager.Instance.OnDraggedItemEquipped();
        }
    }
}
