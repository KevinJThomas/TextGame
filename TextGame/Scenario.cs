using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Scenario
    {
        public string Location { get; set; }
        public string Description { get; set; }
        public string[] Targets { get; set; }
        public string Target { get; set; }
        public Item Item { get; set; }

        protected int _state = 1;
        protected Player _player;

        public Scenario(Player player)
        {
            _player = player;
            Target = null;
            Item = null;
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
                Console.WriteLine("I think your bag is empty! " + ex.Message);
            }

        }

        public void Help()
        {
            Console.WriteLine("\nYou are currently in " + Location);
            Console.WriteLine("Other than selecting one of the listed options for your current location, you may also use:");
            Console.WriteLine("bag - lists the items in your bag and allows you to use an item of your choice");
            Console.WriteLine("look - gives you a better idea of the environment that you're in");
            Console.WriteLine("use *item name* - attempts to use whatever item you specify (you will be asked for a target)\n");
        }

        public void Look()
        {
            Services.ScrollText(Description, 1000);
        }

        public void EnterArea(string[] options, string desc, string[] targets)
        {
            Description = desc;
            Targets = targets;

            Console.WriteLine();
            Services.ScrollText("You are in " + Location);
            Services.ScrollText("What would you like to do?\n");

            foreach(string option in options)
            {
                Services.ScrollText((Array.IndexOf(options, option) + 1) + ") " + option);
            }
            Console.WriteLine();
        }

        public void ExamineCommand(string command)
        {
            if (command.ToLower() == "bag")
            {
                InspectBag();
            }
            else if (command.ToLower() == "help")
            {
                Help();
            }
            else if (command.ToLower() == "look")
            {
                Look();
            }
            else if (command.Length >= 5 && command.Substring(0, 3).ToLower() == "use" && Targets.Length > 0)
            {
                foreach (Item item in _player.Bag.GetContents())
                {
                    int index = 0;
                    bool found = false;

                    if (item.Name.ToLower() == command.Substring(4).ToLower())
                    {
                        index = _player.Bag.GetContents().IndexOf(item);
                        found = true;
                    }

                    if (found == true)
                    {
                        Services.ScrollText("Who/what would you like to use " + _player.Bag.GetContents()[index].Name + " on?", 500);
                        foreach (string target in Targets)
                        {
                            Services.ScrollText((Array.IndexOf(Targets, target) + 1) + " - " + target);
                        }

                        int input;
                        string decision = Console.ReadLine();

                        if (Int32.TryParse(decision, out input))
                        {
                            if (Targets.Length <= input)
                            {
                                Item = _player.Bag.GetContents()[index];
                                Target = Targets[input - 1];
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
}
