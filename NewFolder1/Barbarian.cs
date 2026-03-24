using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Barbarian : Enemy
    {
        public Barbarian(
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
            "Barbarian",
            'p'
        )
        {

        }
    }
}
