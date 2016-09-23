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
            int num = 1;

            Console.WriteLine("Your bag contains:");

            foreach(Item item in items)
            {
                Console.WriteLine("{0}) {1}", num, item.Name);
                num++;
            }
        }
    }
}
