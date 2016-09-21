using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //Haven't put much thought into this class.. But I think this is where most of the code should run
    class Scenario
    {
        public string Location { get; set; } //Used to track the player's location within a specific scenario - ie. Room 1/Room 2/Basement/Roof/etc.

        public Scenario()
        {

        }

        public void BurningBuilding()
        {
            //The code for playing through the burning building
        }

        public void AssassinateTarget()
        {
            //The code for playing through another possible scenario down the line
        }
    }
}
