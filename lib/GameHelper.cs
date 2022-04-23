using System.Text.Json;
using System.IO;

namespace lib;


public class GameHelper
{
    public static ConsoleColor DefaultColor = ConsoleColor.DarkGreen;
    public static ConsoleColor HighlightColor = ConsoleColor.DarkRed;
    private IGameStorageService StorageService {get; set;}

    public GameHelper(IGameStorageService inStorageService)
    {
        this.StorageService = inStorageService;
    }

    public static List<string> GetSaves()
    {
        var savesArray = System.IO.Directory.GetDirectories("../saves");
        var savesList = new List<string>();
        foreach (var save in savesArray)
        {
            //Remove initial part of path
            savesList.Add(save.Remove(0, 9));
        }

        return savesList;
    }

    public Game Load(string inName)
    {
        return StorageService.LoadGame(inName);
    }

    public void Save(Game inGame)
    {
        StorageService.SaveGame(inGame);
    }

    public Game GenerateTestGame()
    {
        return new Game("testgame", new List<Item> { new Weapon("Weapon 1", 5, 3, 2), new Weapon("Weapon 2", 4, 1, 1), new Armor("Armor 1", 3, 1, 1), new CraftingItem("Craft 1") }, null, null);
    }
}
