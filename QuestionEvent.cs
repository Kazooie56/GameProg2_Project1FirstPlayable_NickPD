using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class QuestionEvent : Item
    {
        public (int startY, int startX, int endY, int endX) BonesTiles { get; set; }

        public QuestionEvent(int x, int y, (int, int, int, int) bonesTiles) : base(x, y, '?')
        {
            CollectMessage = "The spirits are asking for just 5 points of your health for their assistance. Do you accept? \nPress Y to accept, any other key to decline.";
            BonesTiles = bonesTiles;
        }

        public override void OnCollect(Player player, GameManager gameManager)
        {
            // I don't know why this is needed when it works fine for health potions and sparkles
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine(CollectMessage);

            var input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.Y)
            {
                player.Health.TakeDamage(5);
                gameManager.CheckForDeaths();

                if (player.Health.Current >= 5)
                {
                    // draw bones from start to end
                    for (int y = BonesTiles.startY; y <= BonesTiles.endY; y++)
                    {
                        for (int x = BonesTiles.startX; x <= BonesTiles.endX; x++)
                        {
                            gameManager.Map._map[y, x] = '#';
                        }

                    }
                    Console.WriteLine("A pile of bones rises from the pits to reveal a new path!");
                    Console.ReadKey(true);
                    RemoveOnCollect = true;
                }
            }
            else
            {
                Console.WriteLine("You step back and decide not to sacrifice health.");
                Console.ReadKey(true);
                RemoveOnCollect = false;

            }
        }
    }
}
