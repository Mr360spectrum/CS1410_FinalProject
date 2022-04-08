using System.Text.Json;
using System.IO;

namespace lib;


public class GameHelper
{
    public static ConsoleColor defaultColor = ConsoleColor.DarkGreen;
    public static ConsoleColor highlightColor = ConsoleColor.DarkRed;

    public static Game GetGame()
    {
        var options = new List<string>() { "New Game", "Load Game", "Exit" };

        while (true)
        {
            Console.Clear();
            string logo = @"
      :::::::::  :::::::::   ::::::::          ::::::::      :::       :::   :::   :::::::::: 
     :+:    :+: :+:    :+: :+:    :+:        :+:    :+:   :+: :+:    :+:+: :+:+:  :+:         
    +:+    +:+ +:+    +:+ +:+               +:+         +:+   +:+  +:+ +:+:+ +:+ +:+          
   +#++:++#:  +#++:++#+  :#:               :#:        +#++:++#++: +#+  +:+  +#+ +#++:++#      
  +#+    +#+ +#+        +#+   +#+#        +#+   +#+# +#+     +#+ +#+       +#+ +#+            
 #+#    #+# #+#        #+#    #+#        #+#    #+# #+#     #+# #+#       #+# #+#             
###    ### ###         ########          ########  ###     ### ###       ### ##########    

";
            int selection = DisplayMenu(logo, options);
            switch (selection)
            {
                case 0:
                    return StartNewGame();
                case 1:
                    return LoadGame();
                case 2:
                    System.Environment.Exit(1);
                    break;
            }
        }
    }

    private static int DisplayMenu(string logo, List<string> options, string message = "")
    {

        int cursorPos = 0;
        int maxPos = options.Count - 1;

        while (true)
        {
            //TODO: Have the cursor move back and rewrite the highlighted 
            //line instead of clearing the whole screen to prevent flickering
            Console.Clear();
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(logo);
            Console.WriteLine("Use the arrow keys to navigate.");
            if (message != "")
            {
                Console.WriteLine(message);
            }
            foreach (var option in options)
            {
                if (options[cursorPos] == option)
                {
                    Console.ForegroundColor = highlightColor;
                    Console.WriteLine($" > {option} <");
                    Console.ForegroundColor = defaultColor;
                }
                else
                {
                    Console.WriteLine($"   {option}");
                }
            }

            var key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    //Loop back to top if "down" is pressed while cursor is at bottom
                    if (cursorPos == maxPos)
                    {
                        cursorPos = 0;
                    }
                    //Move down
                    else
                    {
                        cursorPos++;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    //Loop back to bottom if "up" is pressed while cursor is at top
                    if (cursorPos == 0)
                    {
                        cursorPos = maxPos;
                    }
                    //Move up
                    else
                    {
                        cursorPos--;
                    }
                    break;
                case ConsoleKey.Enter:
                    return cursorPos;
            }
        }
    }

    public static Game LoadGame()
    {
        var loadLogo = "LOAD";
        var fileOptions = GetSaves();
        if (fileOptions.Count == 0)
        {
            Console.WriteLine("There are no saved games.");
            Console.WriteLine("Press a key to create a new game.");
            Console.ReadKey();
            return GameHelper.StartNewGame();
        }
        var nameSelection = DisplayMenu(loadLogo, fileOptions, "Select a saved game.");

        return DeserializeGame(fileOptions[nameSelection]);
    }

    public static Game DeserializeGame(string fileName)
    {
        var jsonString = File.ReadAllText(fileName);
        var game = JsonSerializer.Deserialize<Game>(jsonString);
        return game;
    }

    public static List<string> GetSaves()
    {
        //TODO: Save individual JSON files for each type of item in inventory and save to a separate folder using the player's name

        string projectDirectory = System.IO.Directory.GetCurrentDirectory();
        var loadDir = new DirectoryInfo(projectDirectory);

        List<string> folders = new List<string>();
        foreach (var folder in loadDir.GetDirectories())
        {
            if (folder.Name != "bin" && folder.Name != "obj")
            {
                folders.Add(folder.Name);
            }
        }

        return folders;
    }

    public static void Save(Game inGame)
    {
        string playerSavePath = $"../saves/{inGame.player.Name}";

        try
        {
            if (!Directory.Exists(playerSavePath))
            {
                Directory.CreateDirectory(playerSavePath);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while saving.");
            Console.WriteLine(ex.Message);
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

    private static Game StartNewGame()
    {
        //!REMOVE
        Console.Clear();
        Console.WriteLine("Starting a new game...");
        Console.WriteLine("What's your name?");
        string name = Console.ReadLine();
        return new Game(name);
    }


    public static Game GenerateTestGame()
    {
        return new Game("testgame", new List<Item> { new Weapon("Weapon 1"), new Weapon("Weapon 2"), new Armor("Armor 1"), new CraftingItem("Craft 1") });
    }
}
