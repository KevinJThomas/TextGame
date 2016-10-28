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
        public string Type { get; set; }

        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }

        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseCost { get; set; }

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
        public bool Enrage { get; set; } = false;
        public bool EndOfTurn { get; set; } = false;
        
        public int Attacks { get; set; } = 0;

        public Card(string name)
        {
            Name = name;
        }
    }
}
