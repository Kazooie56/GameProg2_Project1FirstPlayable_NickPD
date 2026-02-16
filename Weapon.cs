using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    internal class Weapon
    {
        //static int bronzeSwordDamageRange = 5;
        //static int[] ironSwordDamageRange = { 7, 9 };
        //static int[] steelSwordDamageRange = { 11, 13, 15 };
        //static int[] silverSwordDamageRange = { 15, 17, 19, 23 };

        //static int bronzeSwordDurability = 8;
        //static int ironSwordDurability = 6;
        //static int steelSwordDurability = 4;
        //static int silverSwordDurability = 2;

        public string Name;
        private int[] damageRange;
        public int Durability;

        public int[] DamageRange => damageRange;

        public Weapon(string name, int[] damageRange, int durability)
        {
            Name = name;
            this.damageRange = damageRange;
            Durability = durability;
        }

        public int Attack(Random random) // deal damage based on the weapon random damage values
        {
            Durability--;
            return damageRange[random.Next(damageRange.Length)];
        }

        public void Repair(int amount)
        {
            Durability += amount;
        }

        public bool IsBroken()
        {
            return Durability == 0;
        }
    }
}
