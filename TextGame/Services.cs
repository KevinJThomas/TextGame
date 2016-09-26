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
    }
}
