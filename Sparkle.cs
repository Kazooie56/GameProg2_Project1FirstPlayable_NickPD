using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class Sparkle : Item
    {
        public Sparkle(int x, int y) : base(x, y, '*')
        {
            CollectMessage = "Your Silver Sword has gained a little more endurance!";
            RemoveOnCollect = true;
        }

        public override void OnCollect(Player player, GameManager gameManager)
        {
            player.Weapons[3].Repair(1);
            gameManager.LastMessage = CollectMessage;
        }
    }
}
