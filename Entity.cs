using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public abstract class Entity
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Icon { get; set; }
        public Entity(int x, int y, char icon) 
        {
            Y = y;
            X = x;
            Icon = icon;
        }
        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }
    }
}
