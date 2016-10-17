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

            switch (Name)
            {
                case "Chicken":
                    Cost = 1;
                    Attack = 1;
                    Health = 1;
                    break;
                case "Stampeding Kodo":
                    Text = "Battlecry: Destroy a random enemy minion with 2 or less attack";
                    Cost = 5;
                    Attack = 3;
                    Health = 5;
                    Battlecry = true;
                    break;
                case "Strangulthorn Tiger":
                    Text = "Stealth";
                    Cost = 5;
                    Attack = 5;
                    Health = 5;
                    Stealth = true;
                    break;
                case "Dire Wolf Alpha":
                    Text = "Adjacent minions have +1 attack";
                    Cost = 2;
                    Attack = 2;
                    Health = 2;
                    Adjacent = true;
                    break;
            }
        }
    }
}
