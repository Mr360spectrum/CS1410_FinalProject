using System;
using lib;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = GameHelper.GetGame();
            Console.WriteLine(game.GetPlayerName());
            while (true)
            {
                game.Play();
            }
        }
    }
}
