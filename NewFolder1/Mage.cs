using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Mage : Enemy
    {
        public Mage(
            int x,
            int y,
            int health,
            int[] damageRange,
            string damageRangeDescription
        ) : base(
            x,
            y,
            health,
            damageRange,
            damageRangeDescription,
            "Mage",
            '^'
            )
        {

        }
        public override (int newY, int newX) Move(Player player)
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
    }
}
