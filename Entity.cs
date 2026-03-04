using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Entity
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Health Health { get; private set; }

        public Entity(int x, int y, int health) 
        {
            Y = y;
            X = x;
            Health = new Health(health);
        }

        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }
    }
}
