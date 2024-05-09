using System.Collections.Generic;

[System.Serializable]
public class Equipment : Item
{
    public EquipmentType Slot;
    public EquipEffect[] Effects;
}

[System.Serializable]
// Because fuck me, that's why
public class EquipEffect
{
    public string Stat;
    public int Value;
}

public enum EquipmentType
{
    Body,
    Boots
}