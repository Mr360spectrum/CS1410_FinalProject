namespace lib;

public abstract class Item
{
    public string Name
    {
        get; protected set;
    }
    public bool Equippable { get; protected set; }
    protected ItemType type;
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
        this.type = ItemType.Crafting;
    }
}

public class Weapon : Item, IRenameable
{
    public void Rename(string inRename)
    {
        this.Name = inRename;
    }

    public Weapon(string inName)
    {
        this.Name = inName;
        this.Equippable = true;
        this.type = ItemType.Weapon;
    }
}