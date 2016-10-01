using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; } //not sure if we want these or if we'll do combat some other way?
        public int Defense { get; set; } //not sure if we want these or if we'll do combat some other way?
        public Bag Bag { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public void Atk(Player target) //note: if a new 'npc' class is made for enemies, the parameter type of this method needs to change from 'Player' to 'npc'
        {
            //Could do combat like this but it only will work for a 'basic attack'
            //If we want different kinds of attacks we should probably make a class for creating 'moves' or 'abilities' or w/e
            //There are lots of ways to go about combat though - might find a better way once we start implementing
        }

        //Used to add one item into the player's bag
        public void Add(Item item)
        {

        }

        //Used to add multiple items into the player's bag
        public void Add(List<Item> items)
        {

        }
    }
}
