using System.Text.Json;

namespace lib;

/// <summary>
/// Represents the methods that are responsible for splitting the inventory,
/// serializing the inventory, and combining weapons.
/// </summary>
public class InventoryHelper
{
    /// <summary>
    /// Serializes the inventory into separate files based on the derived class.
    /// </summary>
    /// <param name="inInv">The inventory (List of type Item) to serialize.</param>
    /// <returns>A tuple of strings containing each JSON string.</returns>
    public static (string, string, string) GetJson(List<Item> inInv)
    {
        var (weaponsList, armorList, craftingList) = InventoryHelper.SplitInventory(inInv);
        string weaponsJsonStr = JsonSerializer.Serialize<List<Weapon>>(weaponsList);
        string armorJsonStr = JsonSerializer.Serialize<List<Armor>>(armorList);
        string craftingJsonStr = JsonSerializer.Serialize<List<CraftingItem>>(craftingList);

        return (weaponsJsonStr, armorJsonStr, craftingJsonStr);
    }

    /// <summary>
    /// Splits the inventory into separate Lists based on the derived class.
    /// </summary>
    /// <param name="inInv">The inventory (List of type Item) to split.</param>
    /// <returns>A tuple of Lists of the objects of the classes that inherit from Item.</returns>
    public static (List<Weapon>, List<Armor>, List<CraftingItem>) SplitInventory(List<Item> inInv)
    {
        var weaponsSaveList = new List<Weapon>();
        var armorSaveList = new List<Armor>();
        var craftingSaveList = new List<CraftingItem>();

        foreach (var item in inInv)
        {
            switch (item.Type)
            {
                case Item.ItemType.Weapon:
                    weaponsSaveList.Add(item as Weapon);
                    break;
                case Item.ItemType.Armor:
                    armorSaveList.Add(item as Armor);
                    break;
                default:
                    craftingSaveList.Add(item as CraftingItem);
                    break;
            }
        }

        return (weaponsSaveList, armorSaveList, craftingSaveList);
    }

    /// <summary>
    /// Combines two Weapons by keeping the name of the first Weapon and the highest value between the two
    /// Weapons for the selected attribute and gets the average for the rest of the attributes.
    /// </summary>
    /// <param name="weapon1">The first Weapon to combine. The name of the returned Weapon is
    /// retrieved from this Weapon.</param>
    /// <param name="weapon2">The second Weapon to combine.</param>
    /// <param name="selectedAttr">The attribute to keep the highest value of.</param>
    /// <returns>A Weapon object with properties based on the two provided Weapons.</returns>
    public static Weapon CombineWeapons(Weapon weapon1, Weapon weapon2, Item.WeaponAttributes selectedAttr)
    {
        int newDamageModifier;
        int newCriticalChance;
        int newCriticalModifier;

        switch (selectedAttr)
        {
            case Item.WeaponAttributes.Attack:
                newDamageModifier = weapon1.DamageModifier > weapon2.DamageModifier ? weapon1.DamageModifier : weapon2.DamageModifier;
                newCriticalChance = (weapon1.CriticalChance + weapon2.CriticalChance) / 2;
                newCriticalModifier = (weapon1.CriticalModifier + weapon2.CriticalModifier) / 2;

                return new Weapon(weapon1.Name, newDamageModifier, newCriticalChance, newCriticalModifier);
            case Item.WeaponAttributes.CriticalChance:
                newDamageModifier = (weapon1.DamageModifier + weapon2.DamageModifier) / 2;
                newCriticalChance = weapon1.CriticalChance > weapon2.CriticalChance ? weapon1.CriticalChance : weapon2.CriticalChance;
                newCriticalModifier = (weapon1.CriticalModifier + weapon2.CriticalModifier) / 2;

                return new Weapon(weapon1.Name, newDamageModifier, newCriticalChance, newCriticalModifier);
            case Item.WeaponAttributes.CriticalModifier:
                newDamageModifier = (weapon1.DamageModifier + weapon2.DamageModifier) / 2;
                newCriticalChance = (weapon1.CriticalChance + weapon2.CriticalChance) / 2;
                newCriticalModifier = weapon1.CriticalModifier > weapon2.CriticalModifier ? weapon1.CriticalModifier : weapon2.CriticalModifier;

                return new Weapon(weapon1.Name, newDamageModifier, newCriticalChance, newCriticalModifier);
            default:
                //This should never be called because the only possible options for selectedAttr are already accounted for
                return new Weapon("Impossible weapon", 0, 0, 0);
        }
    }

