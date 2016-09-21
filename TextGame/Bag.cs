using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //A bag object will be stored in the player object
    class Bag
    {
        public int MaxSize { get; set; } //Max capacity of the bag
        public int CurrentSize { get; set; } //Holds how many items are actually in the bag

        List<Item> items = new List<Item>(); //Holds all of the contents of the bag

        //I don't think anything needs to be in the constructor
        public Bag()
        {

        }

        public void PrintBag()
        {
            //Prints out the contents of the bag - this would probably be called whenever the user types in a certain cmd such as 'bag', 'help', etc.
        }
    }
}
