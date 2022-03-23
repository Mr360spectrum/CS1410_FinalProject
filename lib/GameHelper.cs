namespace lib;

public class GameHelper : IHelper
{
    public static Game DisplayMainMenu()
    {
        var defaultColor = ConsoleColor.DarkGreen;
        var highlightColor = ConsoleColor.DarkRed;
        string[] options = new string[3]{"New Game", "Load Game", "Exit"};
        int cursorPos = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(@"
      :::::::::  :::::::::   ::::::::          ::::::::      :::       :::   :::   :::::::::: 
     :+:    :+: :+:    :+: :+:    :+:        :+:    :+:   :+: :+:    :+:+: :+:+:  :+:         
    +:+    +:+ +:+    +:+ +:+               +:+         +:+   +:+  +:+ +:+:+ +:+ +:+          
   +#++:++#:  +#++:++#+  :#:               :#:        +#++:++#++: +#+  +:+  +#+ +#++:++#      
  +#+    +#+ +#+        +#+   +#+#        +#+   +#+# +#+     +#+ +#+       +#+ +#+            
 #+#    #+# #+#        #+#    #+#        #+#    #+# #+#     #+# #+#       #+# #+#             
###    ### ###         ########          ########  ###     ### ###       ### ##########    

");
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
                    if (cursorPos == 2)
                    {
                        cursorPos = 0;
                    }
                    else
                    {
                        cursorPos++;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (cursorPos == 0)
                    {
                        cursorPos = 2;
                    }
                    else
                    {
                        cursorPos--;
                    }
                    break;
                case ConsoleKey.Enter:
                    switch (cursorPos)
                    {
                        case 0:
                            return StartNewGame();
                        case 1:
                            return LoadGame();
                        case 2:
                            System.Environment.Exit(1);
                            break;
                    }
                    break;
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