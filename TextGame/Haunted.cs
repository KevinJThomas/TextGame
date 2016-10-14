using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextGame
{
    class Haunted : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);
        Thread colorThread = new Thread(ConsoleColors);
        
        //Location options
        string[] darkRoom = new string[] { "Slowly Wander Forward", "Scream for Help" };
        string[] bedroom = new string[] { "Examine Nightstand", "Examine Bed", "Flip Lightswitch", "Go into Hallway", "Scream for Help" };
        string[] bedRoomWall = new string[] { "Walk Along the Wall", "Leave the Wall", "Scream for Help" };
        string[] nightstand = new string[] { "Pick up Lighter", "Leave the Nightstand", "Scream for Help" };
        string[] hallway = new string[] { "Walk to Living Room", "Go into Bedroom", "Go into Bathroom", "Scream for Help" };
        string[] livingRoom = new string[] { "Examine Table", "Examine Bookcase", "Go out Front Door", "Take Stairs to the Basement", "Go into Hallway", "Scream for Help" };
        string[] table = new string[] { "Pick up Gun", "Pick up Vial", "Pick up Amulet", "Leave the Table", "Scream for Help" };
        string[] basement = new string[] { "Examine Water Tanks", "Examine Furnace", "Examine Chair", "Examine Hole in the Wall",
            "Examine Box", "Examine Bag", "Take Stairs to the Living Room", "Scream for Help" };
        string[] chair = new string[] { "Eat the Red Apple", "Eat the Green Apple", "Leave the Chair", "Scream for Help" };
        string[] holeInTheWall = new string[] { "Crawl Through the Hole", "Leave the Hole", "Scream for Help" };

        //Location descriptions
        string darkRoomDesc = "You can't see anything..";
        string bedroomDesc = "It looks like a normal bedroom. There is a nicely made desk with an empty nightstand next to it in the corner.";
        string hallwayDesc = "A hallway with 2 doors in it. It looks like it leads to a living room.";
        string livingRoomDesc = "A small room with one table, one door, and a staircase going down to a basement.";
        string tableDesc = "You see an average looking wooden table in front of you.";
        string basementDesc = "A cold basement with a furnace in front of you and some type of water tanks behind you.";
        string chairDesc = "You see an average looking wooden chair in front of you.";

        //Location targets
        string[] noTarget = new string[] { "Yourself" };
        string[] bedroomTarget = new string[] { "Nightstand", "Bed", "Lightswitch", "Door", "Window", "Yourself" };
        string[] hallwayTarget = new string[] { "Bedroom Door", "Bathroom Door", "Yourself" };
        string[] livingRoomTarget = new string[] { "Bookcase", "Front Door", "Table", "Yourself" };
        string[] basementTarget = new string[] { "Water Tanks", "Furnace", "Chair", "Hole in the Wall", "Yourself" };

        //Water tank puzzle
        string[] tankOptions = new string[] { "Sink", "1 Tank", "5 Tank", "7 Tank" };
        string[] tankState = new string[] { "", "0", "0", "0" };
        int tank1 = 0;
        int tank5 = 0;
        int tank7 = 0;
        bool _tankPuzzleComplete = false;

        Item torch = new Item("Torch");
        Item lighter = new Item("Lighter");
        Item sheet = new Item("Sheet");
        Item gun = new Item("Gun");
        Item vial = new Item("Vial");
        Item amulet = new Item("Amulet");
        Item shuriken = new Item("Shuriken");
        Item holyWater = new Item("Holy Water");
        Item bandage = new Item("Bandage");

        bool _dead = false;
        int _bleeding = 0;

        bool _firstTimeLivingRoom = true;
        bool _enteredBasement = false;
        bool _reenteredBedroom = false;

        bool _lighterPickedUp = false;
        bool _sheetsPickedUp = false;
        bool _tableItemPickedUp = false;
        bool _amuletInBag = false;
        bool _appleEaten = false;
        bool _doorSafe = false;
        bool _boxOpened = false;
        bool _bagOpened = false;
        bool _ghostParalyzed = false;

        bool _tableMetal = false;
        bool _tableGlass = false;

        bool _light = false;
        bool _switchUp = false;
        bool _torchLit = false;

        int _screamCounter = 0;

        static bool _gameRunning = true;
        static bool _sLight = false;
        static bool _blackCon = true;

        Random rand = new Random();

        public Haunted(Player player) : base(player)
        {
            _player = player;
            _player.Health = 100;
            _player.Add(torch);
            musicThread.Start();
            colorThread.Start();

            Thread bleedingThread = new Thread(Bleeding);
            bleedingThread.Start();
        }

        public static void ConsoleColors()
        {
            while (_gameRunning)
            {
                if (_sLight && _blackCon)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    _blackCon = false;
                }
                else if (!_sLight && !_blackCon)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    _blackCon = true;
                }
            }
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public static void PlayMusic()
        {
            Assembly assembly;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream
                ("TextGame.audio.eerieMusic.wav"));
            sp.Stream.Position = 0;
            sp.PlayLooping();
        }

        public void Start()
        {
            Services.ScrollText("You're in a dark room.", 1500);
            Services.ScrollText("You can't see anyone else, but there's a feeling like you're not alone.", 1500);
            Services.ScrollText("You check your bag..", 800);
            Services.ScrollText("You have a torch! Now if there was only some way to light it..", 2000);
            Services.ScrollText("You hear footsteps behind you.", 700);
            Services.ScrollText("You spin around! . . . There is nothing there.", 1700);
            Services.ScrollText("You hear a deep voice slowly and quietly chuckle to himself 'heh..heh..heh'", 1900);
            Services.ScrollText("What is going on?!", 2000);
            Bedroom();
        }

        public void Bedroom()
        {
            if (!_dead)
            {
                Spook();
                if (_light)
                {
                    Location = "the bedroom";
                    EnterArea(bedroom, bedroomDesc, bedroomTarget);

                    int input;
                    string decision = TakeCmd();

                    if (Int32.TryParse(decision, out input))
                    {
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You walk over to the nightstand.", 500);
                                Nightstand();
                                break;
                            case 2:
                                if (_sheetsPickedUp)
                                {
                                    Services.ScrollText("You search the bed, but there's nothing interesting there.", 500);
                                }
                                else
                                {
                                    Services.ScrollText("There is a nicely folded sheet sitting on top of the bed.\nWould you like to pick it up? (y/n)");
                                    string answer = TakeCmd();
                                    if (answer == "y" || answer == "yes")
                                    {
                                        _sheetsPickedUp = true;
                                        _player.Add(sheet);
                                        Services.ScrollText("You put the sheet in your bag.");
                                    }
                                }
                                Bedroom();
                                break;
                            case 3:
                                Services.ScrollText("You flip the light switch.", 1000);
                                FlipLight();
                                Thread.Sleep(300);
                                Console.Clear();
                                if (_enteredBasement && !_light)
                                {
                                    Services.ScrollText("It's pitch black.. you hear a strange, slow ticking noise in the walls.", 1500);
                                    Services.ScrollText(_player.Name + "...", 800);
                                    Services.ScrollText("Your world is so weak.", 1000);
                                    Services.ScrollText("Give in to my presence, and I will show you the power that you can have in my world. (y/n)");
                                    string giveIn = TakeCmd();

                                    if (giveIn.ToLower() == "y" || giveIn.ToLower() == "yes")
                                    {
                                        Services.ScrollText("MUAHAHA. . . .", 1200);
                                        Services.ScrollText("You fool. Your body is mine, and you are now damned to exist in between this world and the next like " +
                                            "I once was.", 2000);
                                        Services.ScrollText("Your world is DOOOOOOMED!");
                                        _dead = true;
                                    }
                                    else
                                    {
                                        Services.ScrollText("How dare you defy me!", 500);
                                        Services.ScrollText("Now you will suffer. . .", 600);
                                        Services.ScrollText(". . . .", 500);
                                        Services.ScrollText("You need to think of something fast!", 500);
                                        Services.ScrollText("It's too dark to see what's going on.. you'll have to use an item you already have in your bag!", 500);
                                        Suffer();
                                    }
                                }
                                else
                                {
                                    Bedroom();
                                }
                                break;
                            case 4:
                                Services.ScrollText("You walk into the hallway.", 500);
                                if (_torchLit)
                                {
                                    Hallway();
                                }
                                else
                                {
                                    Services.ScrollText("It's pitch black..", 1500);
                                    Services.ScrollText("YOU HEAR A DEAFENING SCREAM", 1000);
                                    Services.ScrollText("You run back into the bedroom, slightly rattled.", 1200);
                                    TakeDmg(3);
                                    Bedroom();
                                }

                                break;
                            case 5:
                                Scream();
                                Bedroom();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Bedroom();
                                break;
                        }
                    }
                    else
                    {
                        ExamineCommand(decision);
                        if (Item != null && Target != null)
                        {
                            if (Item.Name == "Lighter" && Target == "Lighter")
                            {
                                Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.", 1000);
                                Thread torchThread = new Thread(LightTorch);
                                torchThread.Start();
                            }
                            else if (Item.Name == "Sheet" && Target == "Yourself")
                            {
                                if (_bleeding > 0)
                                {
                                    Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                    _bleeding--;
                                    _player.Bag.Remove(Item);
                                }
                                else
                                {
                                    Services.ScrollText("It's not very effective.", 500);
                                }
                            }
                            else if (Item.Name == "Bandage" && Target == "Yourself")
                            {
                                if (_bleeding > 0)
                                {
                                    Services.ScrollText("You place the bandage around your wound to stop the bleeding.", 1000);
                                    _bleeding--;
                                    _player.Bag.Remove(Item);
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
                            Bedroom();
                        }
                        else
                        {
                            Bedroom();
                        }
                    }
                }
                else
                {
                    Location = "a dark room";
                    EnterArea(darkRoom, darkRoomDesc, noTarget);

                    int input;
                    string decision = TakeCmd();

                    if (Int32.TryParse(decision, out input))
                    {
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You put your hand up in front of you and inch forward.. you feel two walls.\nYou must be in the corner of the room.", 1000);
                                BedroomWall();
                                break;
                            case 2:
                                Scream();
                                Bedroom();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Bedroom();
                                break;
                        }
                    }
                    else
                    {
                        ExamineCommand(decision);
                        if (Item != null && Target != null)
                        {
                            if (Item.Name == "Lighter" && Target == "Lighter")
                            {
                                Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                                Thread torchThread = new Thread(LightTorch);
                                torchThread.Start();
                            }
                            else if (Item.Name == "Sheet" && Target == "Yourself")
                            {
                                if (_bleeding > 0)
                                {
                                    Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                    _bleeding--;
                                    _player.Bag.Remove(Item);
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
                            Bedroom();
                        }
                        else
                        {
                            Bedroom();
                        }
                    }
                }
            }
        }

        public void Nightstand()
        {
            if (!_dead)
            {
                Spook();
                Location = "the corner by the nightstand";
                EnterArea(nightstand, bedroomDesc, noTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            if (_lighterPickedUp == false)
                            {
                                Services.ScrollText("You put the lighter into your bag", 1000);
                                _lighterPickedUp = true;
                                _player.Add(lighter);
                                nightstand = new string[] { "Leave the Nightstand", "Scream for Help" };
                                Nightstand();
                            }
                            else
                            {
                                Services.ScrollText("You step away from the nightstand.", 1000);
                                Bedroom();
                            }
                            break;
                        case 2:
                            if (_lighterPickedUp == false)
                            {
                                Services.ScrollText("You step away from the nightstand.", 1000);
                                Bedroom();
                            }
                            else
                            {
                                Scream();
                                Nightstand();
                            }
                            break;
                        case 3:
                            if (_lighterPickedUp == false)
                            {
                                Scream();
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please try again.", 1000);
                            }
                            Nightstand();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.", 1000);
                            Nightstand();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
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
                        Nightstand();
                    }
                    else
                    {
                        Nightstand();
                    }
                }
            }
        }

        public void Hallway()
        {
            if (!_dead)
            {
                Spook();
                Location = "the hallway";
                EnterArea(hallway, hallwayDesc, hallwayTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("You walk down the hallway into the living room.", 2000);
                            if (_firstTimeLivingRoom)
                            {
                                _firstTimeLivingRoom = false;
                                Services.ScrollText("You hear something behind you..", 1500);
                                Services.ScrollText("You spin around. . . there's nothing there!", 1000);
                                Services.ScrollText("OUCH!", 300);
                                Services.ScrollText("Something bit your arm.. you're bleeding!", 2200);
                                TakeDmg(5);
                                _bleeding++;
                            }
                            Services.ScrollText("The living room is actually lit!", 1000);
                            LivingRoom();
                            break;
                        case 2:
                            Services.ScrollText("You walk through the bedroom door.", 500);
                            if (_enteredBasement && !_reenteredBedroom)
                            {
                                Services.ScrollText("WWWHHHOOOSSHH", 400);
                                Services.ScrollText("A group of shurkiens flys past you!", 800);
                                Services.ScrollText("One of the clips your shoulder and it starts to bleed", 1100);
                                Services.ScrollText("Someone must really not want you here..", 1500);
                                Services.ScrollText("You take a shurkien off the wall it is stuck in and put it in your bag, this might come in handy.", 1500);
                                _player.Add(shuriken);
                                TakeDmg(8);
                                _bleeding++;
                                _reenteredBedroom = true;
                            }
                            Bedroom();
                            break;
                        case 3:
                            Services.ScrollText("The door is locked.");
                            Hallway();
                            break;
                        case 4:
                            Scream();
                            Hallway();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            Hallway();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
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
                        Hallway();
                    }
                    else
                    {
                        Hallway();
                    }
                }
            }
        }

        public void BedroomWall()
        {
            if (!_dead)
            {
                Spook();
                Location = "the corner of a dark room";
                EnterArea(bedRoomWall, darkRoomDesc, noTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("You feel your way down the wall. . .", 2000);
                            Services.ScrollText("You stop as you feel something on the wall.. it feels like a lightswitch!", 2000);
                            Services.ScrollText("Flip the lightswitch? (y/n)");

                            string flipLight = TakeCmd();

                            if (flipLight.ToLower() == "y" || flipLight.ToLower() == "yes")
                            {
                                FlipLight();
                                Thread.Sleep(500);
                                Console.Clear();
                                if (_light)
                                {
                                    Services.ScrollText("You can see!", 2000);
                                }
                            }
                            else
                            {
                                Services.ScrollText("You step away from the lightswitch without touching it.");
                            }

                            Bedroom();
                            break;
                        case 2:
                            Services.ScrollText("You step away from the wall.");
                            Bedroom();
                            break;
                        case 3:
                            Scream();
                            BedroomWall();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            BedroomWall();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
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
                        BedroomWall();
                    }
                    else
                    {
                        BedroomWall();
                    }
                }
            }
        }

        public void LivingRoom()
        {
            if (!_dead)
            {
                Spook();
                Location = "the living room";
                EnterArea(livingRoom, livingRoomDesc, livingRoomTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("You walk over to the table.", 500);
                            if (!_tableItemPickedUp)
                            {
                                Services.ScrollText("There are 3 items on the table.. and they're all glowing!");
                            }
                            Table();
                            break;
                        case 2:
                            Services.ScrollText("You walk over to the bookcase.", 500);
                            Services.ScrollText("There's one book that isn't covered in dust, maybe it's important", 1000);
                            Services.ScrollText("You pick it up and open it to the bookmarked page.", 1200);
                            Services.ScrollText("The bookmarked page says:");
                            Services.ScrollText("HOLY WATER");
                            Services.ScrollText("The only way to acquire holy water is through infusion of elements.\nWood, metal, and glass must be placed together in" + 
                                " a place of darkness for holy water to form.", 3500);
                            LivingRoom();
                            break;
                        case 3:
                            Services.ScrollText("You try to open the front door.", 1000);
                            if (!_doorSafe)
                            {
                                Services.ScrollText("When you grab the door handle you're flung back by some kind of force.", 750);
                                TakeDmg(3);
                                LivingRoom();
                            }
                            else
                            {
                                Services.ScrollText("It's safe to touch because of the holy water, and you're able to walk straight out.", 500);
                                Services.ScrollText("You escaped with your life!", 4000);
                                _player.LevelCompleted = true;
                                _gameRunning = false;
                            }
                            
                            break;
                        case 4:
                            Services.ScrollText("You walk down the staircase.", 500);
                            Basement();
                            break;
                        case 5:
                            Services.ScrollText("You walk into the hallway.", 500);
                            if (_torchLit)
                            {
                                Hallway();
                            }
                            else
                            {
                                Services.ScrollText("It's pitch black..", 1500);
                                Services.ScrollText("YOU HEAR A DEAFENING SCREAM", 1000);
                                Services.ScrollText("You run back into the living room, slightly rattled.", 1200);
                                TakeDmg(3);
                                LivingRoom();
                            }
                            break;
                        case 6:
                            Scream();
                            LivingRoom();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            LivingRoom();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
                            }
                            else
                            {
                                Services.ScrollText("It's not very effective.", 500);
                            }
                        }
                        else if (Item.Name == "Vial" && Target == "Table")
                        {
                            Services.ScrollText("You place the vial on the table.", 500);
                            _player.Bag.Remove(vial);
                            _tableGlass = true;
                            if (_tableGlass && _tableMetal)
                            {
                                Services.ScrollText("The table begins to shake...", 1000);
                                Services.ScrollText("You hear a loud anguished screaming...", 1500);
                                Services.ScrollText("Suddenly, the shaking and screaming stop.", 500);
                                Services.ScrollText("The vial is now full of water! You put it back in your bag.", 2000);
                                _player.Add(holyWater);
                                _tableMetal = false;
                                _tableGlass = false;
                            }
                        }
                        else if (Item.Name == "Shuriken" && Target == "Table")
                        {
                            Services.ScrollText("You place the shuriken on the table.", 500);
                            _player.Bag.Remove(shuriken);
                            _tableMetal = true;
                            if (_tableGlass && _tableMetal)
                            {
                                Services.ScrollText("The table begins to shake...", 1000);
                                Services.ScrollText("You hear a loud anguished screaming...", 1000);
                                Services.ScrollText("Suddenly, the shaking and screaming stop.", 500);
                                Services.ScrollText("The vial is now full of water! You put it back in your bag.", 1500);
                                _player.Add(holyWater);
                                _tableMetal = false;
                                _tableGlass = false;
                            }
                        }
                        else if (Item.Name == "Holy Water" && Target == "Front Door")
                        {
                            Services.ScrollText("You pour the holy water onto the handle of the front door.", 1000);
                            _doorSafe = true;
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                        }
                        Item = null;
                        Target = null;
                        LivingRoom();
                    }
                    else
                    {
                        LivingRoom();
                    }
                }
            }
        }

        public void Table()
        {
            if (!_dead)
            {
                Spook();
                Location = "the middle of the room by the table";
                EnterArea(table, tableDesc, noTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    if (!_tableItemPickedUp)
                    {
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You take the gun off the table and put it in your bag.. the other items disappeared!", 500);
                                Services.ScrollText("The gun isn't loaded.. that's not very helpful");
                                _player.Add(gun);
                                _tableItemPickedUp = true;
                                table = new string[] { "Leave the Table", "Scream for Help" };
                                Table();
                                break;
                            case 2:
                                Services.ScrollText("You take the vial off the table and put it in your bag.. the other items disappeared!", 500);
                                _player.Add(vial);
                                _tableItemPickedUp = true;
                                table = new string[] { "Leave the Table", "Scream for Help" };
                                Table();
                                break;
                            case 3:
                                Services.ScrollText("You take the amulet off the table and put it in your bag.. the other items disappeared!", 500);
                                Services.ScrollText("You can see the amulet glowing through your bag.", 1000);
                                _player.Add(amulet);
                                _amuletInBag = true;
                                _tableItemPickedUp = true;
                                table = new string[] { "Leave the Table", "Scream for Help" };
                                Table();
                                break;
                            case 4:
                                Services.ScrollText("You walk away from the table.", 500);
                                LivingRoom();
                                break;
                            case 5:
                                Scream();
                                Table();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Table();
                                break;
                        }
                    }
                    else
                    {
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You walk away from the table.", 500);
                                LivingRoom();
                                break;
                            case 2:
                                Scream();
                                Table();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Table();
                                break;
                        }
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
                            }
                            else
                            {
                                Services.ScrollText("It's not very effective.", 500);
                            }
                        }
                        else if (Item.Name == "Vial" && Target == "Table")
                        {
                            Services.ScrollText("You place the vial on the table.", 500);
                            _player.Bag.Remove(vial);
                            _tableGlass = true;
                            if (_tableGlass && _tableMetal)
                            {
                                Services.ScrollText("The table begins to shake...", 1000);
                                Services.ScrollText("You hear a loud anguished screaming...", 1000);
                                Services.ScrollText("Suddenly, the shaking and screaming stop.", 500);
                                Services.ScrollText("The vial is now full of water! You put it back in your bag.", 1500);
                                _player.Add(holyWater);
                                _tableMetal = false;
                                _tableGlass = false;
                            }
                        }
                        else if (Item.Name == "Shuriken" && Target == "Table")
                        {
                            Services.ScrollText("You place the shuriken on the table.", 500);
                            _player.Bag.Remove(shuriken);
                            _tableMetal = true;
                            if (_tableGlass && _tableMetal)
                            {
                                Services.ScrollText("The table begins to shake...", 1000);
                                Services.ScrollText("You hear a loud anguished screaming...", 1000);
                                Services.ScrollText("Suddenly, the shaking and screaming stop.", 500);
                                Services.ScrollText("The vial is now full of water! You put it back in your bag.", 1500);
                                _player.Add(holyWater);
                                _tableMetal = false;
                                _tableGlass = false;
                            }
                        }
                        else if (Item.Name == "Holy Water" && Target == "Front Door")
                        {
                            Services.ScrollText("You pour the holy water onto the handle of the front door.", 1000);
                            _doorSafe = true;
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                        }
                        Item = null;
                        Target = null;
                        Table();
                    }
                    else
                    {
                        Table();
                    }
                }
            }
        }

        public void Basement()
        {
            if (_amuletInBag)
            {
                Services.ScrollText("You feel some kind of dark force draining your energy as you're in the basement..", 1300);
                TakeDmg(12);
            }
            if (!_dead)
            {
                _enteredBasement = true;
                Spook();
                Location = "the basement";
                EnterArea(basement, basementDesc, basementTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("You walk over to the water tanks.", 500);
                            Services.ScrollText("There are three tanks sitting on the ground, and there is a sink next to them.", 1000);
                            Services.ScrollText("One tank has a '5' on it, and the other, larger tank has a '7' on it.", 1000);
                            Services.ScrollText("The third and golden tank is the largest of them all, but only has a '1' on it.", 1500);
                            WaterTanks();
                            break;
                        case 2:
                            Services.ScrollText("That thing is really radiating heat! Probably shouldn't get too close.", 500);
                            Basement();
                            break;
                        case 3:
                            Services.ScrollText("You walk over to the chair.", 500);
                            Chair();
                            break;
                        case 4:
                            Services.ScrollText("You walk over to the hole in the wall.", 500);
                            HoleInTheWall();
                            break;
                        case 5:
                            if (!_boxOpened)
                            {
                                Services.ScrollText("There's a closed cardboard box sitting on the floor.", 800);
                                Services.ScrollText("Would you like to open it? (y/n)");

                                string openBox = TakeCmd();

                                if (openBox.ToLower() == "y" || openBox.ToLower() == "yes")
                                {
                                    Services.ScrollText("You open the box and find there is a bandage inside. You put it in your bag.", 2000);
                                    _player.Add(bandage);
                                    _boxOpened = true;
                                }
                            }
                            else
                            {
                                Services.ScrollText("It's just an empty cardboard box.", 700);
                            }
                            Basement();
                            break;
                        case 6:
                            if (!_bagOpened)
                            {
                                Services.ScrollText("There's a leather bag tied shut on the ground.", 800);
                                Services.ScrollText("Would you like to open it? (y/n)");

                                string openBag = TakeCmd();

                                if (openBag.ToLower() == "y" || openBag.ToLower() == "yes")
                                {
                                    Services.ScrollText("You open the leather bag..", 1100);
                                    Services.ScrollText("CENTIPEDES", 400);
                                    Services.ScrollText("A swarm of centipedes come crawling out!", 400);
                                    Services.ScrollText("They crawl up your arm, up your pant leg, and some even jump down your shirt.", 1500);
                                    Services.ScrollText("After finally brushing them all they scurry away", 800);
                                    Services.ScrollText("You can feel the dark presences here feeding off of your fear..your energy is being drained.");
                                    TakeDmg(15);
                                    _bagOpened = true;
                                }
                            }
                            else
                            {
                                Services.ScrollText("It's an empty leather bag.", 700);
                            }
                            Basement();
                            break;
                        case 7:
                            Services.ScrollText("You walk up the stairs towards the living room.", 500);
                            LivingRoom();
                            break;
                        case 8:
                            Scream();
                            Basement();
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
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
                            }
                            else
                            {
                                Services.ScrollText("It's not very effective.", 500);
                            }
                        }
                        else if (Item.Name == "Amulet" && Target == "Furnace")
                        {
                            Services.ScrollText("You throw the cursed amulet into the furnace.", 1000);
                            Services.ScrollText("It instantly melts away and disappears.", 1000);
                            _player.Bag.Remove(Item);
                            _amuletInBag = false;
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                        }
                        Item = null;
                        Target = null;
                        Basement();
                    }
                    else
                    {
                        Basement();
                    }
                }
            }
        }

        public void WaterTanks()
        {
            if (!_dead)
            {
                Services.ScrollText("(At any point you may type 'exit' to step away from the water tanks)");
                Services.ScrollText("Select what you would like to use as a resource:");
                PrintTankOptions();

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            Services.ScrollText("What would you like to fill using the sink?");
                            break;
                        case 2:
                            Services.ScrollText("What would you like to fill using the '1' tank?");
                            break;
                        case 3:
                            Services.ScrollText("What would you like to fill using the '5' tank?");
                            break;
                        case 4:
                            Services.ScrollText("What would you like to fill using the '7' tank?");
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            WaterTanks();
                            break;
                    }
                    if (input >= 1 && input <= 4)
                    {
                        PrintTankOptions();

                        int input2;
                        string decision2 = TakeCmd();

                        if (Int32.TryParse(decision2, out input2))
                        {
                            if (input2 >= 1 && input2 <= 4)
                            {
                                PourWater(input, input2);
                            }
                            else if (decision2.ToLower() == "exit")
                            {
                                Services.ScrollText("You step away from the water tanks.", 1000);
                                Basement();
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please try again.");
                                WaterTanks();
                            }
                        }
                    }
                }
                else if (decision.ToLower() == "exit")
                {
                    Services.ScrollText("You step away from the water tanks.", 1000);
                    Basement();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    WaterTanks();
                }
            }
        }

        public void PrintTankOptions()
        {
            foreach (string option in tankOptions)
            {
                Services.ScrollText((Array.IndexOf(tankOptions, option) + 1) + " - " + option + " (" + tankState[Array.IndexOf(tankOptions, option)] + ")");
            }
        }

        public void PourWater(int source, int target)
        {
            switch (source)
            {
                case 1:
                    switch (target)
                    {
                        case 1:
                            Services.ScrollText("You can't fill the sink with the sink, it's for pouring and draining water!", 1000);
                            break;
                        case 2:
                            Services.ScrollText("You fill the '1' tank with water.. nothing happens. You dump it out.", 1000);
                            break;
                        case 3:
                            Services.ScrollText("You fill the '5' tank with water.", 1000);
                            tank5 = 5;
                            tankState[2] = "5";
                            break;
                        case 4:
                            Services.ScrollText("You fill the '7' tank with water.", 1000);
                            tank7 = 7;
                            tankState[3] = "7";
                            break;
                        default:
                            Services.ScrollText("ERROR: PourWater(int, int). . .Unrecognized target #: " + target);
                            break;
                    }
                    break;
                case 2:
                    Services.ScrollText("The '1' tank in empty.");
                    break;
                case 3:
                    switch (target)
                    {
                        case 1:
                            Services.ScrollText("You dump out the '5' tank into the sink.", 1000);
                            tank5 = 0;
                            tankState[2] = "0";
                            break;
                        case 2:
                            Services.ScrollText("You dump  the '5' tank into the '1' tank..", 1000);
                            if (tank5 == 1)
                            {
                                Services.ScrollText("You hear a sound from the hole in the wall!", 1000);
                                _tankPuzzleComplete = true;
                                tank5 = 0;
                                tankState[2] = "0";
                                tank1 = 1;
                                tankState[1] = "1";
                            }
                            else
                            {
                                Services.ScrollText("Nothing happens so you dump it out.", 1000);
                                tank5 = 0;
                                tankState[2] = "0";
                            }
                            break;
                        case 3:
                            Services.ScrollText("You can't fill the '5' tank with itself..");
                            break;
                        case 4:
                            Services.ScrollText("You pour the '5' tank into the '7' tank.");
                            if (tank7 + tank5 <= 7)
                            {
                                tank7 = tank7 + tank5;
                                tankState[3] = tank7.ToString();
                                tank5 = 0;
                                tankState[2] = tank5.ToString();
                            }
                            else
                            {
                                int diff = 7 - tank7;
                                tank5 -= diff;
                                tankState[2] = tank5.ToString();
                                tank7 += diff;
                                tankState[3] = tank7.ToString();
                            }
                            break;
                        default:
                            Services.ScrollText("ERROR: PourWater(int, int). . .Unrecognized target #: " + target);
                            break;
                    }
                    break;
                case 4:
                    switch (target)
                    {
                        case 1:
                            Services.ScrollText("You dump out the '7' tank into the sink.", 1000);
                            tank7 = 0;
                            tankState[3] = "0";
                            break;
                        case 2:
                            Services.ScrollText("You dump  the '7' tank into the '1' tank..", 1000);
                            if (tank7 == 1)
                            {
                                Services.ScrollText("You hear a sound from the hole in the wall!", 1000);
                                _tankPuzzleComplete = true;
                                tank7 = 0;
                                tankState[3] = "0";
                                tank1 = 1;
                                tankState[1] = "1";
                            }
                            else
                            {
                                Services.ScrollText("Nothing happens so you dump it out.", 1000);
                                tank7 = 0;
                                tankState[3] = "0";
                            }
                            break;
                        case 3:
                            Services.ScrollText("You pour the '7' tank into the '5' tank.");
                            if (tank7 + tank5 <= 5)
                            {
                                tank5 = tank7 + tank5;
                                tankState[2] = tank5.ToString();
                                tank7 = 0;
                                tankState[3] = tank7.ToString();
                            }
                            else
                            {
                                int diff = 5 - tank5;
                                tank5 += diff;
                                tankState[2] = tank5.ToString();
                                tank7 -= diff;
                                tankState[3] = tank7.ToString();
                            }
                            break;
                        case 4:
                            Services.ScrollText("You can't fill the '7' tank with itself..");
                            break;
                        default:
                            Services.ScrollText("ERROR: PourWater(int, int). . .Unrecognized target #: " + target);
                            break;
                    }
                    break;
                default:
                    Services.ScrollText("ERROR: PourWater(int, int). . .Unrecognized source #: " + source);
                    break;
            }

            if (_tankPuzzleComplete)
            {
                Basement();
            }
            else
            {
                WaterTanks();
            }
        }

        public void Chair()
        {
            if (!_dead)
            {
                Spook();
                Location = "the corner of the room by the chair";
                EnterArea(chair, chairDesc, noTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    if (!_appleEaten)
                    {
                        int poisonApple = rand.Next(2);
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You eat the red apple", 500);
                                if (poisonApple == 0)
                                {
                                    Services.ScrollText("It was poisoned! You feel weaker...", 1200);
                                    TakeDmg(20);
                                }
                                else
                                {
                                    Services.ScrollText("It's revitalizing! You feel better.", 1200);
                                    if (_player.Health < 80)
                                    {
                                        _player.Health += 20;
                                    }
                                    else
                                    {
                                        _player.Health = 100;
                                    }
                                }
                                Services.ScrollText("When you look back at the chair, the other apple is gone.", 1500);
                                _appleEaten = true;
                                chair = new string[] { "Leave the Chair", "Scream for Help" };
                                Chair();
                                break;
                            case 2:
                                Services.ScrollText("You eat the red apple", 500);
                                if (poisonApple == 1)
                                {
                                    Services.ScrollText("It was poisoned! You feel weaker...", 1200);
                                    TakeDmg(20);
                                }
                                else
                                {
                                    Services.ScrollText("It's revitalizing! You feel better.", 1200);
                                    if (_player.Health < 80)
                                    {
                                        _player.Health += 20;
                                    }
                                    else
                                    {
                                        _player.Health = 100;
                                    }
                                }
                                Services.ScrollText("When you look back at the chair, the other apple is gone.", 1500);
                                _appleEaten = true;
                                chair = new string[] { "Leave the Chair", "Scream for Help" };
                                Chair();
                                break;
                            case 3:
                                Services.ScrollText("You walk back to the middle of the basement.", 500);
                                Basement();
                                break;
                            case 4:
                                Scream();
                                Chair();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Chair();
                                break;
                        }
                    }
                    else
                    {
                        switch (input)
                        {
                            case 1:
                                Services.ScrollText("You walk back to the middle of the basement.", 500);
                                Basement();
                                break;
                            case 2:
                                Scream();
                                Chair();
                                break;
                            default:
                                Console.WriteLine("Invalid input. Please try again.");
                                Chair();
                                break;
                        }
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
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
                        Chair();
                    }
                    else
                    {
                        Chair();
                    }
                }
            }
        }

        public void HoleInTheWall()
        {
            if (!_dead)
            {
                Spook();
                Location = "the corner by the hole in the wall";
                EnterArea(holeInTheWall, basementDesc, noTarget);

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    switch (input)
                    {
                        case 1:
                            if (_tankPuzzleComplete)
                            {
                                Services.ScrollText("You squeeze through the hole and starting crawling..", 1000);
                                Services.ScrollText("A few moments later you begin to see light, you're out!", 1500);
                                _player.LevelCompleted = true;
                                _gameRunning = false;
                            }
                            else
                            {
                                Services.ScrollText("You can't! It's blocked by a stone slab.", 500);
                                HoleInTheWall();
                            }
                            break;
                        case 2:
                            Services.ScrollText("You return to the middle of the basement.", 500);
                            Basement();
                            break;
                        case 3:
                            Scream();
                            HoleInTheWall();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            HoleInTheWall();
                            break;
                    }
                }
                else
                {
                    ExamineCommand(decision);
                    if (Item != null && Target != null)
                    {
                        if (Item.Name == "Lighter" && Target == "Lighter")
                        {
                            Services.ScrollText("You use the lighter to light the torch. . .this should last about 5 minutes.");
                            Thread torchThread = new Thread(LightTorch);
                            torchThread.Start();
                        }
                        else if (Item.Name == "Sheet" && Target == "Yourself")
                        {
                            if (_bleeding > 0)
                            {
                                Services.ScrollText("You wrap the sheet around your wound to stop the bleeding.", 1000);
                                _bleeding--;
                                _player.Bag.Remove(Item);
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
                        HoleInTheWall();
                    }
                    else
                    {
                        HoleInTheWall();
                    }
                }
            }
        }

        public void LightTorch()
        {
            _torchLit = true;
            _light = true;
            _sLight = true;
            Thread.Sleep(300000);
            MessageBox.Show("Your torch went out!");
            _torchLit = false;
            if (_torchLit == false)
            {
                if (Location != "the bedroom" && Location != "the corner by the nightstand")
                {
                    if (Location == "the hallway")
                    {
                        _light = false;
                        _sLight = false;
                    }
                }
                else
                {
                    if (!_switchUp)
                    {
                        _light = false;
                        _sLight = false;
                    }
                }
            }
        }

        public void Bleeding()
        {
            while (_gameRunning)
            {
                if (_bleeding > 0)
                {
                    MessageBox.Show("You weaken from your bleeding..");
                    int damage = _bleeding * 5;
                    TakeDmg(damage);
                }
                Thread.Sleep(30000);
            }
        }

        public void Suffer()
        {
            TakeDmg(_screamCounter + 7);
            Services.ScrollText("You can feel your body weakening from the ghost's power..", 500);
            if (!_dead)
            {
                Services.ScrollText("Select an item to use:");
                foreach (Item item in _player.Bag.GetContents())
                {
                    Services.ScrollText((_player.Bag.GetContents().IndexOf(item) + 1) + " - " + item.Name);
                }

                int input;
                string decision = TakeCmd();

                if (Int32.TryParse(decision, out input))
                {
                    if (_player.Bag.GetContents().Count >= input)
                    {
                        if (_player.Bag.GetContents()[input - 1].Name == "Lighter" && !_ghostParalyzed)
                        {
                            Services.ScrollText("You light your torch..", 850);
                            Services.ScrollText("The ghost start screaming. . . 'NOOOOOOOO'", 500);
                            Services.ScrollText("Get rid of the light! Put it out!", 800);
                            Services.ScrollText("You can see the ghost! It looks like he's paralyzed!", 1500);
                            _ghostParalyzed = true;
                            _screamCounter++;
                            Suffer();
                        }
                        else if (_player.Bag.GetContents()[input - 1].Name == "Torch" && _ghostParalyzed)
                        {
                            Services.ScrollText("You hurl the torch at the paralyzed ghost.", 750);
                            Services.ScrollText("He is instantly engulfed in flames and quickly withers away into nothing.", 1000);
                            Services.ScrollText("The dark presence you have been feeling is no longer there! You're free!", 4000);
                            _player.LevelCompleted = true;
                            _gameRunning = false;
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            _screamCounter++;
                            Suffer();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        _player.Health += _screamCounter + 7;
                        Suffer();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                    _player.Health += _screamCounter + 7;
                    Suffer();
                }
            }
        }

        public void TakeDmg(int dmg)
        {
            _player.Health = _player.Health - dmg;
            if (CheckDeath())
            {
                _dead = true;
            }
        }

        public bool CheckDeath()
        {
            if (_player.Health > 0)
            {
                return false;
            }
            else
            {
                _gameRunning = false;
                return true;
            }
        }

        public void FlipLight()
        {
            _switchUp = !_switchUp;
            if (_switchUp || _torchLit)
            {
                _light = true;
                _sLight = true;
            }
            else
            {
                _light = false;
                _sLight = false;
            }
        }

        public void Scream()
        {
            Services.ScrollText("You scream for help..", 1200);
            Services.ScrollText("After a slight delay, you hear a soft 'sshhh'", 1000);
            Services.ScrollText("I think that voice came from the wall..", 1000);
            _screamCounter++;
        }

        public void Spook()
        {
            Thread.Sleep(1000);
            if (_screamCounter < 5)
            {
                int num = rand.Next(3);
                if (num == 0)
                {
                    PrintGhost();
                }
                else
                {
                    Console.Clear();
                }
            }
            else if (_screamCounter < 10)
            {
                int num = rand.Next(4);
                if (num == 0 || num == 1)
                {
                    PrintGhost();
                }
                else
                {
                    Console.Clear();
                }
            }
            else if (_screamCounter < 15)
            {
                int num = rand.Next(5);
                if (num <= 3)
                {
                    PrintGhost();
                }
                else
                {
                    Console.Clear();
                }
            }
            else
            {
                PrintGhost();
            }
        }

        public void PrintGhost()
        {
            int pic = rand.Next(3);
            switch (pic)
            {
                case 0:
                    PictureOne();
                    break;
                case 1:
                    PictureTwo();
                    break;
                case 2:
                    PictureThree();
                    break;
            }
        }

        public string TakeCmd()
        {
            string yesNo;
            if (_light)
            {
                yesNo = "Yes";
            }
            else
            {
                yesNo = "No";
            }
            Console.Write("H:{0} Lgt:{1}> ", _player.Health, yesNo);
            string cmd = Console.ReadLine();
            return cmd;
        }

        public void PictureOne()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n\n");
            Console.WriteLine("                                            ________________   _");
            Console.WriteLine("                                           |                    |");
            Console.WriteLine("                                          |                      |");
            Console.WriteLine("                                         |        |||   |||       |");
            Console.WriteLine("                                         |      |||  | |  |||     |");
            Console.WriteLine("                                         |     ||    | |    ||    |");
            Console.WriteLine("                                        |||||||||    | |    |||||||");
            Console.WriteLine("                                        |||||||||    | |    |||||||");
            Console.WriteLine("                                        ||||||  |   || ||   |  ||||");
            Console.WriteLine("                                         |      ||  |   |  ||    |");
            Console.WriteLine("                                         |       ||||   ||||     |");
            Console.WriteLine("                                         |                   /   |");
            Console.WriteLine("                                         |       \\_________/    |");
            Console.WriteLine("                                         \\                      /");
            Console.WriteLine("                                          \\              ___   /");
            Console.WriteLine("");
            Thread.Sleep(400);
            Console.Clear();
        }

        public void PictureTwo()
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("                                                                                  _______");
            Console.WriteLine("                                                                                 / _____ |");
            Console.WriteLine("                                                                                 ||     \\/");
            Console.WriteLine("                                                                                / |");
            Console.WriteLine("                                                                               /  \\ ");
            Console.WriteLine("                                                                              /    \\ ");
            Console.WriteLine("                                                                             ( || ||\\ ");
            Console.WriteLine("                                                                             (   *   \\");
            Console.WriteLine("                                                                             (___O____|");
            Thread.Sleep(400);
            Console.Clear();
        }

        public void PictureThree()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("     \\\\\\          ///");
            Console.WriteLine("      \\\\\\        ///");
            Console.WriteLine("       \\\\\\      ///");
            Console.WriteLine();
            Console.WriteLine("             |     )");
            Console.WriteLine("                  /");
            Console.WriteLine("                 /");
            Console.WriteLine("         _______/");
            Thread.Sleep(400);
            Console.Clear();
        }
        
    }
}
