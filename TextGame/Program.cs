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
            //Asks the user if they would like to play again
            //If yes, a new game object will be created and they start again at level one - can add checkpoints later on if we get there
            //If no, the program will exit
        }
    }
}
