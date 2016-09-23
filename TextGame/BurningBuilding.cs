using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class BurningBuilding : Scenario
    {
        //Since this is a child class of Scenario, it will automatically have Location as a property - we don't have to write it again here

        public BurningBuilding()
        {

        }

        public void Start()
        {
            //The intro text could go here instead of in the game class?
            RoomOne();
        }

        public void RoomOne()
        {
            Console.WriteLine(" ");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("What would you like to do?");
            Console.WriteLine(" ");
            Console.WriteLine("1 - Open door");
            Console.WriteLine("2 - Examine desk");
            Console.WriteLine("3 - Examine chair");
            Console.WriteLine("4 - Examine lamp");
            Console.WriteLine("5 - Examine rug");

            int decisionOne;
            string roomOne = Console.ReadLine();

            if (Int32.TryParse(roomOne, out decisionOne))
            {
                switch (decisionOne)
                {
                    case 1:
                        Console.WriteLine("The door handle is too hot. You burn yourself.");
                        RoomOne();
                        break;
                    case 2:
                        Console.WriteLine("");
                        break;
                    case 3:
                        Console.WriteLine("");
                        break;
                    case 4:
                        Console.WriteLine("");
                        break;
                    case 5:
                        Console.WriteLine("");
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        RoomOne();
                        break;
                }
            }
        }
    }
}
