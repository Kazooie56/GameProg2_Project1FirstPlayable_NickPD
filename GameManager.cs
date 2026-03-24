using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    // Future to do list:
    // find a way to add weapons instead of hardcoding it.
    // have the weapon choices not be repeated code
    // 
    public class GameManager
    {
        public Map Map;
        public Player player;
        public EnemyManager enemyManager;
        public ItemManager ItemManager;

        static bool isInFort = false;
        static Random random = new Random();
        public Enemy LastEnemyFought = null;
        public string LastMessage { get; set; }
        static bool GameOver = false;
        public GameManager(Map map, ItemManager itemManager)
        {
            Map = map;
            ItemManager = itemManager;
        }
        public void Init()
        {
            Map = new Map("map.txt", ItemManager);
            player = new Player(16, 2, "Player", new Health(40)); // 15th line down from the actual map, not including border, 2nd line to the right.
            enemyManager = new EnemyManager();
            LastEnemyFought = null;
            GameOver = false;
        }
        public void Update(ConsoleKey? input = null)
        {
            if (GameOver) return;

            if (input.HasValue)
                MovePlayer(input.Value);

            MoveEnemies();

            CheckForDeaths();
        }
        public void Draw()
        {
            Console.Clear();
            Map.DrawMap();
            Map.DrawEnemies(enemyManager.enemies);
            player.DrawPlayer();
            Map.DrawPopUp(player, this, LastEnemyFought);

            Console.CursorVisible = false;
        }
        // ADD A DEBUG METHOD TO MOVE PLAYERS TO SPECIFIC POINTS ON THE MAP
        public int? GetEnemyIndexAtPosition(int playerY, int playerX)
        {
            // check all enemies
            for (int i = 0; i < enemyManager.enemies.Length; i++)
            {
                // ignore dead ones
                if (enemyManager.enemies[i].Health.Current <= 0)
                    continue;

                // Compare with player coordinates,
                if (enemyManager.enemies[i].Y == playerY &&
                    enemyManager.enemies[i].X == playerX)
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

            Item item = Map.GetItemAt(newY, newX);

            if (item != null)
            {
                item.OnCollect(player, this);
                if (item.RemoveOnCollect)
                {
                    Map.RemoveItem(item);
                }
                return;
            }

            player.SetPosition(newX, newY);      // then set new position, if we don't want to adjust the position

            Map.IsInFort = Map.IsFort(player.Y, player.X); // this is the one that sends the message to console AND reduces damage later down the code. Definitely shouldn't be together.
        }
        public void MoveEnemies()
        {
            if (GameOver == true) return;

            for (int i = 0; i < enemyManager.enemies.Length; i++)
            {
                var enemy = enemyManager.enemies[i];

                if (!enemy.IsAlive()) // skip dead guys
                    continue;

                var (newY, newX) = enemy.Move(player);

                // Check if enemy would move onto player
                if (newY == player.Y && newX == player.X)
                {
                    StartCombat(i);
                    continue; // don't move enemy onto player
                }

                // Check if another enemy is there
                if (Map.IsWalkable(newY, newX) &&
                    GetEnemyIndexAtPosition(newY, newX) == null)
                {
                    enemy.SetPosition(newX, newY);
                }
            }
        }
        public void StartCombat(int enemyIndex)
        {
            if (GameOver || !player.IsAlive())
                return;

            Draw();

            CheckForDeaths();

            var enemy = enemyManager.enemies[enemyIndex];
            if (!enemyManager.enemies[enemyIndex].IsAlive())
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

            if (!enemy.IsAlive())
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

        public bool CheckForDeaths()
        {
            if (!player.IsAlive())
            { 
                GameOver = true;
            }

            return GameOver;
        }
    }
}
