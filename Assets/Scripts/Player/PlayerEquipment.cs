using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment
{
    private InventoryItem[] _equipment = new InventoryItem[System.Enum.GetNames(typeof(EquipmentType)).Length];
    public InventoryItem[] CurrentEquipment => _equipment;
    
    public void UnequipItem(int slot)
    {
        Debug.LogWarning("This functionality has not been added yet.");


        _equipment[(int)slot] = null;
        // Remove bonuses from wearing it
    }

    public bool TryEquipItem(EquipmentType slot, InventoryItem itemToEquip, out InventoryItem oldEquipment)
    {
        oldEquipment = _equipment[(int)slot];

        if (itemToEquip == null) return false;
        if (itemToEquip.Item == null) return false;
        if (itemToEquip.Item is not Equipment) return false;

        var newEquipment = (Equipment)itemToEquip.Item;
        if (newEquipment.Slot != slot) return false;

        Debug.LogWarning("This functionality has not been added yet.");

        _equipment[(int)slot] = itemToEquip;
        // Apply bonuses from wearing it
        return true;
    }
}
