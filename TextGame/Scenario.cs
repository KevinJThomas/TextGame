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

        public void InspectBag()
        {
            try
            {
                _player.Bag.PrintBag();

                int input;
                string bagInput = Console.ReadLine();

                if (Int32.TryParse(bagInput, out input))
                {
                    _player.Bag.GetContents()[input - 1].UseItem();
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("I think you bag is empty! " + ex.Message);
            }

        }

        public void Help()
        {
            Console.WriteLine("Other than selecting one of the listed options for your current location, you may also use:");
            Console.WriteLine("bag - lists the items in your bag and allows you to use an item of your choice");
        }
    }
}