    /// <summary>
    /// Combines two Armor objects by keeping the name of the first Armor object and the highest value between the two
    /// Armor objects for the selected attribute and gets the average for the rest of the attributes.
    /// </summary>
    /// <param name="armor1">The first Armor to combine. The name of the returned Armor object is
    /// retrieved from this Armor.</param>
    /// <param name="armor2">The second Armor to combine.</param>
    /// <param name="selectedAttr">The attribute to keep the highest value of.</param>
    /// <returns>An Armor object with properties based on the two provided Armor objects.</returns>
    public static Armor CombineArmor(Armor armor1, Armor armor2, Item.ArmorAttributes selectedAttr)
    {
        int newDefenseModifier;
        int newDodgeChance;
        int newAttackBonus;

        switch (selectedAttr)
        {
            case Item.ArmorAttributes.Defense:
                newDefenseModifier = armor1.DefenseModifier > armor2.DefenseModifier ? armor1.DefenseModifier : armor2.DefenseModifier;
                newDodgeChance = (armor1.DodgeChance + armor2.DodgeChance) / 2;
                newAttackBonus = (armor1.AttackBonus + armor2.AttackBonus) / 2;
                return new Armor(armor1.Name, newDefenseModifier, newDodgeChance, newAttackBonus);
            case Item.ArmorAttributes.DodgeChance:
                newDefenseModifier = (armor1.DefenseModifier + armor2.DefenseModifier) / 2;
                newDodgeChance = armor1.DodgeChance > armor2.DodgeChance ? armor1.DodgeChance : armor2.DodgeChance;
                newAttackBonus = (armor1.AttackBonus + armor2.AttackBonus) / 2;
                return new Armor(armor1.Name, newDefenseModifier, newDodgeChance, newAttackBonus);
            case Item.ArmorAttributes.AttackBonus:
                newDefenseModifier = (armor1.DefenseModifier + armor2.DefenseModifier) / 2;
                newDodgeChance = (armor1.DodgeChance + armor2.DodgeChance) / 2;
                newAttackBonus = armor1.AttackBonus > armor2.AttackBonus ? armor1.AttackBonus : armor2.AttackBonus;
                return new Armor(armor1.Name, newDefenseModifier, newDodgeChance, newAttackBonus);
            default:
                //This should never be called because the only possible options for selectedAttr are already accounted for
                return new Armor("Impossible armor", 0, 0, 0);
        }
    }

    /// <summary>
    /// Creates a new Armor object with random stats. Used when combining sticks in the inventory.
    /// </summary>
    /// <param name="name">The name of the new Armor object.</param>
    /// <returns>An Armor object with the provided name and random stats.</returns>
    public static Armor ForgeArmor(string name)
    {
        var rand = new Random();
        int defenseModifier = rand.Next(1, 30);
        int dodgeChance = rand.Next(1, 10);
        int attackBonus = rand.Next(1, 5);

        return new Armor(name, defenseModifier, dodgeChance, attackBonus);
    }

    /// <summary>
    /// Creates a new Weapon object with random stats. Used when combining sticks in the inventory.
    /// </summary>
    /// <param name="name">The name of the new Weapon object.</param>
    /// <returns>A Weapon object with the provided name and random stats.</returns>
    public static Weapon ForgeWeapon(string name)
    {
        var rand = new Random();
        int attack = rand.Next(1, 10);
        int criticalChance = rand.Next(1, 20);
        int criticalModifier = rand.Next(1, 5);

        return new Weapon(name, attack, criticalChance, criticalModifier);
    }

    /// <summary>
    /// Creates a new Armor object with random stats. Used when generating Armor for chests.
    /// </summary>
    /// <param name="name">The name of the new Armor object.</param>
    /// <returns>An Armor object with the provided name and random stats.</returns>
    public static Armor CreateNewArmorInChest()
    {
        var rand = new Random();
        int defenseModifier = rand.Next(5, 40);
        int dodgeChance = rand.Next(5, 20);
        int attackBonus = rand.Next(2, 5);

        return new Armor("New Armor", defenseModifier, dodgeChance, attackBonus);
    }

    /// <summary>
    /// Creates a new Weapon object with random stats. Used when generating Weapons for chests.
    /// </summary>
    /// <param name="name">The name of the new Weapon object.</param>
    /// <returns>A Weapon object with the provided name and random stats.</returns>
    public static Weapon CreateNewWeaponInChest()
    {
        var rand = new Random();
        int attack = rand.Next(5, 15);
        int criticalChance = rand.Next(5, 30);
        int criticalModifier = rand.Next(3, 8);

        return new Weapon("New Weapon", attack, criticalChance, criticalModifier);
    }
}