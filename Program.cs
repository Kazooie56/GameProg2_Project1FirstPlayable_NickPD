using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    // Maybe have an entity class
    // Entity -> character
    // character -> enemy
    // enemy -> each class
    internal class Program
    {
        //static Map map;     // this is our map reference.
        //static GameManager gameManager;
        //static bool GameOver = false;

        static void Main(string[] args)
        {
            ItemManager itemManager = new ItemManager();
            GameManager gameManager = new GameManager(null, itemManager); // map will be initialized in Init
            gameManager.Init(); // initialize once at the start

            while (!gameManager.CheckForDeaths())
            {
                gameManager.Draw(); // render map and characters

                ConsoleKey key = Console.ReadKey(true).Key; // get player input

                gameManager.Update(key); // handle movement, enemies, combat
            }

            Console.Clear();
            Console.WriteLine("Game Over.");
            Console.WriteLine("Press any key to close the game.");
            Console.ReadKey();
        }
    }
}
