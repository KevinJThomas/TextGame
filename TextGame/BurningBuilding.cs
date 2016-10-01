using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class BurningBuilding : Scenario
    {
        Item theChairLeg = new Item("Chair Leg");
        Item rug = new Item("rug");
        string[] roomonetargets = new string[] { "door", "desk", "lamp", "chair" };
        string[] room1 = new string[] { "Open door", "Examine desk", "Examine chair", "Examine lamp", "Examine rug" };

        bool rugPickedUp = false;

        public BurningBuilding(Player player) : base(player)
        {
            _player = player;
        }

        public void Start()
        {
            
            string introText1 = "You awaken lying on the floor of an unfamiliar room. Smoke billows all around\nyou and alarms can be heard blaring nearby.";
            string introText2 = "You stand up...";
            string introText3 = "You quickly take in your surroundings. The only entrance to the room is a single metal door. Within the room there is a desk, a folding chair, a lamp, and a rug on the floor.";

            //Anytime where you're repeating code a method usually will save you time/space
            Services.ScrollText(introText1, 2000);
            Console.WriteLine("\n");
            Services.ScrollText(introText2, 2000);
            Console.WriteLine("\n");
            Services.ScrollText(introText3, 2000);
            RoomOne();

            //If we're going to be making multiple scenarios each one should be its own subclass of scenario.cs otherwise scenario.cs will get HUGE cause it will contain
            //the majority of the code for the entire game..if you don't know about parent/child classes I think kudvenkat has a vid on it
        }

        public void RoomOne()
        {
            Location = "Room One";
            EnterArea(room1, "moo", roomonetargets);

            string text1 = "You grab the door handle, but it is too hot. You burn yourself.";
            string text2 = "\nUpon closer examination you see that the desk has a drawer. There is a paper on the top of the desk.";
            string text5 = "The chair is a simple folding chair made of wood. One of the legs is damaged.";
            string text6 = "Try to break off the leg?\n Yes or No.";
            string text7 = "You stomp on the leg of the chair and break it off.";
            string text8 = "Chair Leg has been added to your bag.";
            int decisionOne;
            string roomOne = Console.ReadLine();

            if (Int32.TryParse(roomOne, out decisionOne))
            {
                switch (decisionOne)
                {
                    case 1:
                        Console.WriteLine("\n");
                        Services.ScrollText(text1, 2000);
                        RoomOne();
                        break;
                    case 2:
                        Services.ScrollText(text2, 2000);
                        DeskChoices();
                        break;                                         
                    case 3:
                        Services.ScrollText(text5, 2000);
                        Services.ScrollText(text6, 2000);
                        string chairLeg = Console.ReadLine().ToUpper();
                        if (chairLeg == "YES" || chairLeg == "Y")
                        {
                            Services.ScrollText(text7, 2000);
                            Services.ScrollText(text8, 2000);
                            _player.Add(theChairLeg);
                        }

                        RoomOne();
                        break;
                    case 4:
                        Console.WriteLine("The lamp is screwed to the wall. There isn't much you can do with it.");
                        RoomOne();
                        break;
                    case 5:
                        if (rugPickedUp == false)
                        {
                            Services.ScrollText("It's a rug. Would you like to pick it up? Yes or No", 2000);
                            string floorRug = Console.ReadLine().ToUpper();
                            if (floorRug == "YES" || floorRug == "Y")
                            {
                                Services.ScrollText("The rug has been added to your bag", 2000);
                                _player.Add(rug);
                                rugPickedUp = true;
                                room1 = new string[] { "Open door", "Examine desk", "Examine chair", "Examine lamp" };
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
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
                ExamineCommand(roomOne);
                if (Item != null && Target != null)
                {
                    if (Item.Name == "rug")
                    {
                        if(Target == "door")
                        {
                            Services.ScrollText("HELLO WORLD");
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                        }
                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        Item = null;
                        Target = null;
                    }
                    RoomOne();
                }
                else
                {
                    RoomOne();
                }
            }                 
        }

        public void DeskChoices()
        {
            string text3 = "There is a number of the paper. 1738993";
            string text4 = "The drawer is empty.";
            string text9 = "\nYou step away from the desk";
            Console.WriteLine("\n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("1 - Examine paper");
            Console.WriteLine("2 - Open drawer");
            Console.WriteLine("3 - Nothing");
            string deskChoice = Console.ReadLine();
            int deskDecision;
            if (Int32.TryParse(deskChoice, out deskDecision))
            {
                switch (deskDecision)
                {
                    case 1:
                        Services.ScrollText(text3, 2000);
                        DeskChoices();
                        break;
                    case 2:
                        Services.ScrollText(text4, 2000);
                        DeskChoices();
                        break;
                    case 3:
                        Services.ScrollText(text9, 2000);
                        RoomOne();
                        break;
                    default:
                        DeskChoices();
                        break;
                }
            }
        }
    }
}
