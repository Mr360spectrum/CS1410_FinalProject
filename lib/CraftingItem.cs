namespace lib;

/// <summary>
/// Represents items that can be used to forge new Weapons or Armor.
/// </summary>
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
