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
        public Player player = new Player(16, 2); // 15th line down from the actual map, not including border, 2nd line to the right.
        public Enemy[] enemies =                // all our enemies with a new type "name"
        { // btw, 1 = furthest left on map, 9 means how many blocks down
            new Enemy(4, 16, 16, new int[]{2}, "2", "Theif", 't', Enemy.MovementStrategy.Retreat),
            new Enemy(12, 16, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),               // Barbarians are the most volatile
            new Enemy(12, 15, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(8, 1, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(17, 14, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),                   // More consistent barbarians
            new Enemy(17, 5, 32, new int[]{3,4,5,6}, "3-6", "Boss Barbarian", 'P', Enemy.MovementStrategy.ShortSighted),     // Stronger than average Barbarian with 2x health
            new Enemy(18, 15, 16, new int[]{2,3,4,5}, "2-5", "Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(17, 16, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),
            new Enemy(17, 15, 12, new int[]{4}, "4", "Mage", '^', Enemy.MovementStrategy.Regular),                            // Higher damage than average but lowest health
            new Enemy(14, 5, 16, new int[]{3,4}, "3-4", "Myrmidon", '/', Enemy.MovementStrategy.Regular),
            new Enemy(14, 6, 16, new int[]{2,3,4,5}, "2-5","Barbarian", 'p', Enemy.MovementStrategy.Regular),
            new Enemy(20, 6, 12, new int[]{4}, "4", "Mage", '^', Enemy.MovementStrategy.Regular)
        };

        static bool isInFort = false;
        static Random random = new Random();
        public Enemy LastEnemyFought = null;
        static bool GameOver = false;
        public GameManager(Map map)
        {
            Map = map;
        }

        public void Draw()
        {
            Console.Clear();
            Map.DrawMap();
            Map.DrawEnemies(enemies);
            player.DrawPlayer();
            Map.DrawPopUp(player, LastEnemyFought);
        }
        public int? GetEnemyIndexAtPosition(int playerY, int playerX)
        {
            // check all enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                // ignore dead ones
                if (enemies[i].Health.Current <= 0)
                    continue;

                // Compare with player coordinates,
                if (enemies[i].Y == playerY &&
                    enemies[i].X == playerX)
                {
                    return i;
                }
            }

            return null;
        }

        public void MovePlayer(ConsoleKey key)
        {
            var (newY, newX) = player.MovePlayer(key);

            // if the place you're trying to walk to isn't ground, an item, or a fort, return
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

            if (!QuestionEvents(newX, newY))
            {
                return;
            }

            if (Map.IsSparkle(newY, newX))
            {
                Map.SparkleCollected = true;
                player.Weapons[3].Repair(1);
                Map.RemoveSparkle(newY, newX);

                return;
            }

            if (Map.IsHealthPotion(newY, newX))
            {
                Map.HealthPotionCollected = true;
                player.Health.Heal(5);
                Map.RemoveHealthPotion(newY, newX);

                return;
            }

            player.SetPosition(newX, newY);      // then set new position, now we can check for sparkles because we want the player to be on top of them first.

            Map.IsInFort = Map.IsFort(player.Y, player.X); // this is the one that sends the message to console AND reduces damage later down the code. Definitely shouldn't be together.
        }
        public void MoveEnemies()
        {
            if (GameOver) return;

            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = enemies[i];

                if (enemy.Health.Current <= 0) // skip dead guys
                    continue;

                var (newY, newX) = enemy.DecideMove(player);

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
        public void StartCombat(int enemyIndex)
        {
            if (GameOver || player.Health.Current <= 0)
                return;

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
            Console.WriteLine($"{enemy.Type} Health: {enemy.Health.Current}\n");

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

            if (Map.IsFort(enemy.Y, enemy.X)) // if the enemy is on a fort, reduce damage by 1.
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
            LastEnemyFought = enemy;

            CheckForDeaths();
            if (!GameOver)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

        }   
        private bool QuestionEvents(int x, int y)
        {

            int mapY = y - 1;
            int mapX = x - 1;

            if (Map._map[y, x] == '?') // only run if the tile is still a ? also why did I make this backwards

            // Event 1
            if (!Map.Event1Done && x == 8 && y == 4)
            {
                Console.WriteLine("The spirits are asking for just 5 points of your health for their assistance. Do you accept?");
                Console.WriteLine("Press Y to accept, any other key to decline");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Y)
                {
                    player.Health.TakeDamage(5);
                    CheckForDeaths();

                    if (player.Health.Current >= 5)
                    {
                        Map._map[3, 7] = '#'; // the question mark
                        Map._map[2, 7] = '#';
                        Map._map[1, 7] = '#';
                        Console.WriteLine("A pile of bones rise from the pits to reveal a new path!");
                        Console.ReadKey(true);
                    }

                    Map.Event1Done = true;  // mark as done
                    return false;
                }
                else
                {
                    Console.WriteLine("You step back and decide not to sacrifice health.");
                    Console.ReadKey(true);
                    return false;
                    }
                }

            // Event 1
            if (!Map.Event1Done && x == 8 && y == 4)
            {
                Console.WriteLine("The spirits are asking for just 5 points of your health for their assistance. Do you accept?");
                Console.WriteLine("Press Y to accept, any other key to decline");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Y)
                {
                    player.Health.TakeDamage(5);
                    CheckForDeaths();

                    if (player.Health.Current >= 5)
                    {
                        Map._map[3, 7] = '#'; // the question mark
                        Map._map[2, 7] = '#';
                        Map._map[1, 7] = '#';
                        Console.WriteLine("A pile of bones rise from the pits to reveal a new path!");
                        Console.ReadKey(true);
                    }

                    Map.Event1Done = true;  // mark as done
                    return false;
                }
                else
                {
                    Console.WriteLine("You step back and decide not to sacrifice health.");
                    Console.ReadKey(true);
                    return false;
                }
            }

            // Event 2
            if (!Map.Event2Done && x == 9 && y == 5)
            {
                Console.WriteLine("The spirits are asking for just 5 points of your health for their assistance. Do you accept?");
                Console.WriteLine("Press Y to accept, any other key to decline");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Y)
                {
                    player.Health.TakeDamage(5);
                    CheckForDeaths();

                    if (player.Health.Current >= 5)
                    {
                        Map._map[4, 8] = '#'; // the question mark
                        Map._map[4, 9] = '#';
                        Map._map[4, 10] = '#';
                        Map._map[4, 11] = '#';
                        Console.WriteLine("A pile of bones rise from the pits to reveal a new path!");
                        Console.ReadKey(true);
                    }

                    Map.Event2Done = true;  // mark as done
                    return false;
                }
                else
                {
                    Console.WriteLine("You step back and decide not to sacrifice health.");
                    Console.ReadKey(true);
                    return false;
                }
            }

            // Event 3
            if (!Map.Event3Done && x == 11 && y == 10)
            {
                Console.WriteLine("The spirits are asking for just 5 points of your health for their assistance. Do you accept?");
                Console.WriteLine("Press Y to accept, any other key to decline");
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Y)
                {
                    player.Health.TakeDamage(5);
                    CheckForDeaths();

                    if (player.Health.Current >= 5)
                    {
                        Map._map[9, 10] = '#'; // the question mark
                        Map._map[9, 11] = '#';
                        Map._map[9, 12] = '#';
                        Map._map[9, 13] = '#';
                        Map._map[9, 14] = '#';
                        Map._map[9, 15] = '#';
                        Map._map[9, 16] = '#';
                        Console.WriteLine("A pile of bones rise from the pits to reveal a new path!");
                        Console.ReadKey(true);
                    }

                    Map.Event3Done = true;  // mark as done
                    return false;
                }
                else
                {
                    Console.WriteLine("You step back and decide not to sacrifice health.");
                    Console.ReadKey(true);
                    return false;
                }
            }

            return true; // normal tiles allow movement
        }
        public bool CheckForDeaths()
        {
            if (player.Health.Current <= 0)
            {
                GameOver = true;
            }

            return GameOver;
        }
    }
}
