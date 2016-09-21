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

        Player player; //Since it is single player this object will always be the person playing
        //Possibly will either have to make player objects for npc's to battle or make a new class 'npc' or 'minion' etc. 

        //By default all games will start on level one unless a different level is specified
        public Game(int currentLevel = 1)
        {
            CurrentLevel = currentLevel;
        }

        public void Intro()
        {
            //Could start out by asking the player for his name
            //Probably a switch statement that will call the correct next method depending on what level the game is
            //ie. if CurrentLevel == 1, PlayLevelOne() will be called
        }

        public void PlayLevelOne()
        {
            Scenario scenario = new Scenario();
            scenario.BurningBuilding();

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

        
    }
}
