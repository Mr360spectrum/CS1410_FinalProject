namespace lib;

/// <summary>
/// Represents the object that is responsible for loading and saving Game objects
/// by using an instance of a class that implements the IGameStorageService interface.
/// Also contains other helpful methods for managing Game objects.
/// </summary>
public class GameHelper
{
    public static ConsoleColor DefaultColor = ConsoleColor.DarkGreen;
    public static ConsoleColor HighlightColor = ConsoleColor.DarkRed;
    private IGameStorageService StorageService {get; set;}

    public GameHelper(IGameStorageService inStorageService)
    {
        this.StorageService = inStorageService;
    }

    /// <summary>
    /// Finds the names of the subdirectories in the "saves" folder at the project root.
    /// </summary>
    /// <returns>A List of type String containing the names of the save folders.</returns>
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

    /// <summary>
    /// Gets a by using the LoadGame() method provided by StorageService.
    /// </summary>
    /// <param name="inName">The name of the save to load in.</param>
    /// <returns>A Game object returned by the LoadGame() method in StorageService.</returns>
    public Game Load(string inName)
    {
        return StorageService.LoadGame(inName);
    }

    /// <summary>
    /// Uses the SaveGame() method provided by StorageService to store the Game object.
    /// </summary>
    /// <param name="inGame">The Game object to be stored.</param>
    public void Save(Game inGame)
    {
        StorageService.SaveGame(inGame);
    }

    /// <summary>
    /// Creates a Game object used for testing.
    /// </summary>
    /// <returns>A Game object containing information useful for testing.</returns>
    public Game GenerateTestGame()
    {
        return new Game("testgame", new List<Item> { new Weapon("Weapon 1", 5, 3, 2), new Weapon("Weapon 2", 4, 1, 1), new Armor("Armor 1", 3, 1, 1), new CraftingItem("Craft 1") }, null, null);
    }
}
