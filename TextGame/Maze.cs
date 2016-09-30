using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Maze : Scenario
    {
        //Arrays of available commands for each area/person
        string[] roomOne = new string[] { "Examine Desk", "Examine Table", "Use West Door", "Use East Door" };
        string[] deskOne = new string[] { "Examine Notebook", "Examine Paper", "Leave the Desk" };
        string[] tableOne = new string[] { "Pick up Red Key and Sledge Hammer", "Leave the Table" };

        string[] roomTwo = new string[] { "Use North Door", "Use East Door", "Use South Door" };

        string[] roomThree = new string[] { "Examine Light Switch", "Examine Poster", "Use North Door", "Use East Door", "Use South Door" };

        string[] roomFour = new string[] { "Pull Lever", "Use North Door", "Use East Door" };

        string[] roomFive = new string[] { "Examine Bed", "Examine Chair", "Examine Vent", "Use East Door", "Use South Door", "Use West Door" };
        string[] bedFive = new string[] { "Search Pillowcase", "Search Sheets", "Search Underneath Bed", "Leave the Bed" };

        string[] roomSix = new string[] { "Examine Glass Box", "Use North Door", "Use South Door", "Use West Door" };

        string[] roomSeven = new string[] { "Examine Microwave", "Use North Door", "Use East Door", "Use South Door" };
        string[] microwaveSeven = new string[] { "Open Microwave", "Use Microwave", "Leave the Microwave" };

        string[] roomEight = new string[] { "Use North Door", "Use East Door", "Use West Door" };

        string[] roomNine = new string[] { "Examine Table", "Use North Door", "Use South Door", "Use West Door" };
        string[] tableNine = new string[] { "Pick up Blue Key", "Leave the Table" };

        string[] roomTen = new string[] { "Examine Oven", "Use South Door", "Use West Door" };
        string[] ovenTen = new string[] { "Open Oven", "Preheat Oven", "Leave the Oven" };

        string[] roomEleven = new string[] { "Use East Door", "Use South Door", "Use West Door" };

        //Descriptions for each area
        string roomDesc = "You're in a perfectly square room with white floors, ceilings, and walls.";

        //Targets for items for each area
        string[] roomOneTarget = new string[] { "West Door", "East Door", "Table", "Desk" };
        string[] roomTwoTarget = new string[] { "North Door", "East Door", "South Door" };
        string[] roomThreeTarget = new string[] { "North Door", "East Door", "South Door", "Light Switch", "Poster" };
        string[] roomFourTarget = new string[] { "North Door", "East Door", "Lever" };
        string[] roomFiveTarget = new string[] { "East Door", "South Door", "West Door", "Bed", "Chair", "Vent" };
        string[] roomSixTarget = new string[] { "North Door", "South Door", "West Door", "Glass Box" };
        string[] roomSevenTarget = new string[] { "North Door", "East Door", "Microwave" };
        string[] roomEightTarget = new string[] { "North Door", "East Door", "West Door" };
        string[] roomNineTarget = new string[] { "North Door", "South Door", "West Door", "Table" };
        string[] roomTenTarget = new string[] { "South Door", "West Door", "Oven" };
        string[] roomElevenTarget = new string[] { "East Door", "South Door", "West Door" };

        string[] noTargets = new string[0];

        //Items to be collected throughout the level
        Item sledgeHammer = new Item("Sledge Hammer");
        Item key1 = new Item("Red Key");
        Item key2 = new Item("Blue Key");
        Item key3 = new Item("Green Key");

        //Starting Map
        Item map = new Item("Map");

        bool _leverPulled = false;
        bool _doorBroken = false;
        bool _k1HammerPickedUp = false;
        bool _k2PickedUp = false;
        bool _k3PickedUp = false;
        bool _ovenOn = false;

        public Maze(Player player) : base(player)
        {
            _player = player;
            _player.Bag.Add(map);
        }

        public void Start()
        {
            Services.ScrollText("You are inside of a cubic white room.", 750);
            Services.ScrollText("There is a table in one corner and a desk in another.", 750);
            Services.ScrollText("You have no idea where you are or how you got there, but right now getting out is the most important priority.", 750);
            Services.ScrollText("You look in your bag and find an electronic map, at least you have that going for you!", 750);
            Services.ScrollText("From the looks of it, this is a pretty big building; escaping may prove to be a challenge..", 750);

            RoomOne();
        }

        public void RoomOne()
        {
            _state = 1;
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
                    else if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
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
            _state = 2;
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
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
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
            _state = 3;
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
                        Services.ScrollText("It's a Metallica poster, but the 'c' is whited out..", 500);
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
                        RoomThree();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomThree();
                }
                else
                {
                    RoomThree();
                }
            }
        }

        public void RoomFour()
        {
            _state = 4;
            Location = "a square room";
            EnterArea(roomFour, roomDesc, roomFourTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You pull the lever on the wall.. there is a quiet grinding sound in the distance.", 500);
                        _leverPulled = !_leverPulled;
                        RoomFour();
                        break;
                    case 2:
                        Services.ScrollText("You walk through the north door.", 500);
                        RoomThree();
                        break;
                    case 3:
                        Services.ScrollText("The door is locked!", 500);
                        RoomFour();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomFour();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomFour();
                }
                else
                {
                    RoomFour();
                }
            }
        }

        public void RoomFive()
        {
            _state = 5;
            Location = "a square room";
            EnterArea(roomFive, roomDesc, roomFiveTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the bed in the corner of the room.", 500);
                        BedFive();
                        break;
                    case 2:
                        Services.ScrollText("It looks like it's just a normal wooden chair.", 500);
                        RoomFive();
                        break;
                    case 3:
                        Services.ScrollText("That vent is really dusty. It looks like it hasn't been used in months, gross!", 500);
                        RoomFive();
                        break;
                    case 4:
                        if (_k2PickedUp == true)
                        {
                            Services.ScrollText("You use your blue key to open the east door and walk through it.");
                            RoomSix();
                        }
                        else
                        {
                            Services.ScrollText("The door is locked!", 500);
                            RoomFive();
                        }
                        break;
                    case 5:
                        Services.ScrollText("The door is locked!", 500);
                        RoomFive();
                        break;
                    case 6:
                        Services.ScrollText("You walk through the west door.", 500);
                        RoomThree();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomFive();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomFive();
                }
                else
                {
                    RoomFive();
                }
            }
        }

        public void RoomSix()
        {
            _state = 6;
            Location = "a square room";
            EnterArea(roomSix, roomDesc, roomSixTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the glass box.. It has a keyboard and screen next to it. The screen says:\n", 500);
                        Services.ScrollText("5 8 13 __ 34\n");
                        boxSix();
                        break;
                    case 2:
                        Services.ScrollText("The door is locked!", 500);
                        RoomSix();
                        break;
                    case 3:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomFive();
                        break;
                    case 4:
                        Services.ScrollText("The door is locked!", 500);
                        RoomSix();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomEleven();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomEleven();
                }
                else
                {
                    RoomEleven();
                }
            }
        }

        public void RoomSeven()
        {
            _state = 7;
            Location = "a square room";
            EnterArea(roomSeven, roomDesc, roomSevenTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the bed in the corner of the room.", 500);
                        MicrowaveSeven();
                        break;
                    case 2:
                        Services.ScrollText("The door is locked!", 500);
                        RoomSeven();
                        break;
                    case 3:
                        if (_leverPulled == true)
                        {
                            Services.ScrollText("You walk through the east door.", 500);
                            RoomEight();
                        }
                        else
                        {
                            Services.ScrollText("The door is locked!", 500);
                            RoomSeven();
                        }
                        break;
                    case 4:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomTwo();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomSeven();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomSeven();
                }
                else
                {
                    RoomSeven();
                }
            }
        }

        public void RoomEight()
        {
            _state = 8;
            Location = "a square room";
            EnterArea(roomEight, roomDesc, roomEightTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        if (_k3PickedUp == true)
                        {
                            Services.ScrollText("You use your blue key to open the east door and walk through it.");
                            RoomEleven();
                        }
                        else
                        {
                            Services.ScrollText("The door is locked!", 500);
                            RoomEight();
                        }
                        break;
                    case 2:
                        Services.ScrollText("The door is locked, but there is a keyboard and screen in the wall next to it..", 500);
                        EnterPassword();
                        break;
                    case 3:
                        Services.ScrollText("You walk through the west door.", 500);
                        RoomSeven();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomEight();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomEight();
                }
                else
                {
                    RoomEight();
                }
            }
        }

        public void RoomNine()
        {
            _state = 9;
            Location = "a square room";
            EnterArea(roomNine, roomDesc, roomNineTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the table.", 500);
                        TableNine();
                        break;
                    case 2:
                        Services.ScrollText("You walk through the north door.", 500);
                        RoomTen();
                        break;
                    case 3:
                        Services.ScrollText("The door is locked!", 500);
                        RoomNine();
                        break;
                    case 4:
                        Services.ScrollText("You walk through the west door.", 500);
                        RoomEight();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomNine();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomNine();
                }
                else
                {
                    RoomNine();
                }
            }
        }

        public void RoomTen()
        {
            _state = 10;
            Location = "a square room";
            EnterArea(roomTen, roomDesc, roomTenTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the oven.", 500);
                        OvenTen();
                        break;
                    case 2:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomNine();
                        break;
                    case 3:
                        if (_ovenOn)
                        {
                            Services.ScrollText("You walk through the west door.", 500);
                            RoomEleven();
                        }
                        else
                        {
                            Services.ScrollText("The door is locked!", 500);
                            RoomTen();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomTen();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomTen();
                }
                else
                {
                    RoomTen();
                }
            }
        }

        public void RoomEleven()
        {
            _state = 11;
            Location = "a square room";
            EnterArea(roomEleven, roomDesc, roomElevenTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk through the east door.", 500);
                        RoomTen();
                        break;
                    case 2:
                        Services.ScrollText("You walk through the south door.", 500);
                        RoomEight();
                        break;
                    case 3:
                        Services.ScrollText("You walk through the west door.", 500);
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomEleven();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "Map" && Target == "Map")
                    {
                        PrintMap(_state);
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                    }
                    Item = null;
                    Target = null;
                    RoomEleven();
                }
                else
                {
                    RoomEleven();
                }
            }
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
                        Services.ScrollText("There is a small scribble at the bottom of the paper: '535'", 500);
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
                            Services.ScrollText("You pick up the red key and sledge hammer off of the table and put them in your bag.", 500);
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

        public void BedFive()
        {
            Location = "the corner near the bed";
            EnterArea(bedFive, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You take the pillowcase off of the pillow and turn it inside out. Nothing.", 500);
                        BedFive();
                        break;
                    case 2:
                        Services.ScrollText("You carefully search each layer of sheets, but there is nothing else there.", 500);
                        BedFive();
                        break;
                    case 3:
                        Services.ScrollText("You kneel down and look underneath the bed.. There's nothing to be seen but dust.", 500);
                        BedFive();
                        break;
                    case 4:
                        Services.ScrollText("You walk back to the middle of the room.", 500);
                        RoomFive();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        BedFive();
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
                    BedFive();
                }
                else
                {
                    BedFive();
                }
            }
        }

        public void boxSix()
        {
            Services.ScrollText("Enter Password:\n", 500);
            
            string pass = Console.ReadLine();
            Console.WriteLine();

            if (pass == "21")
            {
                Services.ScrollText("CORRECT");
                Services.ScrollText("The glass retracts into the ground.", 500);
                if (_k3PickedUp == false)
                {
                    Services.ScrollText("You grab the green key that was inside the glass and put it in your bag.");
                    _k3PickedUp = true;
                }
                RoomSix();
            }
            else if (pass.ToLower() == "exit")
            {
                RoomSix();
            }
            else
            {
                Services.ScrollText("INCORRECT");
                Services.ScrollText("(You may either keep guessing or type 'exit' to go back to the middle of the room)");
                boxSix();
            }
        }

        public void MicrowaveSeven()
        {
            Location = "the corner near the microwave";
            EnterArea(microwaveSeven, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You open the microwave door.. wow! It must be brand new. It looks like it's never been used.", 500);
                        MicrowaveSeven();
                        break;
                    case 2:
                        Services.ScrollText("Enter how long to turn it on:", 500);
                        Console.ReadLine();
                        Services.ScrollText("After hitting enter it doesn't do anything. Weird.");
                        MicrowaveSeven();
                        break;
                    case 3:
                        Services.ScrollText("You walk back to the middle of the room.", 500);
                        RoomSeven();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        MicrowaveSeven();
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
                    MicrowaveSeven();
                }
                else
                {
                    MicrowaveSeven();
                }
            }
        }

        public void TableNine()
        {
            Location = "the corner near the table";
            EnterArea(tableNine, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        if (_k2PickedUp == false)
                        {
                            Services.ScrollText("You pick up the blue key off of the table and put it in your bag.", 500);
                            _k2PickedUp = true;
                            _player.Bag.Add(key2);
                            tableNine = new string[] { "Leave the Table" };
                            TableNine();
                        }
                        else
                        {
                            Services.ScrollText("You walk back to the middle of the room.", 500);
                            RoomNine();
                        }
                        break;
                    case 2:
                        if (_k2PickedUp == false)
                        {
                            Services.ScrollText("You walk back to the middle of the room.", 500);
                            RoomNine();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                            TableNine();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        TableNine();
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
                    TableNine();
                }
                else
                {
                    TableNine();
                }
            }
        }

        public void OvenTen()
        {
            Location = "the corner near the oven";
            EnterArea(ovenTen, roomDesc, noTargets);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You open the oven door.. It's still warm. Someone must've used this recently.", 500);
                        OvenTen();
                        break;
                    case 2:
                        Services.ScrollText("Enter preheat temp:", 500);
                        string preheat = Console.ReadLine();
                        if (preheat.Contains("535"))
                        {
                            Services.ScrollText("You hear the west door click.. did the oven just unlock it?!");
                            _ovenOn = true;
                            RoomTen();
                        }
                        else
                        {
                            Services.ScrollText("Nothing happens.");
                            OvenTen();
                        }
                        break;
                    case 3:
                        Services.ScrollText("You walk back to the middle of the room.", 500);
                        RoomTen();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        OvenTen();
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
                    OvenTen();
                }
                else
                {
                    OvenTen();
                }
            }
        }

        public void EnterPassword()
        {
            Console.WriteLine();
            Services.ScrollText("Enter Password:");
            string pass = Console.ReadLine();
            if (pass.ToLower() == "nearby")
            {
                Services.ScrollText("The door begins to open and you walk through.");
                RoomNine();
            }
            else if (pass.ToLower() == "exit")
            {
                RoomEight();
            }
            else
            {
                Services.ScrollText("Incorrect Password, try again or type 'exit' to return to the middle of the room.");
                EnterPassword();
            }
        }

        public void Exit()
        {
            Services.ScrollText("You come to one large and final door to the outside world. Your red key fits perfectly!", 800);
            Services.ScrollText("You step outside and are reminded of what the feeling of freedom is like; you escaped!", 2000);
            _player.LevelCompleted = true;
        }

        public void PrintMap(int room)
        {
            Console.WriteLine("\nRooms = [ ]\nYour current location = *\n");
            switch (room)
            {
                case 1:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[*]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 2:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[*]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 3:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[*]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 4:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[*]______________[ ]");
                    break;
                case 5:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[*]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 6:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[*]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 7:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[*]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 8:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[*]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 9:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[*]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 10:
                    Console.WriteLine("EXIT_____________[ ]_____________[*]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                case 11:
                    Console.WriteLine("EXIT_____________[*]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
                default:
                    Console.WriteLine("EXIT_____________[ ]_____________[ ]");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine(" |                |               |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]_____________[ ]");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine(" |                                |");
                    Console.WriteLine("[ ]______________[ ]______________|");
                    Console.WriteLine(" |                |");
                    Console.WriteLine(" |                |");
                    Console.WriteLine("[ ]______________[ ]");
                    break;
            }
        }
    }
}
