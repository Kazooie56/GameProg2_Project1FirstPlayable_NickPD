using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{

    //player class handles the player.
    internal class Player
    {
        public int Y; // Should be 15, 15th line down?
        public int X; // Should be 2, 2nd from the left? both of these might not be accurate anymore

        public Health health;
        private List<Weapon> weapons = new List<Weapon>();

        public bool isInFort = false;           // dumb "if inside fort" bool

        public List<Weapon> Weapons => weapons;
        public Random random = new Random();

        public Player(int startY, int startX)
        {
            Y = startY;
            X = startX;
            health = new Health(40);

            weapons.Add(new Weapon("Bronze Sword", new int[] { 5 }, 8));
            weapons.Add(new Weapon("Iron Sword", new int[] { 7, 9 }, 6));
            weapons.Add(new Weapon("Steel Sword", new int[] { 11, 13, 15 }, 4));
            weapons.Add(new Weapon("Silver Sword", new int[] { 15, 17, 19, 23 }, 2));
        }

        public List<Weapon> GetWeapons()
        {
            return weapons;
        }

        public (int newY, int NewX) MovePlayer(ConsoleKey key)
        {
            int newY = Y;
            int newX = X;

            switch (key)
            {
                case ConsoleKey.W: newY--; break;
                case ConsoleKey.S: newY++; break;
                case ConsoleKey.A: newX--; break;
                case ConsoleKey.D: newX++; break;
            }

            return (newY, newX);
        }

        public void SetPosition(int newY, int newX)
        {
            Y = newY;
            X = newX;
        }

        public void TakeDamage(int amount)
        {
            health.TakeDamage(amount);
        }

        public bool IsDead()
        {
            return health.Dead();
        }

        public int GetHealth()
        {
            return health.Current;
        }

        public int Attack(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= weapons.Count)
                return 0; // error

            return weapons[weaponIndex].Attack(random); // not error
        }
        // INCLUDE ABSTRACT CLASSES TO FINISH THE OTHER ENEMY TYPES IN THE GAME
    }
}
