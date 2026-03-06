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
        static Map map;     // this is our map reference.
        static GameManager gameManager;
        static bool GameOver = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            map = new Map("map.txt");

            gameManager = new GameManager(map);

            while (GameOver == false)
            {
                gameManager.Draw();

                ConsoleKey key = Console.ReadKey(true).Key;

                gameManager.MovePlayer(key);
                gameManager.MoveEnemies();

                GameOver = gameManager.CheckForDeaths();
            }

            Console.WriteLine($"Game Over.");
            Console.WriteLine("Press any key to close the game.");
            Console.ReadKey();
        }
    }
}
