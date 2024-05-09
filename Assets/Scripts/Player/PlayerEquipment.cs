using UnityEngine;
using SD.Characters;

public class PlayerEquipment
{
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChanged;

    private PlayerData _playerData;

    private InventoryItem[] _equipment = new InventoryItem[System.Enum.GetNames(typeof(EquipmentType)).Length];
    public InventoryItem[] CurrentEquipment => _equipment;
    
    public PlayerEquipment(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void UnequipItem(int slot)
    {
        OnItemUnequipped(_equipment[(int)slot]);
        _equipment[(int)slot] = null;
        // Remove bonuses from wearing it
        onEquipmentChanged?.Invoke();
    }

    public bool TryEquipItem(EquipmentType slot, InventoryItem itemToEquip, out InventoryItem oldEquipment)
    {
        oldEquipment = _equipment[(int)slot];

        if (!ItemIsValid(slot, itemToEquip)) return false;
        // Make sure that current equipment can be placed back in inventory
        if (oldEquipment != null && !_playerData.Inventory.TryFitItem(oldEquipment)) return false;

        OnItemEquipped((Equipment)itemToEquip.Item);
        _equipment[(int)slot] = itemToEquip;

        onEquipmentChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Returns true if the given item can be equipped in the given slot.
    /// </summary>
    private bool ItemIsValid(EquipmentType slot, InventoryItem itemToEquip)
    {
        if (itemToEquip == null) return false;
        if (itemToEquip.Item == null) return false;
        if (itemToEquip.Item is not Equipment) return false;

        var newEquipment = (Equipment)itemToEquip.Item;
        if (newEquipment.Slot != slot) return false;

        return true;
    }

    /// <summary>
    /// Handles applying the effects granted by the new equipment.
    /// </summary>
    private void OnItemEquipped(Equipment newEquip)
    {
        //Debug.Log("This functionality has not been fully added yet.");

        // So here is where I run into some minor issues. 
        // There are the stats that only the player has, and then the stats shared by all characters
        // But also only the player can wear equipment, so.... I don't know if it's worth adding a space for modifiers to stats... I think so

        foreach (var effect in newEquip.Effects)
        {
            if (effect.Stat == "PHYS") _playerData.PlayerStats.AddModifier(Attributes.Physicality, effect.Value);
            else if (effect.Stat == "INT") _playerData.PlayerStats.AddModifier(Attributes.Intelligence, effect.Value);
            else if (effect.Stat == "SURV") _playerData.PlayerStats.AddModifier(Attributes.Survival, effect.Value);
            else if (effect.Stat == "SOC") _playerData.PlayerStats.AddModifier(Attributes.Social, effect.Value);
        }
    }

    /// <summary>
    /// Handles removing the effects lost from the old equipment
    /// </summary>
    private void OnItemUnequipped(InventoryItem oldItem)
    {
        if (oldItem == null || oldItem.Item == null) return;
        //Debug.Log("This functionality has not been fully added yet.");

        var oldEquip = (Equipment)oldItem.Item;

        foreach(var effect in oldEquip.Effects)
        {
            if (effect.Stat == "PHYS") _playerData.PlayerStats.RemoveModifier(Attributes.Physicality, effect.Value);
            else if (effect.Stat == "INT") _playerData.PlayerStats.RemoveModifier(Attributes.Intelligence, effect.Value);
            else if (effect.Stat == "SURV") _playerData.PlayerStats.RemoveModifier(Attributes.Survival, effect.Value);
            else if (effect.Stat == "SOC") _playerData.PlayerStats.RemoveModifier(Attributes.Social, effect.Value);
        }
    }
}
