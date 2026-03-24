using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Map
    {
        // map class handles the map.

        // usage: map[y, x] MAP IS 15 by 15 vertical

        // map legend:
        // ▓ = ground:      walkable
        // ▒ = water:       not walkable
        // ░ = walls:       also not walkable
        // █ = fort:        makes any friend or foe within these walls take 1 less damage
        // * = sparkles:    pick up a leftover weapon with a single use
        // + = hp potion:   single use heal for 5 health.

        public char[,] _map;
        private List<Item> _items = new List<Item>();
        public int Rows
        {
            get { return _map.GetLength(0); }
        }
        public int Cols
        {
            get { return _map.GetLength(1); }
        }
        public static bool IsInFort { get; set; }           // dumb "if inside fort" bool
        public Map(string path, ItemManager itemManager)
        {
            MakeMap(path, itemManager);
        }
        private void MakeMap(string path, ItemManager itemManager)
        {
            string[] lines = File.ReadAllLines(path);   // turns each line from the map file we have in our project into a string array. we call it lines.
            
            int rows = lines.Length;                    // This is the vertical rows
            int cols = lines[0].Length;                 // This is the horizontal rows

            // Create the 2D array
            // this makes the map consist of a new map where the number for each row and column
            // makes the tuple for the 2d array. (might be outdated description from old project,
            // fix description if time)
            _map = new char[rows, cols];           

            // Fills the rows and columns to draw the map
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    _map[y, x] = lines[y][x];
                }
            }

            foreach (var item in itemManager.AllItems)
            {
                AddItem(item);
            }
        }
        // REDRAW ONLY THE STUFF THAT GETS UPDATED
        public void DrawMap()
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
                        case '#':   // ground but bones if you believe
                        case '+':   // hp potion
                        case '?':   // question
                        case '*':   // collectable, also doesn't need a different foreground color because it's shared.
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case '▒':   // water
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case '░':   // walls
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
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
        }
        public void DrawEnemies(Enemy[] enemies) // draws enemies
        {
            foreach (var enemy in enemies)
            {
                enemy.DrawEnemy();
            }

            Console.ResetColor();
        }
        // draws map pop ups
        public void DrawPopUp(Player player, GameManager gameManager, Enemy lastEnemy = null)
        {
            int messageLine = Rows + 2; // +2 means it's just underneath the border's bottom.

            Console.SetCursorPosition(0, messageLine);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Player Health: {player.Health.Current}");
            Console.ResetColor();

            if (lastEnemy != null)
            {
                Console.ResetColor();
                Console.Write($" | ");
                Console.ForegroundColor = ConsoleColor.Red;
                if (!lastEnemy.IsAlive())
                {
                    Console.Write($"{lastEnemy.Type} Health: Defeated.");
                }
                else
                {
                    Console.WriteLine($"{lastEnemy.Type} Health: {lastEnemy.Health.Current} Power: {lastEnemy.DamageRangeDescription}");
                }
                Console.ResetColor();
            }
            else
            {
                // Leave enemy portion blank if no enemy fought
                Console.WriteLine("                                     ");
            }

            messageLine++;

            if (!string.IsNullOrEmpty(gameManager.LastMessage))
            {
                Console.SetCursorPosition(0, messageLine);
                Console.WriteLine(gameManager.LastMessage);
                Console.Write("Press any key to continue.");
                Console.ReadKey(true);

                // Clear
                Console.SetCursorPosition(0, messageLine);
                Console.WriteLine(new string(' ', Console.WindowWidth));
                Console.Write(new string(' ', Console.WindowWidth));

                gameManager.LastMessage = null;
            }

            if (IsInFort == true)
            {
                Console.SetCursorPosition(0, messageLine);
                Console.Write("Warriors take 1 less damage while inside the fort!");    // FIX this message now goes away, but it's implementation still sucks
            }
            else
            {
                Console.SetCursorPosition(0, messageLine);
                Console.Write(new string(' ', Console.WindowWidth));
            }
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
            return tile == '▓' || tile == '*' || tile == '+' || tile == '█' || tile == '#' || tile == '?';
        }
        private bool InBounds(int y, int x) // might be irrelevant
        {
            return y >= 0 && y < Rows &&
                   x >= 0 && x < Cols;
        }
        public Item GetItemAt(int y, int x)
        {
            return _items.FirstOrDefault(i => i.X == x && i.Y == y);
        }
        public void AddItem(Item item)
        {
            _items.Add(item);

            int mapY = item.Y - 1;
            int mapX = item.X - 1;

            _map[mapY, mapX] = item.Icon;
        }
        public void RemoveItem(Item item)
        {
            _items.Remove(item);

            int mapY = item.Y - 1;
            int mapX = item.X - 1;

            _map[mapY, mapX] = item.ReplacementTile;
        }
        public bool IsFort(int y, int x)
        {
            // every space on our map adjusting for borders
            int mapY = y - 1;
            int mapX = x - 1;

            // will be a fort if it has this space.
            return _map[mapY, mapX] == '█';
        }
    }
}