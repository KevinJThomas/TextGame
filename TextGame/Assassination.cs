using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextGame
{
    class Assassination : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        bool _talkToGuard = false;
        int _timer;

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
        string basementDesc = "An empty room except for a trashy couch and a tv. The lighting is very poor.";

        //Targets for items for each area
        string[] redSquareEnteranceTarget = new string[] { "Crowd" };
        string[] stageFrontTarget = new string[] { "Crowd", "Stage" };
        string[] hotelTarget = new string[] { "Receptionist" };
        string[] stageBackTarget = new string[] { "Guard", "Tarps", "Crates" };
        string[] basementTarget = new string[] { "Bodrov Ilyich", "Couch" };

        //Items the player will start with
        Item gun = new Item("Gun");
        Item knife = new Item("Knife");
        Item twine = new Item("Twine");

        public Assassination(Player player) : base(player)
        {
            _player = player;
            List<Item> items = new List<Item>() { gun, knife, twine };
            _player.Bag.Add(items);
            musicThread.Start();

            switch (_player.DifficultyLevel)
            {
                case 1:
                    _timer = 75;
                    break;
                case 2:
                    _timer = 50;
                    break;
                case 3:
                    _timer = 25;
                    break;
                case 4:
                    _timer = 10;
                    break;
            }
        }

        public static void PlayMusic()
        {
            Assembly assembly;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream
                ("TextGame.audio.drums.wav"));
            sp.Stream.Position = 0;
            sp.PlayLooping();
        }

        public void Start()
        {
            Services.ScrollText("You are a Russian KGB operative.", 1200);
            Services.ScrollText("There has been a small uprising against the government in Moscow that has a chance of gaining some traction.", 1200);
            Services.ScrollText("The uprising is being lead by a very vocal individual: Bodrov Ilyich", 1200);
            Services.ScrollText("He must be silenced before too much damage is done. This your mission.", 1200);
            Services.ScrollText("He is scheduled to be speaking in the Red Square in 5 minutes.", 1200);
            Services.ScrollText("You are arriving to the square now; there isn't much time to eliminate the target.", 1200);
            Services.ScrollText("In your bag you are carrying a handgun, knife, and 3 feet of twine.", 1200);
            Services.ScrollText("Find and kill Bodrov Ilyich. Your life depends on it.", 3500);

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
                    if (Target == "Crowd")
                    {
                        if (Item.Name == "Gun")
                        {
                            if (_player.DifficultyLevel > 2)
                            {
                                Services.ScrollText("You fire your gun into the crowd!", 1200);
                                Services.ScrollText("You're instantly swarmed by guards, disarmed, and enprisoned.", 1500);
                            }
                            else
                            {
                                Services.ScrollText("You probably shouldn't shoot innocent people..", 500);
                                Timer();
                                Item = null;
                                Target = null;
                                RedSquareEnterance();
                            }
                        }
                        else if (Item.Name == "Knife")
                        {
                            if (_player.DifficultyLevel > 2)
                            {
                                Services.ScrollText("You pull out your knife. . .panic ensues in the crowd.", 1200);
                                Services.ScrollText("After a few seconds of chaos, you are shot by a nearby guard", 1500);
                            }
                            else
                            {
                                Services.ScrollText("You probably shouldn't hurt innocent people..", 500);
                                Timer();
                                Item = null;
                                Target = null;
                                RedSquareEnterance();
                            }
                        }
                        else if (Item.Name == "Twine")
                        {
                            Services.ScrollText("You try to show off your twine to a few folks, but no one shows any interest.");
                            Timer();
                            Item = null;
                            Target = null;
                            RedSquareEnterance();
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            Timer();
                            Item = null;
                            Target = null;
                            RedSquareEnterance();
                        }
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        Timer();
                        Item = null;
                        Target = null;
                        RedSquareEnterance();
                    }
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
                    if (Target == "Crowd")
                    {
                        if (Item.Name == "Gun")
                        {
                            if (_player.DifficultyLevel > 2)
                            {
                                Services.ScrollText("You fire your gun into the crowd!", 1200);
                                Services.ScrollText("You're instantly swarmed by guards, disarmed, and enprisoned.", 1500);
                            }
                            else
                            {
                                Services.ScrollText("You probably shouldn't shoot innocent people..", 500);
                                Timer();
                                Item = null;
                                Target = null;
                                Stage();
                            }
                        }
                        else if (Item.Name == "Knife")
                        {
                            if (_player.DifficultyLevel > 2)
                            {
                                Services.ScrollText("You pull out your knife. . .panic ensues in the crowd.", 1200);
                                Services.ScrollText("After a few seconds of chaos, you are shot by a nearby guard", 1500);
                            }
                            else
                            {
                                Services.ScrollText("You probably shouldn't hurt innocent people..", 500);
                                Timer();
                                Item = null;
                                Target = null;
                                Stage();
                            }
                        }
                        else if (Item.Name == "Twine")
                        {
                            Services.ScrollText("You try to show off your twine to a few folks, but no one shows any interest.");
                            Timer();
                            Item = null;
                            Target = null;
                            Stage();
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            Timer();
                            Item = null;
                            Target = null;
                            Stage();
                        }
                    }
                    else if (Target == "Stage")
                    {
                        Services.ScrollText("It's too far away!", 500);
                        Item = null;
                        Target = null;
                        Timer();
                        Stage();
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        Item = null;
                        Target = null;
                        Timer();
                        Stage();
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.", 500);
                    Stage();
                }
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
                    if (_player.DifficultyLevel >= 4)
                    {
                        if (Target == "Receptionist")
                        {
                            if (Item.Name == "Twine")
                            {
                                Services.ScrollText("You show the receptionist your twine..", 800);
                                Services.ScrollText("GUARDS! GUARDS! HE HAS A WEAPON!", 800);
                            }
                            else
                            {
                                Services.ScrollText("It's not very effective.", 500);
                                Item = null;
                                Target = null;
                                Timer();
                                Hotel();
                            }
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            Item = null;
                            Target = null;
                            Timer();
                            Hotel();
                        }
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        Item = null;
                        Target = null;
                        Timer();
                        Hotel();
                    }
                }
                else
                {
                    Hotel();
                }
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
                        else if (Item.Name == "Knife" && _player.DifficultyLevel >= 4)
                        {
                            Services.ScrollText("You pull out your knife and stab in into a crate..", 800);
                            Services.ScrollText("BOOOOOOM. . .", 1000);
                            Services.ScrollText("The explosion kills you..", 600);
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
                        else if (Target == "Couch" && _player.DifficultyLevel >= 3)
                        {
                            Services.ScrollText("You walk up the couch and place your twine on it.", 800);
                            Services.ScrollText("Bodrov hears you and jolts out of the couch, gun in hand", 1000);
                            Services.ScrollText("You're left defenseless against your target", 1000);
                            Services.ScrollText("POW!", 400);
                            Services.ScrollText("He shoots you and you quickly slip into darkness", 800);
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
                Services.ScrollText("Uh oh, there's a guard at the doorway at the bottom of the stairs.");
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
                Services.ScrollText("Invalid input. Try again.", 500);
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
                Services.ScrollText("Invalid input. Try again.", 500);
                Guard();
            }
        }

        public void Timer()
        {
            _timer--;
            switch (_player.DifficultyLevel)
            {
                case 1:
                    switch (_timer)
                    {
                        case 60:
                            Services.ScrollText("There are 4 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 45:
                            Services.ScrollText("There are 3 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 30:
                            Services.ScrollText("There are 2 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 15:
                            Services.ScrollText("There is 1 minute until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 0:
                            Services.ScrollText("You're too late! The speech is starting. Mission failed.", 500);
                            Services.PlayAgain();
                            break;
                    }
                    break;
                case 2:
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
                    break;
                case 3:
                    switch (_timer)
                    {
                        case 20:
                            Services.ScrollText("There are 4 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 15:
                            Services.ScrollText("There are 3 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 10:
                            Services.ScrollText("There are 2 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 5:
                            Services.ScrollText("There is 1 minute until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 0:
                            Services.ScrollText("You're too late! The speech is starting. Mission failed.", 500);
                            Services.PlayAgain();
                            break;
                    }
                    break;
                case 4:
                    switch (_timer)
                    {
                        case 8:
                            Services.ScrollText("There are 4 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 6:
                            Services.ScrollText("There are 3 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 4:
                            Services.ScrollText("There are 2 minutes until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 2:
                            Services.ScrollText("There is 1 minute until he makes it to that stage; you must eliminate the target soon!", 500);
                            break;
                        case 0:
                            Services.ScrollText("You're too late! The speech is starting. Mission failed.", 500);
                            Services.PlayAgain();
                            break;
                    }
                    break;
            }
            
        }
    }
}
