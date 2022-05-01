namespace lib;

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
