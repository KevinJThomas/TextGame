using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    class Card
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }

        public bool Battlecry { get; set; }
        public bool DeathRattle { get; set; }
        public bool Adjacent { get; set; }
        public bool Aura { get; set; }
        public bool Taunt { get; set; }
        public bool Stealth { get; set; }
        public bool Spell { get; set; }

        public Card(string name)
        {
            Name = name;

            Text = "";
            Battlecry = false;
            DeathRattle = false;
            Adjacent = false;
            Aura = false;
            Taunt = false;
            Stealth = false;
            Spell = false;
        }
    }
}
