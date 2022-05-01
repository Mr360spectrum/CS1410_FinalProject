namespace lib;

/// <summary>
/// That abstract class that each Item type inherits from.
/// </summary>
public abstract class Item
{
    protected string name;
    public string Name
    {
        get => name;
        set
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Item name cannot be set as null or an empty string.");
            }
            name = value;
        }
    }

    protected bool equippable;
    public bool Equippable
    {
        get => equippable;
        set => equippable = value;
    }

    protected ItemType type;
    public ItemType Type
    {
        get => type;
        set => type = value;
    }

    public enum ItemType
    {
        Crafting,
        Weapon,
        Armor
    }

    public enum WeaponAttributes
    {
        Attack,
        CriticalChance,
        CriticalModifier
    }

    public enum ArmorAttributes
    {
        Defense,
        DodgeChance,
        AttackBonus
    }
}
