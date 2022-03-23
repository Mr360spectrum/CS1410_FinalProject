namespace lib;

public class GameHelper : IHelper
{
    public static Game GetGame()
    {
        string[] options = new string[3] { "New Game", "Load Game", "Exit" };

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
            Console.WriteLine("Use the arrow keys to navigate.");
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

    private static int DisplayMenu(string logo, string[] options)
    {
        var defaultColor = ConsoleColor.DarkGreen;
        var highlightColor = ConsoleColor.DarkRed;
        int cursorPos = 0;
        int maxPos = options.Length - 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(logo);
            Console.WriteLine("Use the arrow keys to navigate.");
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

    private static Game LoadGame()
    {
        //!REMOVE
        Console.WriteLine("Loading!");
        //TODO: Implement universal menu method that takes in an array of options
        return new Game();
    }

    private static Game StartNewGame()
    {
        //!REMOVE
        Console.WriteLine("Starting a new game...");
        return new Game();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}

public class Game
{

}