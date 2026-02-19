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
        public int Y { get; private set; }
        public int X { get; private set; }

        public Health Health { get; private set; }
        private List<Weapon> _weapons = new List<Weapon>();

        public bool IsInFort { get; set; }          // dumb "if inside fort" bool, also I think this means it's false by default.

        public List<Weapon> Weapons
        {
            get { return _weapons; }
        }
        private Random _random = new Random();

        public Player(int startY, int startX)
        {
            Y = startY;
            X = startX;
            Health = new Health(40);

            _weapons.Add(new Weapon("Bronze Sword", new int[] { 5 }, 8));
            _weapons.Add(new Weapon("Iron Sword", new int[] { 7, 9 }, 6));
            _weapons.Add(new Weapon("Steel Sword", new int[] { 11, 13, 15 }, 4));
            _weapons.Add(new Weapon("Silver Sword", new int[] { 15, 17, 19, 23 }, 2));
        }

        public List<Weapon> GetWeapons()
        {
            return _weapons;
        }

        public (int newY, int newX) MovePlayer(ConsoleKey key)
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

        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public void TakeDamage(int amount)
        {
            Health.TakeDamage(amount);
        }

        public bool IsDead()
        {
            return Health.Dead();
        }

        public int GetHealth()
        {
            return Health.Current;
        }

        public int Attack(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= _weapons.Count)
                return 0; // error

            return _weapons[weaponIndex].Attack(_random); // not error
        }
        // INCLUDE ABSTRACT CLASSES TO FINISH THE OTHER ENEMY TYPES IN THE GAME // FIX
    }
}
