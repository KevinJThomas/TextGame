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

            switch (CurrentLevel)
            {
                case 1:
                    PlayLevelOne();
                    break;
                default:
                    Console.WriteLine("ERROR: Game.Intro(): Game.CurrentLevel OutOfBounds = {0}", CurrentLevel);
                    Console.WriteLine("The game will now crash. . .Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }

            //Could start out by asking the player for his name
            //Probably a switch statement that will call the correct next method depending on what level the game is
            //ie. if CurrentLevel == 1, PlayLevelOne() will be called
        }

        public void PlayLevelOne()
        {
            
            BurningBuilding burningBuilding = new BurningBuilding(player); 
            burningBuilding.deskChoice();

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

        //Will have to move this method into scenario class if we move the above intro text
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
