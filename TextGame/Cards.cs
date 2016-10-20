using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    static class Cards
    {
        public static Card[] GetCards()
        {
            Card[] cards = new Card[]
            {
                new Card("Chicken")
                {
                    Cost = 1,
                    Health = 1,
                    Attack = 1
                },
                new Card("Stampeding Kodo")
                {
                    Text = "Battlecry: Destroy a random enemy minion with 2 or less attack",
                    Cost = 5,
                    Attack = 3,
                    Health = 5,
                    Battlecry = true
                },
                new Card("Strangulthorn Tiger")
                {
                    Text = "Stealth",
                    Cost = 5,
                    Attack = 5,
                    Health = 5,
                    Stealth = true
                },
                new Card("Dire Wolf Alpha")
                {
                    Text = "Adjacent minions have +1 attack",
                    Cost = 2,
                    Attack = 2,
                    Health = 2,
                    Adjacent = true
                }
        };

            return cards;
        }
    }
}
