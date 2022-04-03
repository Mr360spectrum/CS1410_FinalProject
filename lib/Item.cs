namespace lib;

public abstract class Item
{
    protected string name;
    public string Name
    {
        get => name;
        set
        {
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
        set
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
        this.name = inName;
        this.equippable = false;
        this.type = ItemType.Crafting;
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
        this.name = inName;
        this.equippable = true;
        this.type = ItemType.Weapon;
    }
    //* Only for JSON deserialization
    public Weapon()
    {

    }
}