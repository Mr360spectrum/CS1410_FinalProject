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

/// <summary>
/// Represents Weapon items to be used to attacking.
/// </summary>
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

    public override string ToString()
    {
        return "Atk: " + DamageModifier + " CritChance: " + CriticalChance + " CritDmg: " + CriticalModifier;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Weapon);
    }

    public bool Equals(Weapon otherWeapon)
    {
        if (otherWeapon == null)
        {
            return false;
        }

        return this.Name == otherWeapon.Name &&
                this.DamageModifier == otherWeapon.DamageModifier &&
                this.CriticalChance == otherWeapon.CriticalChance &&
                this.CriticalModifier == otherWeapon.CriticalModifier;
    }
}

/// <summary>
/// Represents Armor objects to be used for defense.
/// </summary>
public class Armor : Item, IRenameable
{
    private int defenseModifier;
    public int DefenseModifier { get => defenseModifier; set => defenseModifier = value; }

    private int dodgeChance;
    public int DodgeChance { get => dodgeChance; set => dodgeChance = value;}

    private int attackBonus;
    public int AttackBonus { get => attackBonus; set => attackBonus = value; }

    public void Rename(string inRename)
    {
        this.name = inRename;
    }

    public Armor(string inName, int inDefense, int inDodge, int inBonus)
    {
        this.Name = inName;
        this.Equippable = true;
        this.type = ItemType.Armor;

        this.DefenseModifier = inDefense;
        this.DodgeChance = inDodge;
        this.AttackBonus = inBonus;
    }
    //* Only for JSON deserialization
    public Armor()
    {

    }

    public override string ToString()
    {
        return "Def: " + DefenseModifier + " Dodge: " + DodgeChance + " AtkBonus: " + AttackBonus;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Armor);
    }

    public bool Equals(Armor otherArmor)
    {
        if (otherArmor == null)
        {
            return false;
        }

        return this.Name == otherArmor.Name && 
                this.DefenseModifier == otherArmor.DefenseModifier &&
                this.DodgeChance == otherArmor.DodgeChance &&
                this.AttackBonus == otherArmor.AttackBonus;
    }
}