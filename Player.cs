using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProg2_Project1FirstPlayable_NickPD
{
    //player class handles the player.
    public class Player : Entity
    {
        private List<Weapon> _weapons = new List<Weapon>();
        public List<Weapon> Weapons
        {
            get { return _weapons; }
        }
        private Random _random = new Random();

        public Player(int startY, int startX) : base(startX, startY, 40)
        {
            _weapons.Add(new Weapon("Bronze Sword", new int[] { 5 }, 8));
            _weapons.Add(new Weapon("Iron Sword", new int[] { 7, 9 }, 6));
            _weapons.Add(new Weapon("Steel Sword", new int[] { 11, 13, 15 }, 4));
            _weapons.Add(new Weapon("Silver Sword", new int[] { 15, 17, 19, 23 }, 2));
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

        public void DrawPlayer()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("0");
            Console.ResetColor();
        }
    }
}
