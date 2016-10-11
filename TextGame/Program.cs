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
            //Game myGame = new Game(7);
            //myGame.Intro();

            Player player = new Player("Kevin");
            War war = new War(player);
            war.PlayTurn();
        }
    }
}
