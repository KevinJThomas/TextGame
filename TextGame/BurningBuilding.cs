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
        Item deskPaper = new Item("Paper");
        string[] roomonetargets = new string[] { "door", "desk", "lamp", "chair" };
        string[] room1 = new string[] { "Open door", "Examine desk", "Examine chair", "Examine lamp", "Examine rug" };
        string[] roomtwotargets = new string[] { "" };
        string[] room2 = new string[] { "" };

        string[] desk1 = new string[] { "Examine paper", "Open drawer", "Nothing" };
        string[] desktargets = new string[] { "" };

        bool rugPickedUp = false;
        bool chairLegpickup = false;
        bool papertaken = false;

        public BurningBuilding(Player player) : base(player)
        {
            _player = player;
        }

        public void Start()
        {
            
            string introText1 = "You awaken lying on the floor of an unfamiliar room. Smoke billows all around\nyou and alarms can be heard blaring nearby.";
            string introText2 = "You stand up...";
            string introText3 = "You quickly take in your surroundings. The only entrance to the room is a single metal door. Within the room there is a desk, a chair, a lamp, and a rug on the floor.";

            
            Services.ScrollText(introText1, 2000);
            Console.WriteLine("\n");
            Services.ScrollText(introText2, 2000);
            Console.WriteLine("\n");
            Services.ScrollText(introText3, 2000);
            RoomOne();


        }

        public void RoomOne()
        {
            Location = "Room One";
            EnterArea(room1, "A small room with some furniture in it.", roomonetargets);

            string text1 = "You grab the door handle, but it is too hot. You burn yourself.";
            string text2 = "\nUpon closer examination you see that the desk has a drawer.";
            string text5 = "It's a simple chair made of wood. One of the legs is damaged.";
            string text6 = "Try to break off the leg?\n Yes or No.";
            string text7 = "You stomp on the leg of the chair and break it off.";
            string text8 = "Chair Leg has been added to your bag.";
            string text10 = "There is a paper on the top of the desk.";
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
                        if (papertaken == false)
                        {
                            Services.ScrollText(text10, 2000);
                        }

                        DeskChoices();
                        break;                                         
                    case 3:
                        if (chairLegpickup == false)
                        {
                            Services.ScrollText(text5, 2000);
                            Services.ScrollText(text6);
                            string chairLeg = Console.ReadLine().ToUpper();
                            if (chairLeg == "YES" || chairLeg == "Y")
                            {
                                Services.ScrollText(text7, 2000);
                                Services.ScrollText(text8, 2000);
                                _player.Add(theChairLeg);
                                chairLegpickup = true;
                            }
                        }
                        else
                        {
                            Services.ScrollText("It's a simple wooden chair. One of the legs has been broken off.");
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
                            Services.ScrollText("It's a rug. Would you like to pick it up? Yes or No");
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
                            Services.ScrollText("You use the rug to shield your hand from the heat as you open the door.");
                            Services.ScrollText("You step through the door into the next room.");
                            RoomTwo();
                        }
                        else if (Target == "lamp")
                        {
                            Services.ScrollText("You shouldn't do that. You're trying to escape, not start more fires.");
                            RoomOne();
                        }
                        else
                        {
                            Services.ScrollText("It's not very effective.", 500);
                            RoomOne();
                        }

                    }
                    else
                    {
                        Services.ScrollText("It's not very effective.", 500);
                        RoomOne();
                    }
                    Item = null;
                    Target = null;
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

            if (papertaken == true)
            {
                desk1 = new string[] { "Open drawer", "Nothing" };
            }
            
            Console.WriteLine("\n");
            EnterObject(desk1, "A desk with a single drawer.", desktargets);

            string deskChoice = Console.ReadLine();
            int deskDecision;
            if (Int32.TryParse(deskChoice, out deskDecision))
            {
                switch (deskDecision)
                {
                    case 1:
                        if (papertaken == false)
                        {
                            Services.ScrollText(text3, 2000);
                            Services.ScrollText("Take the paper? Yes or no.");
                            string takePaper = Console.ReadLine().ToUpper();
                            if (takePaper == "YES" || takePaper == "Y")
                            {
                                _player.Add(deskPaper);
                                Services.ScrollText("Paper has been added to your bag.");
                                papertaken = true;
                            }
                        }
                        else
                        {
                            Services.ScrollText(text4, 2000);
                            DeskChoices();
                        }

                             
                        DeskChoices();
                        break;
                    case 2:
                        if (papertaken == false)
                        {
                            Services.ScrollText(text4, 2000);
                            DeskChoices();
                        }
                        else
                        {
                            Services.ScrollText(text9, 2000);
                            RoomOne();
                        }
                        break;
                    case 3:
                        if (papertaken == false)
                        {
                            Services.ScrollText(text9, 2000);
                            RoomOne();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                        break;
                    default:
                        DeskChoices();
                        break;
                }
            }
        }

        public void RoomTwo()
        {
            Location = "Room Two";
            EnterArea(room2, "A room.", roomtwotargets);

            
            Services.ScrollText("This room contains a ");
        }
    }
}
