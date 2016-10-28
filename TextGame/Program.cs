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
            //Game myGame = new Game(8);
            //myGame.Intro();

            Player kevin = new Player("Kevin");
            CardGame game = new CardGame(kevin);
            game.Start();
        }
    }
}
