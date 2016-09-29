using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Riddles : Scenario
    {
        //Riddles
        string _riddle1 = "I have a heart that never beats, I have a home but I never sleep. I can take a mans house and build anothers, "
            + "And I love to play games with my many brothers. I am a king among fools. Who am I?";
        string _riddle2 = "A clock chimes 5 times in 4 seconds. How many times will it chime in 10 seconds?";
        string _riddle3 = "Here on earth it is true, yesterday is always before today; but there is a place where yesterday always follows today. Where?";
        string _riddle4 = "What word starting with BR, that with the addition of the letter E, becomes another word that sounds the same as the first?";
        string _riddle5 = "I am weightless, but you can see me. Put me in a bucket, and I'll make it lighter. What am I?";
        string _riddle6 = "Pronounced as one letter, And written with three, Two letters there are, And two only in me." +
            " I'm double, I'm single, I'm black, blue, and gray, I'm read from both ends, And the same either way. What am I?";
        string _riddle7 = "I never was, am always to be, No one ever saw me, nor ever will," +
            " And yet I am the confidence of all To live and breathe on this terrestrial ball. What am I?";

        //Rooms
        string[] theCircularRoom = new string[] { "Talk to the man at the door", "Talk to the man with a bright orange scarf", "Talk to the man picking grass",
            "Talk to the man holding a large and vibrantly blue snake", "Talk to the man eating an eggplant", "Talk to the man making a fire",
            "Talk to the man growing sunflowers" };
        string theCircularRoomDesc = "You're in a sizeable, ceilingless room surrounded by old men. They look like they could be wizards!";
        string[] theCircularRoomTarget = new string[] { };

        //People
        string[] doorMan = new string[] { "Why am I here?", "I would like to leave", "Goodbye" };

        bool trusted = false;

        public Riddles(Player player) : base(player)
        {
            _player = player;
        }

        public void Start()
        {
            Services.ScrollText("You are in the middle of a large, perfectly circular room.", 1000);
            Services.ScrollText("You are standing on an aged marble floor, and there is no ceiling above you; it is only sky.", 500);
            Services.ScrollText("Looking around, you see 2 large wooden doors as your only way out.", 500);
            Services.ScrollText("There is an old man with a long white beard sitting on a decorated chair in front of the door.", 500);
            Services.ScrollText("There are 6 benches around the circle, and there is an old man sitting on each one of these as well.", 500);
            Services.ScrollText("These men look similar to the first man you saw, except their beards aren't quite as long.", 500);
            Services.ScrollText("Hmm.. What is going on?!", 500);

            TheCircularRoom();
        }

        public void TheCircularRoom()
        {
            Location = "the circular room";
            EnterArea(theCircularRoom, theCircularRoomDesc, theCircularRoomTarget);

            int input;
            string decision = Console.ReadLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("You walk over to the man sitting in front of the door", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("..Hello young man! What do you seek?");
                            DoorMan();
                        }
                        else
                        {
                            Services.ScrollText(_riddle1);
                            Password(1);
                        }
                        break;
                    case 2:
                        Services.ScrollText("You walk over to the man with a bright orange scarf", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("What is the password?");
                            if (ProvidePass("11"))
                            {
                                Services.ScrollText(_riddle3);
                                Password(3);
                            }
                            else
                            {
                                Deny();
                            }
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    case 3:
                        Services.ScrollText("You walk over to the man picking grass", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("What is the password?");
                            if (ProvidePass("braking"))
                            {
                                Services.ScrollText(_riddle5);
                                Password(5);
                            }
                            else
                            {
                                Deny();
                            }
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    case 4:
                        Services.ScrollText("You walk over to the man holding a large and vibrantly blue snake", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("What is the password?");
                            if (ProvidePass("hole"))
                            {
                                Services.ScrollText(_riddle6);
                                Password(6);
                            }
                            else
                            {
                                Deny();
                            }
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    case 5:
                        Services.ScrollText("You walk over to the man eating an eggplant", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("What is the password?");
                            if (ProvidePass("eye"))
                            {
                                Services.ScrollText(_riddle7);
                                Password(7);
                            }
                            else
                            {
                                Deny();
                            }
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    case 6:
                        Services.ScrollText("You walk over to the man making a fire", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText(_riddle2);
                            Password(2);
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    case 7:
                        Services.ScrollText("You walk over to the man growing sunflowers", 750);
                        if (trusted == true)
                        {
                            Services.ScrollText("What is the password?");
                            if (ProvidePass("dictionary"))
                            {
                                Services.ScrollText(_riddle4);
                                Password(4);
                            }
                            else
                            {
                                Deny();
                            }
                        }
                        else
                        {
                            Deny();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        TheCircularRoom();
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
                    TheCircularRoom();
                }
                else
                {
                    TheCircularRoom();
                }

            }
        }

        public void DoorMan()
        {
            Console.WriteLine();
            foreach (string option in doorMan)
            {
                Services.ScrollText((Array.IndexOf(doorMan, option) + 1) + " - " + option);
            }

            int input;
            string decision = Console.ReadLine();
            Console.WriteLine();

            if (Int32.TryParse(decision, out input))
            {
                switch (input)
                {
                    case 1:
                        Services.ScrollText("\"" + doorMan[input - 1] + "\"\n", 1000);
                        Services.ScrollText("That is something that you will have to find out for yourself, young one.", 1000);
                        DoorMan();
                        break;
                    case 2:
                        Services.ScrollText("\"" + doorMan[input - 1] + "\"\n", 1000);
                        Services.ScrollText("What is the password?", 1000);
                        if (ProvidePass("tomorrow"))
                        {
                            _player.LevelCompleted = true;
                            Services.ScrollText("Very good! You may leave now. Good luck on your future journies.", 1500);
                        }
                        else
                        {
                            Deny();
                            TheCircularRoom();
                        }
                        break;
                    case 3:
                        Services.ScrollText("\"" + doorMan[input - 1] + "\"\n", 1000);
                        Services.ScrollText("Farewell..", 500);
                        TheCircularRoom();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        DoorMan();
                        break;
                }
            }
            else
            {
                ExamineCommand(decision);
                DoorMan();
            }
        }

        public void Password(int num)
        {
            string answer;
            switch (num)
            {
                case 1:
                    answer = Console.ReadLine();
                    if (answer.ToLower().Contains("king of hearts"))
                    {
                        trusted = true;
                        Services.ScrollText("Very good! I'm impressed.", 750);
                        Services.ScrollText("*The old man yells something in Latin*", 500);
                        Services.ScrollText("All of the men knod their heads in unison before returning to what they were doing.", 1000);
                        TheCircularRoom();
                    }
                    else if (answer.ToLower() == "exit")
                    {
                        TheCircularRoom();
                    }
                    else
                    {
                        Services.ScrollText("No no.. that's not right. Try again.");
                        Services.ScrollText("(Or type 'exit' to stop guessing)");
                        Console.WriteLine();
                        Password(num);
                    }
                    break;
                case 2:
                    Guessing(num, "11");
                    break;
                case 3:
                    Guessing(num, "dictionary");
                    break;
                case 4:
                    Guessing(num, "braking");
                    break;
                case 5:
                    Guessing(num, "hole");
                    break;
                case 6:
                    Guessing(num, "eye");
                    break;
                case 7:
                    Guessing(num, "tomorrow");
                    break;
            }
        }

        public void Deny()
        {
            Services.ScrollText("The old man ignores you..");
            TheCircularRoom();
        }

        public void Guessing(int num, string pass)
        {
            string answer = Console.ReadLine();
            if (answer.ToLower().Contains(pass))
            {
                Services.ScrollText("You have passed my test. The answer was '" + pass + "'.", 750);
                TheCircularRoom();
            }
            else if (answer.ToLower() == "exit")
            {
                TheCircularRoom();
            }
            else
            {
                Services.ScrollText("Wrong. Keep guessing.");
                Services.ScrollText("(Or type 'exit' to stop guessing)");
                Console.WriteLine();
                Password(num);
            }
        }

        public bool ProvidePass(string pass)
        {
            string answer = Console.ReadLine();
            if (answer.ToLower() == pass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
