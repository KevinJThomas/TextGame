using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextGame
{
    static class Services
    {
        public static void ScrollText(string text, int delay = 0)
        {
            Random rand = new Random();
            foreach (char character in text)
            {
                Console.Write(character);
                Thread.Sleep(rand.Next(10, 25));
            }
            Thread.Sleep(delay);
            Console.WriteLine();
        }

        public static void FastScrollText(string text, int delay = 0, bool OneLine = false)
        {
            Random rand = new Random();
            foreach (char character in text)
            {
                Console.Write(character);
                Thread.Sleep(rand.Next(2));
            }
            Thread.Sleep(delay);

            if (!OneLine)
                Console.WriteLine();
        }

        public static void PlayAgain()
        {
            ScrollText("Would you like to play again? (y/n)");

            string answer = Console.ReadLine();

            if (answer == "yes" || answer == "y")
            {
                Game newGame = new Game();
                newGame.Intro();
            }
            else if (answer == "no" || answer == "n")
            {
                ScrollText("Thanks for playing!\nPress any key to exit. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                ScrollText("Invalid input. Please try again.\n");
                PlayAgain();
            }
        }
    }
}
