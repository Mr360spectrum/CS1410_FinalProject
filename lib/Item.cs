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
        set
        {
            equippable = value;
        }
    }
    protected ItemType type;
    public ItemType Type
    {
        get => type;
        protected set
        {
            type = value;
        }
    }
    public enum ItemType
    {
        Crafting,
        Weapon,
        Armor
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
    public void Rename(string inRename)
    {
        this.name = inRename;
    }

    public Weapon(string inName)
    {
        this.Name = inName;
        this.Equippable = true;
        this.Type = ItemType.Weapon;
    }
    //* Only for JSON deserialization
    public Weapon()
    {

    }
}

public class Armor : Item, IRenameable
{
    public void Rename(string inRename)
    {
        this.name = inRename;
    }

    public Armor(string inName)
    {
        this.Name = inName;
        this.Equippable = true;
        this.type = ItemType.Armor;
    }
    //* Only for JSON deserialization
    public Armor()
    {
        
    }
}