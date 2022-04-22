using System.Text.Json;

namespace lib;

public interface IGameStorageService
{
    void SaveGame(Game inGame);
    Game LoadGame(string saveName);
}

public class OnDiskGameStorageService : IGameStorageService
{
    public Game LoadGame(string saveName)
    {
        string playerDataStr = File.ReadAllText($"../saves/{saveName}/game.json");
        string weaponsDataStr = File.ReadAllText($"../saves/{saveName}/weapons.json");
        string armorDataStr = File.ReadAllText($"../saves/{saveName}/armor.json");
        string craftingDataStr = File.ReadAllText($"../saves/{saveName}/crafting.json");

        var weaponsList = JsonSerializer.Deserialize<List<Weapon>>(weaponsDataStr);
        var armorList = JsonSerializer.Deserialize<List<Armor>>(armorDataStr);
        var craftingList = JsonSerializer.Deserialize<List<CraftingItem>>(craftingDataStr);

        List<Item> items = new List<Item>();
        foreach (var w in weaponsList)
        {
            items.Add(w as Item);
        }
        foreach (var a in armorList)
        {
            items.Add(a as Armor);
        }
        foreach (var c in craftingList)
        {
            items.Add(c as CraftingItem);
        }

        var game = JsonSerializer.Deserialize<Game>(playerDataStr);
        return new Game(game.player.Name, items, game.player.EquippedWeapon, game.player.EquippedArmor);
    }

    public void SaveGame(Game inGame)
    {
        string playerSavePath = $"../saves/{inGame.player.Name}";

        if (!Directory.Exists(playerSavePath))
        {
            Directory.CreateDirectory(playerSavePath);
        }

        //Save current game information
        string gameJson = JsonSerializer.Serialize<Game>(inGame);
        File.WriteAllText(Path.Combine(playerSavePath, "game.json"), gameJson);

        //Save inventory items separately
        var (weaponsJson, armorJson, craftingJson) = InventoryHelper.GetJson(inGame.player.Inventory);
        File.WriteAllText(Path.Combine(playerSavePath, "weapons.json"), weaponsJson);
        File.WriteAllText(Path.Combine(playerSavePath, "armor.json"), armorJson);
        File.WriteAllText(Path.Combine(playerSavePath, "crafting.json"), craftingJson);
    }
}

public class InMemoryGameStorageService : IGameStorageService
{
    private Game game;
    public Game LoadGame(string saveName)
    {
        return game;
    }

    public void SaveGame(Game inGame)
    {
        game = inGame;
    }
}