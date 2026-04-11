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
        public static int PlayerStartHealth = 80;

        // Enemy stats
        public static int BossBarbarianHealth = 32;
        public static int BarbarianHealth = 16;
        public static int MageHealth = 12;
        public static int TheifHealth = 16;
        public static int MyrmidonHealth = 16;

        // Health potion amount
        public static int HealthPotion = 20;

        

    }
}
