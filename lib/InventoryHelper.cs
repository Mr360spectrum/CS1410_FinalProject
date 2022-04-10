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
}