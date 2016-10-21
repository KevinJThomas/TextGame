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

        public bool Battlecry { get; set; } = false;
        public bool DeathRattle { get; set; } = false;
        public bool Adjacent { get; set; } = false;
        public bool Aura { get; set; } = false;
        public bool Taunt { get; set; } = false;
        public bool Stealth { get; set; } = false;
        public bool Spell { get; set; } = false;
        public bool Charge { get; set; } = false;
        public bool Windfury { get; set; } = false;
        public bool Poisoned { get; set; } = false;

        public bool Sleeping { get; set; } = true;

        public Card(string name)
        {
            Name = name;
        }
    }
}
