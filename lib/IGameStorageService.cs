using System.Text.Json;

namespace lib;

/// <summary>
/// The interface that storage services must implement to be used by GameHelper.
/// </summary>
public interface IGameStorageService
{
    void SaveGame(Game inGame);
    Game LoadGame(string saveName);
}

/// <summary>
/// The storage service that uses the JSON serializer to save/load Game objects on disk.
/// </summary>
public class OnDiskGameStorageService : IGameStorageService
{
    /// <summary>
    /// Reads all text from each JSON file stored in the selected folder and deserializes
    /// them into their respective objects.
    /// </summary>
    /// <param name="saveName">The name of the folder in which the selected save is stored.</param>
    /// <returns>A Game object containing the deserialized information.</returns>
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

    /// <summary>
    /// Serializes and stores the provided Game object in multiple JSON files in a folder
    /// in the "saves" folder at the project root.
    /// </summary>
    /// <param name="inGame">The Game object to serialize and save.</param>
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

/// <summary>
/// The storage service used in unit tests. Games are only stored in memory and are never written to disk.
/// </summary>
public class InMemoryGameStorageService : IGameStorageService
{
    private Game game;
    /// <summary>
    /// Gets the Game object stored in the game variable.
    /// </summary>
    /// <param name="saveName">Not used in this class.</param>
    /// <returns>The Game object stored in the game variable.</returns>
    public Game LoadGame(string saveName)
    {
        return game;
    }

    /// <summary>
    /// Assigns the game variable to the provided Game object.
    /// </summary>
    /// <param name="inGame">The Game object to be stored.</param>
    public void SaveGame(Game inGame)
    {
        game = inGame;
    }
}