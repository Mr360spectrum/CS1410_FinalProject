namespace lib;

public abstract class Item
{
    public string Name
    {
        get
        {
            if (Name == "" || Name is null)
            {
                throw new EmptyNameException("'Name' returned null or an empty string.");
            }
            return Name;
        }
        protected set
        {
            if (value == "" || value is null)
            {
                throw new EmptyNameException("Item name cannot be set to null or an empty string.");
            }
        }
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
    public void Rename()
    {

    }

    public Weapon(string inName)
    {
        this.Name = inName;
        this.Equippable = true;
        this.type = ItemType.Weapon;
    }
}