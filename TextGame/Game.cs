using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Game
    {
        //Used for seeing whether the game is won or if the player should simply advance to the next level
        public static int _totalNumberOfLevels = 5; //We will have to manually change this every time we add a new level

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
            
            Services.ScrollText(text1, 1500);
            Services.ScrollText(text2, 1500);
            Services.ScrollText(text3);

            Console.Write("> ");
            string name = Console.ReadLine();

            player.Name = name;

            string text4 = ("Great! Nice to meet you " + player.Name + ".");

            Services.ScrollText(text4, 1500);
            Services.ScrollText(text5, 3500);

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
            Services.ScrollText(introText1, 2000);
            Services.ScrollText(introText2, 2000);
            Services.ScrollText(introText3, 2000);

            //If we're going to be making multiple scenarios each one should be its own subclass of scenario.cs otherwise scenario.cs will get HUGE cause it will contain
            //the majority of the code for the entire game..if you don't know about parent/child classes I think kudvenkat has a vid on it
            BurningBuilding burningBuilding = new BurningBuilding(player); 
            burningBuilding.Start();

            CheckSuccessful();
        }

        public void PlayLevelTwo()
        {
            Assassination assassination = new Assassination(player);
            assassination.Start();

            CheckSuccessful();
        }

        public void PlayLevelThree()
        {
            Riddles riddles = new Riddles(player);
            riddles.Start();

            CheckSuccessful();
        }

        public void PlayLevelFour()
        {
            Bomb bomb = new Bomb(player);
            bomb.Start();

            CheckSuccessful();
        }

        public void PlayLevelFive()
        {
            Maze maze = new Maze(player);
            maze.Start();

            CheckSuccessful();
        }

        //Advances the user to the next level
        public void Advance()
        {
            player.LevelCompleted = false;
            CurrentLevel++;
            Console.WriteLine();
            Services.ScrollText("Congratulations! You successfully completed the level.", 750);
            Services.ScrollText("Advancing to level " + CurrentLevel + ". . .", 3500);
            Console.Clear();
            StartLevel();
        }

        public void GameOver()
        {
            string text1 = "Game Over";
            string text2 = "You made it through " + (CurrentLevel - 1) + " levels.";

            Services.ScrollText(text1, 1000);
            Services.ScrollText(text2, 1000);

            Services.PlayAgain();
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
                case 3:
                    PlayLevelThree();
                    break;
                case 4:
                    PlayLevelFour();
                    break;
                case 5:
                    PlayLevelFive();
                    break;
                default:
                    Console.WriteLine("ERROR: Game.StartLevel(): Game.CurrentLevel OutOfBounds = {0}", CurrentLevel);
                    Console.WriteLine("The game will now crash. . .Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }

        public void CheckSuccessful()
        {
            if (player.LevelCompleted == true)
            {
                if (CurrentLevel == _totalNumberOfLevels)
                {
                    Winner();
                }
                else
                {
                    Advance();
                }
            }
            else
            {
                GameOver();
            }
        }

        public void Winner()
        {
            Services.ScrollText("You win!");
            Services.PlayAgain();
        }
    }
}
