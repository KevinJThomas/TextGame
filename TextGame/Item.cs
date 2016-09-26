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

        //Probably a better way to do this.. Could not even have this method at all and just put logic for items in each scenario
        public void UseItem()
        {
            switch(Name)
            {
                case "Gun":
                    Console.WriteLine("Using Gun");
                    break;
                case "Knife":
                    Console.WriteLine("Using Knife");
                    break;
                case "Twine":
                    Console.WriteLine("Using Twine");
                    break;
                default:
                    Console.WriteLine("ERROR: Item.UseItem(). . .Do not recognize '{0}' as an item", Name);
                    break;
            }
        }
    }
}
