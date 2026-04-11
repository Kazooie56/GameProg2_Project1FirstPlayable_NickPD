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
            AllItems.Add(new Sparkle(13, 5));
            AllItems.Add(new Sparkle(20, 12));
            AllItems.Add(new Sparkle(19, 17));

            // New Sparkles
            AllItems.Add(new Sparkle(11, 1));
            AllItems.Add(new Sparkle(18, 1));
            AllItems.Add(new Sparkle(20, 1));
            AllItems.Add(new Sparkle(9, 2));
            AllItems.Add(new Sparkle(10, 2));
            AllItems.Add(new Sparkle(2, 5));
            AllItems.Add(new Sparkle(1, 6));
            AllItems.Add(new Sparkle(3, 6));
            AllItems.Add(new Sparkle(5, 6));
            AllItems.Add(new Sparkle(7, 6));
            AllItems.Add(new Sparkle(17, 10));
            AllItems.Add(new Sparkle(18, 10));
            AllItems.Add(new Sparkle(19, 10));
            AllItems.Add(new Sparkle(7, 14));
            AllItems.Add(new Sparkle(6, 15));
            AllItems.Add(new Sparkle(8, 15));
            AllItems.Add(new Sparkle(7, 16));
            AllItems.Add(new Sparkle(18, 16));
            AllItems.Add(new Sparkle(8, 17));
            AllItems.Add(new Sparkle(18, 17));
            AllItems.Add(new Sparkle(20, 17));
            AllItems.Add(new Sparkle(19, 18));

            AllItems.Add(new HealthPotion(9, 1));
            AllItems.Add(new HealthPotion(19, 1));
            AllItems.Add(new HealthPotion(2, 6));

            AllItems.Add(new QuestionEvent(8, 4, (1, 7, 3, 7)));
            AllItems.Add(new QuestionEvent(9, 5, (4, 8, 4, 11)));
            AllItems.Add(new QuestionEvent(11, 10, (9, 10, 9, 16)));

        }
    }
}
