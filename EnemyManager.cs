using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    public class EnemyManager
    {
        public Enemy[] enemies = // all our enemies with a new type "name"
        { // btw, first number = furthest left on map, second number means how many blocks down
          new Thief(4, 16, 16, new int[]{2}, "2"), 
          new Barbarian(12, 16, 16, new int[]{2,3,4,5}, "2-5"), // Barbarians are the most volatile
          new Barbarian(12, 15, 16, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(8, 1, 16, new int[]{2,3,4,5}, "2-5"),
          new Myrmidon(17, 14, 16, new int[] { 3, 4 }, "3-4"), // More consistent barbarians
          new BossBarbarian(17, 5, 32, new int[] { 3, 4, 5, 6 }, "3-6"), // Stronger than average Barbarian with 2x health
          new Barbarian(18, 15, 16, new int[] { 2, 3, 4, 5 }, "2-5"),
          new Myrmidon(17, 16, 16, new int[] { 3, 4 }, "3-4"),
          new Mage(17, 15, 12, new int[] { 4 }, "4"), // Higher damage than average but lowest health
          new Myrmidon(14, 5, 16, new int[] { 3, 4 }, "3-4"),
          new Barbarian(14, 6, 16, new int[] { 2, 3, 4, 5 }, "2-5"),
          new Mage(20, 6, 12, new int[] { 4 }, "4")
        };
    }
}
