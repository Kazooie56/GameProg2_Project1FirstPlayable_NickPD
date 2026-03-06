using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Enemy : Entity
    {
        public int[] DamageRange { get; }
        public string DamageRangeDescription { get; }
        public char Icon { get; set; }
        public int TurnToMove { get; set; }
        public string Type { get; set; } // would be class but classes are a code thing

        public enum MovementStrategy
        {
            Regular,
            ShortSighted,
            Retreat
        }
        public MovementStrategy Movement { get; set; }
        public Enemy(int x, int y, int health, int[] damageRange, string damageRangeDescription, string type, char icon, MovementStrategy movement) : base(x, y, health) // too long and ugly
        {
            DamageRange = damageRange;
            DamageRangeDescription = damageRangeDescription;
            Type = type;
            Icon = icon;
            Movement = movement;
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
        public (int newY, int newX) DecideMove(Player player)
        {
            switch (Movement)
            {
                case MovementStrategy.Regular:
                    return RegularMovePattern(player);

                case MovementStrategy.ShortSighted:
                    return ShortSightedMovePattern(player);

                case MovementStrategy.Retreat:
                    return RetreatMovePattern(player);

                default:
                    return (Y, X);
            }
        }
        public (int newY, int newX) RegularMovePattern(Player player)
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
        public (int newY, int newX) ShortSightedMovePattern(Player player)
        {
            int newY = Y;
            int newX = X;

            int distanceY = Math.Abs(player.Y - Y);
            int distanceX = Math.Abs(player.X - X);

            int sightDistance = distanceY + distanceX;

            if (sightDistance > 1)
                return (Y, X); // returning Y, X means no chasing.

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
        public (int newY, int newX) RetreatMovePattern(Player player)
        {
            int newY = Y;
            int newX = X;

            int distanceY = Math.Abs(player.Y - Y);
            int distanceX = Math.Abs(player.X - X);

            if (distanceY > distanceX)
            {
                if (Y < player.Y) newY--; // swap the increases with negatives and vice versa to run away
                else if (Y > player.Y) newY++;
            }
            else
            {
                if (X < player.X) newX--;
                else if (X > player.X) newX++;
            }

            return (newY, newX);
        }
    }
}
