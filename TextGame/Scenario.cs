using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //Haven't put much thought into this class.. But I think this is where most of the code should run
    class Scenario
    {
        public string Location { get; set; } //Used to track the player's location within a specific scenario - ie. Room 1/Room 2/Basement/Roof/etc.
        protected Player _player;

        public Scenario(Player player)
        {
            _player = player;
        }

        public void ScrollText(string text, int delay = 0)
        {
            Random rand = new Random();
            foreach (char character in text)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(rand.Next(20, 75));
            }
            System.Threading.Thread.Sleep(delay);
            Console.WriteLine("");
        }
    }
}
