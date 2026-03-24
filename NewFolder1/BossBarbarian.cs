using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class BossBarbarian : Enemy
    {
        public BossBarbarian(
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
            "Boss Barbarian",
            'P'
            )
        {

        }

        public override (int newY, int newX) Move(Player player)
        {
            int newY = Y;
            int newX = X;

            int distanceY = Math.Abs(player.Y - Y);
            int distanceX = Math.Abs(player.X - X);

            int sightDistance = distanceY + distanceX;

            if (sightDistance > 1)
            {
                return (Y, X); // returning Y, X means no chasing.
            }

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
