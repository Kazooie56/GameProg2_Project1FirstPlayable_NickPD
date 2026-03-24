using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public abstract class Item : Entity
    {
        public Item(int x, int y, char icon) : base(x, y, icon)
        {
        }
        public abstract void OnCollect(Player player, GameManager gameManager);
        public virtual string CollectMessage { get; protected set; }
        public bool RemoveOnCollect { get; protected set; } = true;
        public virtual char ReplacementTile
        {
            get { return '▓'; }
        }
    }
}
