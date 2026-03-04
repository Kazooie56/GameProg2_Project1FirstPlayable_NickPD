using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class GameManager
    {
        public Map Map;     // this is our map reference
        public Player player = new Player(15, 2); // 15th line down from the actual map, not including border, 2nd line to the right.
        public Enemy[] enemies =                // all our enemies with a new type "name"
        { // btw, 1 = furthest left on map, 9 means how many blocks down
            new Enemy(1, 9, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),               // Barbarians are the most volatile
            new Enemy(7, 10, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(4, 11, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),                   // More consistent barbarians
            new Enemy(8, 2, 32, new int[]{3,4,5,6}, "3-6", "Boss Barbarian", 'P', Enemy.MovementStrategy.ShortSighted),     // Stronger than average Barbarian with 2x health
            new Enemy(6, 3, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(7, 4, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),
            new Enemy(8, 3, 12, new int[]{4}, "4", "Mage", '^', Enemy.MovementStrategy.Regular),                            // Higher damage than average but lowest health
            new Enemy(9, 4, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),
            new Enemy(10, 3, 16, new int[]{2,3,4,5}, "2-5","Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(10, 7, 12, new int[]{4}, "4", "Mage", '^', Enemy.MovementStrategy.Regular)
        };

        static bool isInFort = false;
        static Random random = new Random();
        static bool GameOver = false;
        public GameManager(Map map)
        {
            Map = map;
        }

        public void Draw()
        {
            Console.Clear();
            Map.DrawAll(player.Y, player.X, enemies);
            player.DrawPlayer();
        }

        public void MovePlayer(ConsoleKey key)
        {
            var (newY, newX) = player.MovePlayer(key);

            // if the place you're trying to move is within the map's boundaries, and it is either the ground, sparkle, or a fort
            if (!Map.IsWalkable(newY, newX))
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

            isInFort = Map.IsFort(player.Y, player.X);

            if (Map.IsFort(player.Y, player.X))
            {
                Map.IsInFort = true;
            }
            else
            {
                Map.IsInFort = false;
            }

            if (Map.IsSparkle(player.Y, player.X))
            {
                player.Weapons[3].Repair(1);

                Map.RemoveSparkle(newY, newX);
                Map.SparkleCollected = true;
            }
        }
        public void MoveEnemies()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = enemies[i];

                if (enemy.Health.Current <= 0) // skip dead guys
                    continue;

                var (newY, newX) = enemy.RegularMovePattern(player);

                if (Map.IsWalkable(newY, newX))
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
                        enemy.SetPosition(newX, newY);
                    }
                }
            }
        }
        public int? GetEnemyIndexAtPosition(int playerY, int playerX) // I have to use int? because sometimes there wont be an enemy there
        {
            // check all enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                // ignore dead ones
                if (enemies[i].Health.Current <= 0)
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
        public void StartCombat(int enemyIndex)
        {
            Draw();

            CheckForDeaths();

            var enemy = enemies[enemyIndex];
            if (enemies[enemyIndex].Health.Current <= 0)
            {
                return;
            }

            Console.SetCursorPosition(0, Map.Rows + 3);
            Console.Write(new string(' ', Console.WindowWidth));

            Console.WriteLine($"A {enemy.Type} attacks!");
            Console.WriteLine($"Player Health: {player.Health.Current} | {enemy.Type} Health: {enemy.Health.Current}\n");

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

            if (Map.IsFort(enemy.Y, enemy.X))
            {
                playerDamage -= 1;
            }

            enemy.Health.TakeDamage(playerDamage);

            Console.WriteLine($"\nPlayer used {weaponName} and dealt {playerDamage} damage!");

            if (enemy.Health.Current <= 0)
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

            CheckForDeaths();
            if (!GameOver)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

        }
        public bool CheckForDeaths() // I think this belong in main because it affects the game not player?
        {
            if (player.Health.Current <= 0)
            {
                GameOver = true;
            }

            return GameOver;
        }
    }
}
