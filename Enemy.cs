using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Enemy : Character
    {
        public int[] DamageRange { get; }
        public string DamageRangeDescription { get; }
        public int TurnToMove { get; set; }
        public Enemy(int x, int y, int health, int[] damageRange, string damageRangeDescription, string type, char icon) : base(x, y, icon, type, new Health(health))
        {
            DamageRange = damageRange;
            DamageRangeDescription = damageRangeDescription;
        }
        public void DrawEnemy()
        {
            if (Health.Current <= 0)
                return;

            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{Icon}");
            Console.ResetColor();
        }
        public virtual (int newY, int newX) Move(Player player)
        {
            int newY = Y;
            int newX = X;

            int distanceY = Math.Abs(player.Y - Y); //Math.Abs means negatives and positives don't count towards the value
            int distanceX = Math.Abs(player.X - X);

            // enemies move to reach the player by traveling the furthest direction first.
            if (distanceY > distanceX)
            {
                if (Y < player.Y) newY++;
                else if (Y > player.Y) newY--;
            }
            else
            {
                if (X < player.X) newX++;
                else if (X > player.X) newX--;
            }

            return (newY, newX);
        }
        public void TryMove(Player player, Map map)
        {
            if (!IsAlive())
                return;

            var (newY, newX) = Move(player);

            if (map.IsWalkable(newY, newX))
            {
                SetPosition(newX, newY);
            }
        }
    }
}