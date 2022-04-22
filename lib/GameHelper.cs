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

    public Game GetGame()
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
                    return LoadGameFromMenu();
                case 2:
                    System.Environment.Exit(1);
                    break;
            }
        }
    }

    private int DisplayMenu(string logo, List<string> options, string message = "")
    {
        int cursorPos = 0;
        int maxPos = options.Count - 1;

        while (true)
        {
            //line instead of clearing the whole screen to prevent flickering
            Console.Clear();
            Console.ForegroundColor = DefaultColor;
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
                    Console.ForegroundColor = HighlightColor;
                    Console.WriteLine($" > {option} <");
                    Console.ForegroundColor = DefaultColor;
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

    public Game LoadGameFromMenu()
    {
        var loadLogo = "LOAD";
        var fileOptions = GetSaves();
        if (fileOptions.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("There are no saved games.");
            Console.WriteLine("Press a key to create a new game.");
            Console.ReadKey();
            return this.StartNewGame();
        }
        var nameSelection = DisplayMenu(loadLogo, fileOptions, "Select a saved game.");

        return this.Load(fileOptions[nameSelection]);
    }

    public List<string> GetSaves()
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

    private Game StartNewGame()
    {
        //!REMOVE
        Console.Clear();
        Console.WriteLine("Starting a new game...");
        Console.WriteLine("What's your name?");
        string name;
        while (true)
        {
            name = Console.ReadLine();
            if (name == null || name == "")
            {
                Console.WriteLine("That is an invalid name.");
                continue;
            }
            return new Game(name);
        }
    }

    public Game GenerateTestGame()
    {
        return new Game("testgame", new List<Item> { new Weapon("Weapon 1", 5, 3, 2), new Weapon("Weapon 2", 4, 1, 1), new Armor("Armor 1", 3, 1, 1), new CraftingItem("Craft 1") }, null, null);
    }
}
