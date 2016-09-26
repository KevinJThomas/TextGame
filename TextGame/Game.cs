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

        Player player = new Player(""); //Since it is single player this object will always be the person playing
        //Possibly will either have to make player objects for npc's to battle or make a new class 'npc' or 'minion' etc. 

        //By default all games will start on level one unless a different level is specified
        public Game(int currentLevel = 1)
        {
            CurrentLevel = currentLevel;
        }

        public void Intro()
        {
            string text1 = "Welcome to 'w/e the game name is'!";
            string text2 = "You are about to start out on a string of wild and bizarre adventures.";
            string text3 = "But first... What is your name?";
            string text5 = "I won't keep you here any longer... Your first adventure starts now!";
            
            ScrollText(text1, 1500);
            ScrollText(text2, 1500);
            ScrollText(text3);

            Console.Write("> ");
            string name = Console.ReadLine();

            if (name == "Kristin" || name == "kristin")
            {
                name = "kristen";
            }

            player.Name = name;

            string text4 = ("Great! Nice to meet you " + player.Name + ".");

            ScrollText(text4, 1500);
            ScrollText(text5, 3500);

            Console.Clear();

            System.Threading.Thread.Sleep(1000);

            StartLevel();
        }

        public void PlayLevelOne()
        {
            //We might want to put all of this into Scenario.BurningBuilding since it is specific to that scenario. It'll run either way for testing so it's
            //not that important atm. I think this method (PlayLevelOne()) is going to end up being just a few lines of code.
            string introText1 = "You awaken lying on the floor of an unfamiliar room. Smoke billows all around\nyou and alarms can be heard blaring nearby.";
            string introText2 = "You stand up...";
            string introText3 = "You quickly take in your surroundings. The only entrance to the room is a single metal door. Within the room there is a desk, a folding chair, a tall lamp, and a rug on the floor.";

            //Anytime where you're repeating code a method usually will save you time/space
            ScrollText(introText1, 2000);              
            ScrollText(introText2, 2000);
            ScrollText(introText3, 2000);

            //If we're going to be making multiple scenarios each one should be its own subclass of scenario.cs otherwise scenario.cs will get HUGE cause it will contain
            //the majority of the code for the entire game..if you don't know about parent/child classes I think kudvenkat has a vid on it
            BurningBuilding burningBuilding = new BurningBuilding(player); 
            burningBuilding.Start();

        }

        public void PlayLevelTwo()
        {
            Assassination assassination = new Assassination(player);
            assassination.Start();
        }

        //Advances the user to the next level
        public void Advance()
        {
            //Could reset certain varialbes like health etc.
            //Add/remove items from bag depending on what they are allowed for the upcoming scenario - some items could carry over?
            CurrentLevel++;
            StartLevel();
        }

        public void GameOver()
        {
            string text1 = "Game Over";
            string text2 = "You made it through " + (CurrentLevel - 1) + " levels.";

            ScrollText(text1, 1000);
            ScrollText(text2, 1000);

            Program.PlayAgain();
        }

        public void StartLevel()
        {
            switch (CurrentLevel)
            {
                case 1:
                    PlayLevelOne();
                    break;
                case 2:
                    PlayLevelTwo();
                    break;
                default:
                    Console.WriteLine("ERROR: Game.StartLevel(): Game.CurrentLevel OutOfBounds = {0}", CurrentLevel);
                    Console.WriteLine("The game will now crash. . .Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }
        
        public static void ScrollText(string text, int delay = 0) //delay set to a default of 0 so if you don't want a delay you don't have to specify anything
        {
            Random rand = new Random();     //Randomizing the thread sleep so typing looks more natural
            foreach (char character in text)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(rand.Next(20, 75));
            }
            System.Threading.Thread.Sleep(delay);
            Console.WriteLine("");        //Since before this line is called the console is ending with a console.write, it is still on the above line
                                          //Console.WriteLine will bring it down to a new line, but if you want an empty line try Console.WriteLine("\n")
        }
    }
}
