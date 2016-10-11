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
    class Game
    {
        Thread musicThread = new Thread(PlayIntro);

        //Used for seeing whether the game is won or if the player should simply advance to the next level
        public static int _totalNumberOfLevels = 6; //We will have to manually change this every time we add a new level

        string[] passwords = new string[] { "skip2", "skip3", "skip4", "skip5", "skip6" };

        public int CurrentLevel { get; set; } //Keeps track of what level the game is on

        Player player = new Player(""); //Since it is single player this object will always be the person playing
        //Possibly will either have to make player objects for npc's to battle or make a new class 'npc' or 'minion' etc. 

        //By default all games will start on level one unless a different level is specified
        public Game(int currentLevel = 1)
        {
            CurrentLevel = currentLevel;
        }

        public static void PlayIntro()
        {
            Assembly assembly;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream
                ("TextGame.audio.intro.wav"));
            sp.Stream.Position = 0;
            sp.Play();
        }

        public void Intro()
        {            
            Services.ScrollText("Welcome to the Impossible Game!", 1500);
            Services.ScrollText("You are about to start out on a string of wild and bizarre adventures.", 1500);
            Services.ScrollText("But first... What is your name?");

            Console.Write("> ");
            string name = Console.ReadLine();

            int check = CheckForSkip(name);
            
            if (check != 0)
            {
                CurrentLevel = check;
                Services.ScrollText("You have skipped to level " + CurrentLevel + "!", 500);
                Services.ScrollText("But I do still need to know your name..");
                Console.Write("> ");
                name = Console.ReadLine();
            }

            player.Name = name;
            
            Services.ScrollText("Great! Nice to meet you " + player.Name + ".", 1500);
            Services.ScrollText("I won't keep you here any longer... Your first adventure starts now!", 3500);

            Console.Clear();

            Thread.Sleep(1000);

            StartLevel();
        }

        public void PlayLevelOne()
        {
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

        public void PlayLevelSix()
        {
            Haunted haunted = new Haunted(player);
            haunted.Bedroom();

            CheckSuccessful();
        }

        //Advances the user to the next level
        public void Advance()
        {
            player.LevelCompleted = false;
            CurrentLevel++;
            player.Bag.Empty();
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
                case 6:
                    PlayLevelSix();
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

        public int CheckForSkip(string skip)
        {
            bool found = false;
            int index = 0;
            foreach(string pass in passwords)
            {
                if (pass == skip)
                {
                    found = true;
                    index = Array.IndexOf(passwords, pass);
                }
            }
            if (found)
            {
                return index + 2;
            }
            else
            {
                return 0;
            }
        }
    }
}
