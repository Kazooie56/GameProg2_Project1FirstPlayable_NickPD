using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    // global or settings class that stores game data (alleviates hardcoding)
    //  variables could be set as constants so that they cannot be changed
    //  although globally accessible
    public class Settings
    {
        // Player
        public const int PlayerStartHealth = 40;

        // Enemy defaults
        public const int BarbarianHealth = 16;
        public const int MageHealth = 12;

    }
}
