using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Assassination : Scenario
    {
        bool _talkToGuard = false;
        int _timer = 50;

        //Arrays of available commands for each area/person
        string[] redSquareEnterance = new string[] { "Scan the crowd", "Head over to the stage", "Go into the nearby hotel" };
        string[] stageFront = new string[] { "Scan the crowd", "Head back to the Red Square enterance", "Go behind the stage", "Go into the nearby hotel" };
        string[] hotel = new string[] { "Talk to the receptionist", "Head back outside towards the stage", "Head back outside towards the enterance",
            "Take the stairs down to the basement" };
        string[] stageBack = new string[] { "Examine Tarps", "Examine Crates", "Head back to the front of the stage" };
        string[] basement = new string[] { "Head back upstairs" };

        string[] receptionist = new string[] { "Do you know when the speech will be starting?", "Is it true? Is Mr. Ilyich really staying in this hotel?",
            "Can I reserve a room for tonight?", "Have a good day!" };
        string[] guard = new string[] { "*Flash gun*. . .I suggest you make a special exception.. Bub.",
            "I'm with the maintenance team. Apparently there's a weak pillar underneath the stage I need to reinforce.", "Sorry for bothering you." };

        //Descriptions for each area
        string redSquareEnteranceDesc = "A giant square packed with people. It's warm out and the weather is sunny; what a great day to be outside!";
        string stageFrontDesc = "In front of you is a large stage with a single podium in the center. There are a few sound guys preparing the microphone.";
        string hotelDesc = "Wow! This is a nice hotel! The lobby a high ceiling and is very open with a few silk couches sitting in the middle around a coffee table.";
        string stageBackDesc = "It's pretty grimy back here. This must just be used as a storage space.";
        string basementDesc = "An emptry room except for a trashy couch and a tv. The lighting is very poor.";

        //Targets for items for each area
        string[] redSquareEnteranceTarget = new string[] { };
        string[] stageFrontTarget = new string[] { };
        string[] hotelTarget = new string[] { "Receptionist" };
        string[] stageBackTarget = new string[] { "Guard", "Tarps", "Crates" };
        string[] basementTarget = new string[] { "Bodrov Ilyich" };

        //Items the player will start with
        Item gun = new Item("Gun");
        Item knife = new Item("Knife");
        Item twine = new Item("Twine");

        public Assassination(Player player) : base(player)
        {
            _player = player;
            List<Item> items = new List<Item>() { gun, knife, twine };
            _player.Bag.Add(items);
        }

        public void Start()
        {          
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
            Location = "the Red Square's enterance";
            EnterArea(redSquareEnterance, redSquareEnteranceDesc, redSquareEnteranceTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        ScanCrowd();
                        Timer();
                        RedSquareEnterance();
                        break;
                    case 2:
                        Services.ScrollText("You walk over to the front of the stage.", 500);
                        Timer();
                        Stage();
                        break;
                    case 3:
                        Services.ScrollText("You walk into the hotel bordering the square", 500);
                        Services.ScrollText("There is security with a metal detector at the door, you are forced to leave your gun and knife with them.", 1000);
                        _player.Bag.Remove(gun);
                        _player.Bag.Remove(knife);
                        Timer();
                        Hotel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RedSquareEnterance();
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
                    Timer();
                    RedSquareEnterance();
                }
                else
                {
                    RedSquareEnterance();
                }
                
            }
        }

        public void Stage()
        {
            Location = "front of the stage";
            EnterArea(stageFront, stageFrontDesc, stageFrontTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        ScanCrowd();
                        Timer();
                        Stage();
                        break;
                    case 2:
                        Services.ScrollText("You walk over to the Red Square's enterance.", 500);
                        Timer();
                        RedSquareEnterance();
                        break;
                    case 3:
                        Services.ScrollText("You walk behind the stage", 500);
                        if (_talkToGuard == false)
                        {
                            Services.ScrollText("You're stopped by a guard", 1000);
                            Services.ScrollText("..Stop right there bub, no one is allowed back here.");
                            Timer();
                            Guard();
                        }
                        else
                        {
                            Timer();
                            StageBack();
                        }
                        break;
                    case 4:
                        Services.ScrollText("You walk into the hotel bordering the square", 500);
                        Services.ScrollText("There is security with a metal detector at the door, you are forced to leave your gun and knife with them.", 1000);
                        _player.Bag.Remove(gun);
                        _player.Bag.Remove(knife);
                        Timer();
                        Hotel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        Stage();
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
                    Timer();
                    Stage();
                }
                Stage();
            }
        }

        public void Hotel()
        {
            Location = "the hotel";
            EnterArea(hotel, hotelDesc, hotelTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the front desk in the lobby and strike up a conversation with the receptionist\n", 1500);
                        Services.ScrollText("..Hello! Welcome to the Metropol Hotel. What can I do for you?", 1500);
                        Timer();
                        Receptionist();
                        break;
                    case 2:
                        Services.ScrollText("You walk out the north door.", 500);
                        Services.ScrollText("You get your gun and knife back on the way out.", 1000);
                        _player.Bag.Add(gun);
                        _player.Bag.Add(knife);
                        Timer();
                        Stage();
                        break;
                    case 3:
                        Services.ScrollText("You walk out the south door", 500);
                        Services.ScrollText("You get your gun and knife back on the way out.", 1000);
                        _player.Bag.Add(gun);
                        _player.Bag.Add(knife);
                        Timer();
                        RedSquareEnterance();
                        break;
                    case 4:
                        Services.ScrollText("You walk down the stairs towards the basement", 500);
                        Timer();
                        Basement();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        Stage();
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
                    Timer();
                    Hotel();
                }
                Hotel();
            }
        }

        public void StageBack()
        {
            Location = "the back of the stage";

            EnterArea(stageBack, stageBackDesc, stageBackTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You take a closer look at the pile of tarps, looks like it's just a bunch of dirty tarps.", 500);
                        Timer();
                        StageBack();
                        break;
                    case 2:
                        Services.ScrollText("You take a closer look at the stack of crates, they're full of fireworks!", 500);
                        Timer();
                        StageBack();
                        break;
                    case 3:
                        Services.ScrollText("You walk back to the front of the stage", 500);
                        Timer();
                        Stage();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        StageBack();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                if (Item != null && Target != null)
                {
                    if (Target == "Guard")
                    {
                        if (Item.Name != "Gun")
                        {
                            Services.ScrollText("WHAT DO YOU THINK YOU'RE DOING?", 500);
                            Services.ScrollText("Code red! Code red!", 1000);
                            Services.ScrollText(". . .", 250);
                            Services.ScrollText("Within seconds you're surrounded and outnumbered. It's hard to complete your mission from a prison cell.", 1000);
                            Services.ScrollText("Game Over", 1000);
                            Services.PlayAgain();
                        }
                        else
                        {
                            Services.ScrollText("You shoot the guard in the back!", 500);
                            Services.ScrollText("He slumps to the ground.", 500);
                            Services.ScrollText(". . .", 250);
                            Services.ScrollText("Within seconds you're surrounded and outnumbered. It's hard to complete your mission from a prison cell.", 1000);
                            Services.ScrollText("Game Over", 1000);
                            Services.PlayAgain();
                        }
                    }
                    else if (Target == "Crates")
                    {
                        if (Item.Name == "Gun")
                        {
                            Services.ScrollText("You take a few steps back and fire your gun at the crates", 500);
                            Services.ScrollText("BOOOOOOM. . .", 1000);
                            Services.ScrollText("What a huge explosion!", 500);
                            Services.ScrollText("It looks like it knocked the guard on his head, he's really passed out!");
                            Services.ScrollText("You take your opportunity to get out of there before someone realizes you caused the explosion.");
                            Item = null;
                            Target = null;
                            _state++;
                            Timer();
                            Stage();
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            Item = null;
                            Target = null;
                            Timer();
                            StageBack();
                        }
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        Item = null;
                        Target = null;
                        Timer();
                        StageBack();
                    }
                }
                else
                {
                    StageBack();
                }
                
            }
        }

        public void Basement()
        {
            Location = "the hotel's basement";
            if (_state == 2)
            {
                EnterArea(basement, basementDesc, basementTarget);

                Services.ScrollText("(Bodrov Ilyich is sitting with his back to you and doesn't seem to see you)", 500);

                int input;
                string decision = Console.ReadLine();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("You walk upstairs to the hotel lobby.", 500);
                            Timer();
                            Hotel();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            Basement();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Target == "Bodrov Ilyich")
                        {
                            if (Item.Name == "Twine")
                            {
                                Services.ScrollText("You sneak up behind Bodrov and quickly wrap the twine around his neck...", 1500);
                                Services.ScrollText("After a brief struggle, he sits motionless on the couch in front of you.");
                                _player.LevelCompleted = true;
                            }
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            Item = null;
                            Target = null;
                            Timer();
                            Basement();
                        }
                        
                    }
                    else
                    {
                        Basement();
                    }
                }
            }
            else
            {
                Services.ScrollText("Uh oh, there's a guard at the doorway at the bottom of the stairs");
                Services.ScrollText("Hey! No one is allowed down here. Go back upstairs!");
                Timer();
                Hotel();
            }
        }

        public void ScanCrowd()
        {
            switch (_state)
            {
                case 1:
                    Services.ScrollText("You scan around you, but you see nothing of interest.", 500);
                    break;
                case 2:
                    Services.ScrollText("Everyone is paying attention to the stage. The diversion worked!", 500);
                    break;
            }
            
        }

        public void Receptionist()
        {
            Console.WriteLine();
            foreach (string option in receptionist)
            {
                Services.ScrollText((Array.IndexOf(receptionist, option) + 1) + " - " + option);
            }

            int input;
            string decision = Console.ReadLine();
            Console.WriteLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("\"" + receptionist[input - 1] + "\"\n", 1000);
                        Services.ScrollText("I believe the speech will be starting in about " + _timer / 10 + " minutes.", 1000);
                        Receptionist();
                        break;
                    case 2:
                        Services.ScrollText("\"" + receptionist[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Yes it is! We were able to give him our best suite down in the basement level!", 1000);
                        Receptionist();
                        break;
                    case 3:
                        Services.ScrollText("\"" + receptionist[input - 1] + "\"\n", 1000);
                        Services.ScrollText("No I'm sorry. We're all booked up for the night.", 1000);
                        Receptionist();
                        break;
                    case 4:
                        Services.ScrollText("\"" + receptionist[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Bye!");
                        Hotel();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        Receptionist();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                Receptionist();
            }
        }

        public void Guard()
        {
            Console.WriteLine();
            foreach (string option in guard)
            {
                Services.ScrollText((Array.IndexOf(guard, option) + 1) + " - " + option);
            }

            int input;
            string decision = Console.ReadLine();
            Console.WriteLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("\"" + guard[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Code red! Code red!", 1000);
                        Services.ScrollText(". . .", 250);
                        Services.ScrollText("Within seconds you're surrounded and outnumbered. It's hard to complete your mission from a prison cell.", 1000);
                        Services.ScrollText("Game Over", 1000);
                        Services.PlayAgain();
                        break;
                    case 2:
                        Services.ScrollText("\"" + guard[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Oh. Well, umm... fine. Make it fast.", 1000);
                        _talkToGuard = true;
                        StageBack();
                        break;
                    case 3:
                        Services.ScrollText("\"" + guard[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Yeah, scram.", 1000);
                        Stage();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        Guard();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                Guard();
            }
        }

        public void Timer()
        {
            _timer--;
            switch (_timer)
            {
                case 40:
                    Services.ScrollText("There are 4 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                    break;
                case 30:
                    Services.ScrollText("There are 3 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                    break;
                case 20:
                    Services.ScrollText("There are 2 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                    break;
                case 10:
                    Services.ScrollText("There is 1 minute until he makes it to that stage; you must eliminate the target soon!", 500);
                    break;
                case 0:
                    Services.ScrollText("You're too late! The speech is starting. Mission failed.", 500);
                    Services.PlayAgain();
                    break;
            }
        }
    }
}
