using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //These items will be stored in the player's bag
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Power { get; set; } //This could be used dynamically ie. if it's a potion it will display how much it heals but if it's a weapon upgrade
                                       //it will display how much attack it adds.. depending on how we implement everything this might change into multiple things
        public bool Friendly { get; set; } //True if it's an item like a potion, false if it's something like a bomb - idk if we'll ever need to use this or not

        public Item(string name)
        {
            Name = name;
        }

        public void UseItem()
        {
            //Probably a giant switch statement that looks at the item's name and then decided what it is does?
        }
    }
}
