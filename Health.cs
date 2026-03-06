using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Health
    {
        public int Current { get; private set; }
        public int Max { get; private set; }

        public Health(int maxHealth)
        {
            Max = maxHealth;
            Current = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) return;

            Current -= amount;

            if (Current < 0)
                Current = 0;
        }

        public void Heal(int amount)
        {
            Current += amount;

            if (Current > Max)
                Current = Max;
        }
    }
}
