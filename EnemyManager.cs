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
          new Thief(4, 16, Settings.TheifHealth, new int[]{2}, "2"),  // For this first guy, 4, 16 means he's 
          new Barbarian(12, 16, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"), // Barbarians are the most volatile
          new Barbarian(12, 15, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(8, 1, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(17, 13, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Myrmidon(17, 14, Settings.MyrmidonHealth, new int[] { 3, 4 }, "3-4"), // More consistent barbarians
          new BossBarbarian(17, 5, Settings.BossBarbarianHealth, new int[] { 3, 4, 5, 6 }, "3-6"), // Stronger than average Barbarian with 2x health
          new Barbarian(18, 15, Settings.BarbarianHealth, new int[] { 2, 3, 4, 5 }, "2-5"),
          new Myrmidon(17, 16, Settings.MyrmidonHealth, new int[] { 3, 4 }, "3-4"),
          new Mage(17, 15, Settings.MageHealth, new int[] { 4 }, "4"), // Higher damage than average but lowest health
          new Myrmidon(14, 5, Settings.MyrmidonHealth, new int[] { 3, 4 }, "3-4"),
          new Barbarian(14, 6, Settings.BarbarianHealth, new int[] { 2, 3, 4, 5 }, "2-5"),
          new Mage(20, 6, Settings.MageHealth, new int[] { 4 }, "4"),
                // extra barbarians
          new Barbarian(7, 1, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(7, 4, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(17, 4, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(3, 5, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(6, 5, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(8, 5, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(16, 5, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(18, 5, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(9, 6, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(6, 7, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(7, 9, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(8, 9, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(9, 9, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(8, 10, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(18, 14, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(19, 15, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(18, 16, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),
          new Barbarian(17, 17, Settings.BarbarianHealth, new int[]{2,3,4,5}, "2-5"),

        };
    }
}
