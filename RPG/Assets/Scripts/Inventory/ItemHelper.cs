using System;

public enum ItemId
{
    Helmet = 1,
    Chest,
    Boots,
    Shield,
    Leggins,
    Sword,
    Knife,
    ChestPlate,
    Halberd
}

[Serializable]
public class Stat
{
    public StatType StatType;
    public int Amount;
}

public enum StatType
{
    Default,
    HP,
    MP,
    Strengh,
    Agility,
    Intelligence,
    Armor,
    Damage,
    AttackSpeed
}

public enum EquipmentType
{
    Weapon = 1,
    Shield,
    Helmet,
    Chest,
    Leggins,
    Boots
}


