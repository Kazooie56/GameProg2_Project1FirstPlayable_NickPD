using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    internal class Enemy
    {
        //enemy class handles the enemy.
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int[] DamageRange { get; }
        public string DamageRangeDescription { get; }
        public string Type { get; set; } // may name them boss, barbarian etc, would be class but classes are a code thing

        public bool isInFort = false;

        public Enemy(int x, int y, int health, int[] damageRange, string damageRangeDescription, string type)
        {
            X = x;
            Y = y;
            Health = health;
            DamageRange = damageRange;
            DamageRangeDescription = damageRangeDescription;
            Type = type;
        }
    }
}
