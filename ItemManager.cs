using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class ItemManager
    {
        public List<Item> AllItems { get; private set; }
        public ItemManager()
        {
            AllItems = new List<Item>();

            AllItems.Add(new Sparkle(10, 1));
            AllItems.Add(new Sparkle(19, 1));
            AllItems.Add(new Sparkle(13, 5));
            AllItems.Add(new Sparkle(20, 12));
            AllItems.Add(new Sparkle(19, 17));

            AllItems.Add(new HealthPotion(9, 1));
            AllItems.Add(new HealthPotion(2, 6));

            AllItems.Add(new QuestionEvent(8, 4, (1, 7, 3, 7)));
            AllItems.Add(new QuestionEvent(9, 5, (4, 8, 4, 11)));
            AllItems.Add(new QuestionEvent(11, 10, (9, 10, 9, 16)));

        }
    }
}
