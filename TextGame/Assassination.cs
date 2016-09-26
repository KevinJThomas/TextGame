using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Assassination : Scenario
    {
        int _timer = 50;

        public Assassination(Player player) : base(player)
        {
            _player = player;
        }

        public void Start()
        {
            //Creating new items and adding them into the player's bag
            Item gun = new Item("Gun");
            Item knife = new Item("Knife");
            Item twine = new Item("Twine");
            List<Item> items = new List<Item>() { gun, knife, twine };
            _player.Bag.Add(items);

            string text1 = "You are a Russian KGB operative.";
            string text2 = "There has been a small uprising against the government in Moscow that has a chance of gaining some traction.";
            string text3 = "The uprising is being lead by a very vocal individual: Bodrov Ilyich";
            string text4 = "He must be silenced before too much damage is done. This your mission.";
            string text5 = "He is scheduled to be speaking in the Red Square in 5 minutes.";
            string text6 = "You are arriving to the square now; there isn't much time to eliminate the target.";
            string text7 = "In your bag you are carrying a handgun, knife, and 3 feet of twine.";
            string text8 = "Find and kill Bodrov Ilyich. Your life depends on it.";

            Services.ScrollText(text1, 2000);
            Services.ScrollText(text2, 2000);
            Services.ScrollText(text3, 2000);
            Services.ScrollText(text4, 2000);
            Services.ScrollText(text5, 2000);
            Services.ScrollText(text6, 2000);
            Services.ScrollText(text7, 2000);
            Services.ScrollText(text8, 3500);

            RedSquareEnterance();
        }

        public void RedSquareEnterance()
        {
            Location = "Red Square Enterance";
            Console.WriteLine();
            Console.WriteLine("You are in enterance to the Red Square");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("1 - Scan the crowd");
            Console.WriteLine("2 - Head over to the stage");
            Console.WriteLine("3 - Go into the nearby hotel");

            int decision;
            string redSquareEnterance = Console.ReadLine();

            if (Int32.TryParse(redSquareEnterance, out decision))
            {
                switch (decision)
                {
                    case 1:
                        Services.ScrollText("You scan around you, but you see nothing of interest.", 500);
                        RedSquareEnterance();
                        break;
                    case 2:
                        Services.ScrollText("You walk over to the front of the stage.", 500);
                        Stage();
                        break;
                    case 3:
                        Services.ScrollText("You walk into the hotel bordering the square", 500);
                        Hotel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RedSquareEnterance();
                        break;
                }
            }
            else if (redSquareEnterance.ToLower() == "bag")
            {
                InspectBag();
                RedSquareEnterance();
            }
            else if (redSquareEnterance.ToLower() == "help")
            {
                Help();
                RedSquareEnterance();
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                RedSquareEnterance();
            }
        }

        public void Stage()
        {
            Location = "Stage";
            Console.WriteLine("IM AT THE STAGE");
        }

        public void Hotel()
        {
            Location = "Hotel";
            Console.WriteLine("IM AT THE HOTEL");
        }
    }
}
