using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Game
    {
        public int CurrentLevel { get; set; } //Keeps track of what level the game is on

        Player player; //Since it is single player this object will always be the person playing
        //Possibly will either have to make player objects for npc's to battle or make a new class 'npc' or 'minion' etc. 

        //By default all games will start on level one unless a different level is specified
        public Game(int currentLevel = 1)
        {
            CurrentLevel = currentLevel;
        }

        public void Intro()
        {
            PlayLevelOne();

            //Could start out by asking the player for his name
            //Probably a switch statement that will call the correct next method depending on what level the game is
            //ie. if CurrentLevel == 1, PlayLevelOne() will be called
        }

        public void PlayLevelOne()
        {
            //All of this might be in the wrong place. Not entirely sure. 
            string introText1 = "You awaken lying on the floor of an unfamiliar room. Smoke billows all around\nyou and alarms can be heard blaring nearby.";
            string introText2 = "You stand up...";
            string introText3 = "You quickly take in your surroundings. The only entrance to the room is a single metal door. Within the room there is a desk, a folding chair, a tall lamp, and a rug on the floor.";
            foreach (var character in introText1)
            {
                Console.Write(character);                   //Scrolling text because why not. I think it looks nicer than abruptly writing entire lines or
                System.Threading.Thread.Sleep(50);          //paragraphs all at once.

            }
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(" ");                     //Why doesn't this print a blank line? Same with lines 42, 50, and 53. 

            foreach (var character in introText2)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(50);
            }
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(" ");

            foreach (var character in introText3)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(50);

            }

        firstRoom:
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
            Int32.TryParse(roomOne, out decisionOne);

            switch (decisionOne)
            {
                case 1:
                    Console.WriteLine("The door handle is too hot. You burn yourself.");
                    goto firstRoom;
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


            }
            Scenario scenario = new Scenario();
            scenario.BurningBuilding();

            //Maybe scenario.BurningBuilding() can return a boolean for whether the player beat the scenario?
            //If they passed, call Advance(). If not, GameOver()
        }

        //Advances the user to the next level
        public void Advance()
        {
            //Could reset certain varialbes like health etc.
            //Add/remove items from bag depending on what they are allowed for the upcoming scenario - some items could carry over?
            CurrentLevel++;
            //Call the method to start the next scenario.. basically the same thing as Intro() except doesn't ask the user for their name
        }

        public void GameOver()
        {
            //Tells the user that they have lost
            //Could possibly display information like how far they/etc?
            Program.PlayAgain();
        }

        
    }
}
