using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //TODO: Just finished room 3, work on 4/5/7 next
    class Maze: Scenario
    {
        //Arrays of available commands for each area/person
        string[] roomOne = new string[] { "Examine Desk", "Examine Table", "Use West Door", "Use East Door" };
        string[] deskOne = new string[] { "Examine Notebook", "Examine Paper", "Leave the Desk" };
        string[] tableOne = new string[] { "Pick up Key and Sledge Hammer", "Leave the Table" };

        string[] roomTwo = new string[] { "Use North Door", "Use East Door", "Use South Door" };

        string[] roomThree = new string[] { "Examine Light Switch", "Examine Poster", "Use North Door", "Use East Door", "Use South Door" };

        //Descriptions for each area
        string roomDesc = "A perfectly square room with white floors, ceilings, and walls.";

        //Targets for items for each area
        string[] roomOneTarget = new string[] { "West Door", "East Door", "Table", "Desk" };
        string[] roomTwoTarget = new string[] { "North Door", "East Door", "South Door" };
        string[] roomThreeTarget = new string[] { "North Door", "East Door", "South Door", "Light Switch", "Poster" };

        string[] noTargets = new string[0];

        //Items to be collected throughout the level
        Item sledgeHammer = new Item("Sledge Hammer");
        Item key1 = new Item("Key");
        Item key2 = new Item("Key");
        Item key3 = new Item("Key");

        //Starting Map
        Item map = new Item("Map");

        bool _doorBroken = false;
        bool _k1HammerPickedUp = false;

        public Maze(Player player) : base(player)
        {
            _player = player;
            _player.Bag.Add(map);
        }

        public void Start()
        {
            RoomOne();
        }

        public void RoomOne()
        {
            Location = "a square room";
            EnterArea(roomOne, roomDesc, roomOneTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the desk.", 500);
                        DeskOne();
                        break;
                    case 2:
                        Services.ScrollText("You walk over to the table.", 500);
                        TableOne();
                        break;
                    case 3:
                        if (_doorBroken == true)
                        {
                            Services.ScrollText("You walk through the west door.", 500);
                            RoomTwo();
                        }
                        else
                        {
                            Services.ScrollText("The door is locked.. it looks like it's made of pretty flimsy wood!", 500);
                            RoomOne();
                        }
                        break;
                    case 4:
                        Services.ScrollText("The door is locked!");
                        RoomOne();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomOne();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Sledge Hammer")
                    {
                        if (Target == "West Door")
                        {
                            Services.ScrollText("You break down the door with one swing. That was easy!");
                            _doorBroken = true;
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                        }
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomOne();
                }
                else
                {
                    RoomOne();
                }

            }
        }

        public void RoomTwo()
        {
            Location = "a square room";
            EnterArea(roomTwo, roomDesc, roomTwoTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk through the north door.", 500);
                        RoomSeven();
                        break;
                    case 2:
                        Services.ScrollText("You walk through the east door.", 500);
                        RoomOne();
                        break;
                    case 3:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomThree();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomTwo();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    Services.ScrollText("It's not very effective.", 500);
                    Item = null;
                    Target = null;
                    RoomTwo();
                }
                else
                {
                    RoomTwo();
                }
            }
        }

        public void RoomThree()
        {
            Location = "a square room";
            EnterArea(roomThree, roomDesc, roomThreeTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("Hmm, switching it on and off doesn't seem to do anything", 500);
                        RoomThree();
                        break;
                    case 2:
                        Services.ScrollText("It's a Metallica, but the 'c' is whited out..", 500);
                        RoomThree();
                        break;
                    case 3:
                        Services.ScrollText("You walk through the north door.", 500);
                        RoomTwo();
                        break;
                    case 4:
                        Services.ScrollText("You walk through the east door.", 500);
                        RoomFive();
                        break;
                    case 5:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomFour();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomTwo();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    Services.ScrollText("It's not very effective.", 500);
                    Item = null;
                    Target = null;
                    RoomTwo();
                }
                else
                {
                    RoomTwo();
                }
            }
        }

        public void RoomFour()
        {

        }

        public void RoomFive()
        {

        }

        public void RoomSeven()
        {

        }

        public void DeskOne()
        {
            Location = "the corner near the desk";
            EnterArea(deskOne, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("The notebook is missing all of it's pages, but on the inside of the cover it says:\n" + 
                            "the password is nearby", 500);
                        DeskOne();
                        break;
                    case 2:
                        Services.ScrollText("There is a small string of characters scribbled at the bottom of the paper: 'A7T32'", 500);
                        DeskOne();
                        break;
                    case 3:
                        Services.ScrollText("You walk back to the middle of the room.", 500);
                        RoomOne();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        DeskOne();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    Services.ScrollText("It's not very effective.", 500);
                    Item = null;
                    Target = null;
                    DeskOne();
                }
                else
                {
                    DeskOne();
                }
            }
        }

        public void TableOne()
        {
            Location = "the corner near the table";
            EnterArea(tableOne, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        if (_k1HammerPickedUp == false)
                        {
                            Services.ScrollText("You pick up the key and sledge hammer off of the table and put them in your bag.", 500);
                            _k1HammerPickedUp = true;
                            _player.Bag.Add(key1);
                            _player.Bag.Add(sledgeHammer);
                            tableOne = new string[] { "Leave the Table" };
                            TableOne();
                        }
                        else
                        {
                            Services.ScrollText("You walk back to the middle of the room.", 500);
                            RoomOne();
                        }
                        break;
                    case 2:
                        if (_k1HammerPickedUp == false)
                        {
                            Services.ScrollText("You walk back to the middle of the room.", 500);
                            RoomOne();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                            TableOne();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        TableOne();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    Services.ScrollText("It's not very effective.", 500);
                    Item = null;
                    Target = null;
                    TableOne();
                }
                else
                {
                    TableOne();
                }
            }
        }

        public void PrintMap()
        {
            //THIS WILL BE ALOT OF FUN TO WRITE
        }
    }
}
