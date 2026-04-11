using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ItemManager itemManager = new ItemManager();
            GameManager gameManager = new GameManager(null, itemManager);
            gameManager.Init(); // initialize once at the start

            while (!gameManager.CheckForDeaths())
            {
                gameManager.Draw();

                ConsoleKey key = Console.ReadKey(true).Key; // get player input

                gameManager.Update(key); // player then enemies move
            }

            Console.Clear();
            Console.WriteLine("Game Over.");
            Console.WriteLine("Press any key to close the game.");
            Console.ReadKey();
        }
    }
}
