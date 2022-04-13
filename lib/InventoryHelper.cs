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
}