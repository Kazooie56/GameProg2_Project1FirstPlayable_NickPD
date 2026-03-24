using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class HealthPotion : Item
    {
        public HealthPotion(int x, int y) : base(x, y, '+')
        {
            CollectMessage = "Your character has recovered some HP!";
            RemoveOnCollect = true;
        }

        public override void OnCollect(Player player, GameManager gameManager)
        {
            player.Health.Heal(5);
            gameManager.LastMessage = CollectMessage;
        }
    }
}
