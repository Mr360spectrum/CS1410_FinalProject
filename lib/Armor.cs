namespace lib;

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