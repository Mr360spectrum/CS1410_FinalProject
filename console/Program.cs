using System;
using lib;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = GameHelper.GetGame();

            //!REMOVE
            GameHelper.Save(game);
            while (true)
            {
                game.Play();
            }
        }
    }
}
