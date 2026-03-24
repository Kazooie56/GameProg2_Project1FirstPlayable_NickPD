using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Thief : Enemy
    {
        public Thief(
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
            "Thief",
            't'
        )
        {

        }
        public override (int newY, int newX) Move(Player player)
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
