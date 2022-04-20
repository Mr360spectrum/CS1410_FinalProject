using System;
using lib;
using System.Text.Json;
using System.IO;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            GameHelper helper = new GameHelper(new OnDiskGameStorageService());
            Game game = helper.GetGame();

            game.Play();
        }
    }
}
