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
        static Map map;     // this is our map reference

        static Enemy[] enemies =                // all our enemies with a new type "name"
        {
            new Enemy(1, 9, 16, new int[]{2,3,4,5}, "2-5", "Barbarian"),        // Barbarians are the most volatile
            new Enemy(7, 10, 16, new int[]{2,3,4,5}, "2-5", "Barbarian"),
            new Enemy(4, 11, 16, new int[]{3,4}, "3-4", "Myrmidon"),            // More consistent barbarians
            new Enemy(8, 2, 32, new int[]{3,4,5,6}, "3-6", "Boss Barbarian"),   // Stronger than average Barbarian with 2x health
            new Enemy(6, 3, 16, new int[]{2,3,4,5}, "2-5", "Barbarian"),
            new Enemy(7, 4, 16, new int[]{3,4}, "3-4", "Myrmidon"),
            new Enemy(8, 3, 12, new int[]{4}, "4", "Mage"),                     // Higher damage than average but lowest health
            new Enemy(9, 4, 16, new int[]{3,4}, "3-4", "Myrmidon"),
            new Enemy(10, 3, 16, new int[]{2,3,4,5}, "2-5","Barbarian"),
            new Enemy(10, 7, 12, new int[]{4}, "4", "Mage")
        };

        static Player player = new Player(15, 2); // 15th line down from the actual map, not including border, 2nd line to the right.

        static bool isInFort = false;           // dumb "if inside fort" bool // FIX WHY IS IT IN HALF THE CLASSES?

        static Random random = new Random();

        static bool GameOver = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            map = new Map("map.txt");

            while (GameOver == false)
            {
                Console.Clear();
                map.DrawAll(player.Y, player.X, enemies);

                ConsoleKey key = Console.ReadKey(true).Key;

                MovePlayer(key);
                MoveEnemies();
            }
        }
        static void MovePlayer(ConsoleKey key)
        {
            var (newY, newX) = player.MovePlayer(key);

            // if the place you're trying to move is within the map's boundaries, and it is either the ground, sparkle, or a fort
            if (!map.IsWalkable(newY, newX))
            {
                return;
            }

            int? enemyIndex = GetEnemyIndexAtPosition(newY, newX);

            // stop the player from moving if an enemy is there first
            if (enemyIndex != null)
            {
                StartCombat(enemyIndex.Value);
                return;
            }

            player.SetPosition(newX, newY);      // then set new position, now we can check for sparkles because we want the player to be on top of them first.

            isInFort = map.IsFort(player.Y, player.X);
            if (map.IsFort(player.Y, player.X))
            {
                Map.IsInFort = true;
            }

            if (map.IsSparkle(player.Y, player.X))
            {
                player.Weapons[3].Repair(1);

                map.RemoveSparkle(newY, newX);
                Map.SparkleCollected = true;
            }
        }
        static void MoveEnemies()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = enemies[i];

                if (enemy.Health <= 0) // skip dead guys
                    continue;

                int newY = enemy.Y;
                int newX = enemy.X;

                // try moving vertically first
                if (enemy.Y < player.Y)
                {
                    newY++;
                }
                else if (enemy.Y > player.Y)
                {
                    newY--;
                }

                // then horizontally, it can do both in one turn // FIX SHOULD BE A METHOD BY ENEMY AND ALSO 1 MOVEMENT PER TURN
                if (enemy.X < player.X)
                {
                    newX++;
                }
                else if (enemy.X > player.X)
                {
                    newX--;
                }

                // our future positions - 1 on both axis because of borders
                int newMapY = newY - 1;
                int newMapX = newX - 1;

                if (map.IsWalkable(newY, newX))
                {
                    // Check if enemy would move onto player
                    if (newY == player.Y && newX == player.X)
                    {
                        StartCombat(i);
                        continue; // don't move enemy onto player
                    }

                    // Check if another enemy is there
                    if (GetEnemyIndexAtPosition(newY, newX) == null)
                    {
                        enemy.Y = newY;
                        enemy.X = newX;
                    }
                }
            }
        }
        static int? GetEnemyIndexAtPosition(int playerY, int playerX) // I have to use int? because sometimes there wont be an enemy there
        {
            // check all enemies
            for (int i = 0; i < enemies.Length; i++)     
            {
                // ignore dead ones
                if (enemies[i].Health <= 0)
                    continue; 

                // Compare with player coordinates, not map indices
                if (enemies[i].Y == playerY &&
                    enemies[i].X == playerX)
                {
                    return i;
                }
            }

            return null;
        }
        static void StartCombat(int enemyIndex)
        {
            Console.Clear();
            map.DrawAll(player.Y, player.X, enemies);

            CheckForDeaths();

            var enemy = enemies[enemyIndex];
            if (enemies[enemyIndex].Health <= 0)
            {
                return;
            }

            Console.SetCursorPosition(0, map.Rows + 2);
            Console.Write(new string(' ', Console.WindowWidth));

            Console.WriteLine($"A {enemy.Type} attacks!");
            Console.WriteLine($"Player Health: {player.Health.Current} | {enemy.Type} Health: {enemy.Health}\n");

            for (int i = 0; i < player.Weapons.Count; i++)
            {
                var weapon = player.Weapons[i];

                string durabilityText;
                if (weapon.IsBroken())
                {
                    durabilityText = "Broken";
                }
                else
                {
                    
                    durabilityText = weapon.Durability.ToString();
                }

                string damageText;
                if (weapon.DamageRange.Length == 1)
                {
                    damageText = weapon.DamageRange[0].ToString();
                }
                else
                {
                    damageText = $"{weapon.DamageRange.First()}-{weapon.DamageRange.Last()}";
                }

                Console.WriteLine($"{i + 1}: {weapon.Name}: Damage: {damageText}. Durability: {durabilityText}");
            }

            Console.WriteLine($"\nThe enemy is going to deal {enemy.DamageRangeDescription} next turn. What will you use?\n");

            int playerDamage = 0;
            string weaponName = "";

            while (true)
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        if (player.Weapons[0].IsBroken())
                        {
                            Console.WriteLine("This weapon is unusable in its current state!");
                            continue;
                        }
                        playerDamage = player.Weapons[0].Attack(random);
                        weaponName = player.Weapons[0].Name;
                        break;

                    case "2":
                        if (player.Weapons[1].IsBroken())
                        {
                            Console.WriteLine("This weapon is unusable in its current state!");
                            continue;
                        }
                        playerDamage = player.Weapons[1].Attack(random);
                        weaponName = player.Weapons[1].Name;
                        break;

                    case "3":
                        if (player.Weapons[2].IsBroken())
                        {
                            Console.WriteLine("This weapon is unusable in its current state!");
                            continue;
                        }
                        playerDamage = player.Weapons[2].Attack(random);
                        weaponName = player.Weapons[2].Name;
                        break;

                    case "4":
                        if (player.Weapons[3].IsBroken())
                        {
                            Console.WriteLine("This weapon is unusable in its current state!");
                            continue;
                        }
                        playerDamage = player.Weapons[3].Attack(random);
                        weaponName = player.Weapons[3].Name;
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        continue;
                }

                break; // valid choice made, exit loop
            }

            if (map.IsFort(enemy.Y, enemy.X))
            {
                playerDamage -= 1;
            }

            enemy.Health -= playerDamage;

            Console.WriteLine($"\nPlayer used {weaponName} and dealt {playerDamage} damage!");

            if (enemy.Health <= 0)
            {
                Console.WriteLine($"You defeated {enemy.Type}!");
            }
            else
            {
                int enemyDamage = enemy.DamageRange[random.Next(enemy.DamageRange.Length)];
                if (isInFort)
                {
                    enemyDamage -= 1; // reduce by 1
                }
                Console.WriteLine($"The {enemy.Type} counterattacks and deals {enemyDamage}!");
                player.Health.TakeDamage(enemyDamage);
            }

            enemies[enemyIndex].Health = enemy.Health;

            CheckForDeaths();
            if (!GameOver)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        static void CheckForDeaths() // I think this belong in main because it affects the game not player?
        {
            if (player.Health.Current <= 0)
            {
                Console.WriteLine($"Game Over.");
                Console.WriteLine("Press any key to close the game.");
                Console.ReadKey();
                GameOver = true;
            }
        }
    }
}
