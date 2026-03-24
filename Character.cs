using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameProg2_Project1FirstPlayable_NickPD.Enemy;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public abstract class Character : Entity
    {
        public string Type { get; set; } // would be class but classes are a code thing
        public Health Health { get; private set; }
        public Character(int x, int y, char icon, string type, Health health) : base(x, y, icon) // dont need to assign things in constructor that are in base
        {
            Type = type;
            Health = health;
        }
        public bool IsAlive()
        {
            return Health.Current > 0;
        }
    }
}