using System.Text.Json;

namespace lib;

public class InventoryHelper
{
    public static (string, string, string) GetJson(List<Item> inInv)
    {
        var (weaponsList, armorList, craftingList) = InventoryHelper.SplitInventory(inInv);
        string weaponsJsonStr = JsonSerializer.Serialize<List<Weapon>>(weaponsList);
        string armorJsonStr = JsonSerializer.Serialize<List<Armor>>(armorList);
        string craftingJsonStr = JsonSerializer.Serialize<List<CraftingItem>>(craftingList);

        return (weaponsJsonStr, armorJsonStr, craftingJsonStr);
    }

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

    public static Armor ForgeArmor(string name)
    {
        var rand = new Random();
        int defenseModifier = rand.Next(1, 30);
        int dodgeChance = rand.Next(1, 10);
        int attackBonus = rand.Next(1, 5);

        return new Armor(name, defenseModifier, dodgeChance, attackBonus);
    }

    public static Weapon ForgeWeapon(string name)
    {
        var rand = new Random();
        int attack = rand.Next(1, 10);
        int criticalChance = rand.Next(1, 20);
        int criticalModifier = rand.Next(1, 5);

        return new Weapon(name, attack, criticalChance, criticalModifier);
    }
}