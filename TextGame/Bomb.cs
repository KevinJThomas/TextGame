using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Bomb : Scenario
    {
        string[] rules = new string[] { "You must cut the wires in the correct order", "There are two ways to identify a wire: color and length",
            "You know there are 3 wires you have to cut", "There are one of each color and length (red, blue, green) (short, medium, long)",
            "The long wire is cut after the short wire", "The green wire is cut before the red wire", "The medium wire is cut before the blue wire",
            "The blue wire is cut before the red wire" };

        public Bomb(Player player) : base(player)
        {
            _player = player;
        }

        public void Start()
        {
            Services.ScrollText("There is a bomb about to off in the middle of Manhattan", 500);
            Services.ScrollText("You have beeing training in the bomb squad but have never dealt with a real threat on your own.", 500);
            Services.ScrollText("Your mentor is out of town; this is your chance to shine.", 500);
            Services.ScrollText("Now as long as you remember all your training, nothing will go wrong..", 1500);
            Services.ScrollText("You enter the bank where the bomb is located and are escorted over to it.", 500);
            Services.ScrollText("Everything you've learned in training is spiraling around your brain..");
            Services.ScrollText("All you can remember is: ");
            PrintRules();
            DisableBomb();
        }

        public void DisableBomb()
        {
            if(WireOne())
            {
                if(WireTwo())
                {
                    if(WireThree())
                    {
                        _player.LevelCompleted = true;
                    }
                }
            }
        }

        public bool WireOne()
        {
            Services.ScrollText("Enter the wire you would like to cut, starting with the length (ex: 'tiny purple')");
            string answer = Console.ReadLine();
            if(CheckAnswer(answer, "medium green"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WireTwo()
        {
            Services.ScrollText("Enter the wire you would like to cut, starting with the length (ex: 'tiny purple')");
            string answer = Console.ReadLine();
            if (CheckAnswer(answer, "short blue"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WireThree()
        {
            Services.ScrollText("Enter the wire you would like to cut, starting with the length (ex: 'tiny purple')");
            string answer = Console.ReadLine();
            if (CheckAnswer(answer, "long red"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckAnswer(string answer, string wire)
        {
            if (answer == wire)
            {
                Services.ScrollText("Nothing blew up! That must've been right!");
                return true;
            }
            else
            {
                Services.ScrollText("BOOOOOOM");
                Services.ScrollText("That wasn't the right wire.. you blew up the town!");
                return false;
            }
        }

        public void PrintRules()
        {
            Console.WriteLine();
            foreach (string option in rules)
            {
                Services.ScrollText((Array.IndexOf(rules, option) + 1) + " - " + option);
            }
            Console.WriteLine();
        }
    }
}
