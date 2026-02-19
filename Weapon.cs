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

        public string Name { get; private set; }
        private int[] _damageRange;
        public int Durability { get; private set; }

        public int[] DamageRange
        {
            get { return _damageRange; }
        }

        public Weapon(string name, int[] damageRange, int durability)
        {
            Name = name;
            _damageRange = damageRange;
            Durability = durability;
        }

        public int Attack(Random random) // deal damage based on the weapon random damage values
        {
            Durability--;
            int damageIndex = random.Next(_damageRange.Length);
            return _damageRange[damageIndex];
        }

        public void Repair(int amount) //With this new method I can now increase a weapon's durability by anything.
        {
            Durability += amount;
        }

        public bool IsBroken()
        {
            return Durability == 0;
        }
    }
}
