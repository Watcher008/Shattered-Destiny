[System.Serializable]
public class Equipment : Item
{
    public EquipmentType Slot;
}

public enum EquipmentType
{
    Body,
    Boots
}