using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //These 2 lines should be all the code that is in the main method
            //A player object will be made in the game, a bag object will be made in the player, and item objects can be inserted and removed from the bag whenever
            Game myGame = new Game();
            myGame.Intro();
        }

        public static void PlayAgain()
        {
            string text1 = "Would you like to play again? (y/n)";
            string text2 = "Thanks for playing!\nPress any key to exit. . .";
            string text3 = "Invalid input. Please try again.\n";

            string answer;

            foreach (var character in text1)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(50);
            }
            Console.Write("\n");
            Console.Write("> ");

            answer = Console.ReadLine();

            if (answer == "yes" || answer == "y")
            {
                Game newGame = new Game();
                newGame.Intro();
            }
            else if (answer == "no" || answer == "n")
            {
                foreach (var character in text2)
                {
                    Console.Write(character);
                    System.Threading.Thread.Sleep(50);
                }
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                foreach (var character in text3)
                {
                    Console.Write(character);
                    System.Threading.Thread.Sleep(50);
                }
                PlayAgain();
            }
        }
    }
}
