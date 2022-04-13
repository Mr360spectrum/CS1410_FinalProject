namespace lib;

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
                throw new EmptyNameException("Player name cannot be set as null or an empty string.");
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
}

public class CraftingItem : Item
{
    public CraftingItem(string inName)
    {
        this.Name = inName;
        this.Equippable = false;
        this.Type = ItemType.Crafting;
    }
    //* Only for JSON deserialization
    public CraftingItem()
    {

    }
}

public class Weapon : Item, IRenameable
{
    private int damageModifier;
    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    private int criticalChance;
    public int CriticalChance { get => criticalChance; set => criticalChance = value; }
    private int criticalModifier;
    public int CriticalModifier { get => criticalModifier; set => criticalModifier = value; }

    public void Rename(string inRename)
    {
        this.name = inRename;
    }

    public Weapon(string inName, int inDamage, int inCriticalChance, int inCriticalModifier)
    {
        this.Name = inName;
        this.Equippable = true;
        this.Type = ItemType.Weapon;
        this.DamageModifier = inDamage;
        this.CriticalChance = inCriticalChance;
        this.CriticalModifier = inCriticalModifier;
    }
    //* Only for JSON deserialization
    public Weapon()
    {

    }
}

public class Armor : Item, IRenameable
{
    private int defenseModifier;
    public int DefenseModifier { get => defenseModifier; set => defenseModifier = value; }

    public void Rename(string inRename)
    {
        this.name = inRename;
    }

    public Armor(string inName, int inDefense)
    {
        this.Name = inName;
        this.Equippable = true;
        this.type = ItemType.Armor;
        this.DefenseModifier = inDefense;
    }
    //* Only for JSON deserialization
    public Armor()
    {

    }
}