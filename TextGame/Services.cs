using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                System.Threading.Thread.Sleep(rand.Next(20, 75));
            }
            System.Threading.Thread.Sleep(delay);
            Console.WriteLine("");
        }

        public static void PlayAgain()
        {
            string text1 = "Would you like to play again? (y/n)";
            string text2 = "Thanks for playing!\nPress any key to exit. . .";
            string text3 = "Invalid input. Please try again.\n";

            string answer;

            ScrollText(text1);

            answer = Console.ReadLine();

            if (answer == "yes" || answer == "y")
            {
                Game newGame = new Game();
                newGame.Intro();
            }
            else if (answer == "no" || answer == "n")
            {
                ScrollText(text2);
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                ScrollText(text3);
                PlayAgain();
            }
        }
    }
}
