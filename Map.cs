using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    internal class Map
    {
        // map class handles the map.

        // usage: map[y, x] MAP IS 15 by 15 vertical

        // map legend:
        // ▓ = ground:      walkable
        // ▒ = water:       not walkable
        // ░ = walls:       also not walkable
        // █ = fort:        makes any friend or foe within these walls take 1 less damage
        // * = sparkles:    pick up a leftover weapon with a single use

        private char[,] _map;
        public int Rows
        {
            get { return _map.GetLength(0); }
        }
        public int Cols
        {
            get { return _map.GetLength(1); }
        }

        public static bool SparkleCollected { get; set; }
        public static bool IsInFort { get; set; }           // dumb "if inside fort" bool

        public Map(string path)
        {
            MakeMap(path);
        }

        private void MakeMap(string path)
        {
            string[] lines = File.ReadAllLines(path);   // turns each line from the map file we have in our project into a string array. we call it lines.

            int rows = lines.Length;                    // This is the vertical rows
            int cols = lines[0].Length;                 // This is the horizontal rows

            // Create the 2D array
            _map = new char[rows, cols];                 // this makes the map consist of a new map where the number for each row and column makes the tuple for the 2d array. (might be outdated description from old project, fix if time)

            // Fills the rows and columns to draw the map
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    _map[y, x] = lines[y][x];
                }
            }
        }
        public void DrawAll(int playerY, int playerX, Enemy[] enemies)     // draws map, player, and enemies
        {
            // Top border
            Console.Write("+");
            for (int i = 0; i < Cols; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine("+");

                for (int y = 0; y < Rows; y++)
                {
                    // Left border
                    Console.Write("|");         

                    for (int x = 0; x < Cols; x++)
                    {
                        char tile = _map[y, x];

                        // For each tile we draw we're gonna change it's color
                        switch (tile)
                        {
                            case '▓':   // ground
                            case '*':   // collectable, also doesn't need a different foreground color because it's shared.
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case '▒':   // water
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case '░':   // walls
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case '█':   // fort
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            default:
                                Console.ResetColor();
                                break;
                        }
                        // draw tile and reset color for each tile
                        Console.Write(tile);                    
                        Console.ResetColor();
                    }
                    // Right border
                    Console.WriteLine("|");                     
                }

            // Bottom border
            Console.Write("+");                                 
            for (int i = 0; i < Cols; i++)
            Console.Write("-");
            Console.WriteLine("+");

            // Draw player
            Console.SetCursorPosition(playerX, playerY);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0");
            Console.ResetColor();

            int messageLine = Rows + 2; // writing this here so that I don't need to make it two more times in each if statement below

            if (SparkleCollected == true)
            {
                Console.SetCursorPosition(0, messageLine);
                Console.Write("Your Silver Sword has gained a little more endurance!");         // MAKE IT SO THE PLAYER PAUSES WHEN COLLECTING AN ITEM
                Thread.Sleep(1000); // show message briefly

                // Clear the message
                Console.SetCursorPosition(0, messageLine);
                Console.Write("                                                     ");

                SparkleCollected = false;
            }

            if (IsInFort == true)
            {
                Console.SetCursorPosition(0, messageLine);
                Console.Write("Warriors take 1 less damage while inside the fort!");    // FIX this message never goes away, Also it's implementation sucks
            }
            else
            {
                Console.SetCursorPosition(0, messageLine);
                Console.Write("                                                     ");
            }

            // Draw enemies
            foreach (var enemy in enemies)
            {
                if (enemy.Health <= 0)
                    continue;

                Console.SetCursorPosition(enemy.X, enemy.Y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X");
            }

            Console.ResetColor();
        }

        public bool IsWalkable(int y, int x)
        {
            // This is to account for the border
            int mapY = y - 1;
            int mapX = x - 1;

            if (!InBounds(mapY, mapX))
            {
                return false;
            }

            char tile = _map[mapY, mapX];
            return tile == '▓' || tile == '*' || tile == '█';
        }
        private bool InBounds(int y, int x) // might be irrelevant
        {
            return y >= 0 && y < Rows &&
                   x >= 0 && x < Cols;
        }

        public bool IsSparkle(int y, int x)
        {
            int mapY = y - 1;
            int mapX = x - 1;

            return _map[mapY, mapX] == '*';
        }

        public bool IsFort(int y, int x)
        {
            int mapY = y - 1;
            int mapX = x - 1;

            return _map[mapY, mapX] == '█';
        }

        public void RemoveSparkle(int y, int x)
        {
            int mapY = y - 1;
            int mapX = x - 1;

            if (_map[mapY, mapX] == '*')
                _map[mapY, mapX] = '▓';
        }
    }
}
