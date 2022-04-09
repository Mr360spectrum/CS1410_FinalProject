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
            var game = GameHelper.GetGame();
            game.Play();
        }
    }
}
