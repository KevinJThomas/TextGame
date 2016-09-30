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
        
        public Bag(int maxSize = 10)
        {
            MaxSize = maxSize;
        }

        public void PrintBag()
        {
            if (items.Count > 0)
            {
                Services.ScrollText("Your bag contains:");

                foreach (Item item in items)
                {
                    Services.ScrollText(items.IndexOf(item) + 1 + ") " + item.Name);
                }
            }
            else
            {
                Services.ScrollText("Your bag is empty");
            }
            
        }

        public void Add(Item item)
        {
            items.Add(item);
        }

        public void Add(List<Item> newItems)
        {
            foreach (Item item in newItems)
            {
                items.Add(item);
            }
        }

        public void Remove(Item item)
        {
            items.Remove(item);
        }

        public List<Item> GetContents()
        {
            return items;
        }
    }
}
