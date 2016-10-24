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
                new Card("Angry Chicken")
                {
                    Text = "Enrage: +5 attack",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Enrage = true
                },
                new Card("Enchanted Raven")
                {
                    Cost = 2,
                    Health = 2,
                    Attack = 2
                },
                new Card("Fiery Bat")
                {
                    Text = "Deathrattle: Deal 1 damage to a random enemy",
                    Cost = 1,
                    Health = 1,
                    Attack = 2,
                    DeathRattle = true
                },
                new Card("Hungry Crab")
                {
                    Text = "Battlecry: Destroy a murloc and gain +2/+2",
                    Cost = 1,
                    Health = 2,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Pit Snake")
                {
                    Text = "Destroy any minion damage by this minion",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Poisoned = true
                },
                new Card("Stonetusk Boar")
                {
                    Text = "Charge",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Charge = true
                },
                new Card("Timber Wolf")
                {
                    Text = "All beasts have +1 attack",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Aura = true
                },
                new Card("Young Dragonhawk")
                {
                    Text = "Windfury",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Windfury = true
                },
                new Card("Bloodfen Raptor")
                {
                    Cost = 2,
                    Health = 2,
                    Attack = 3
                },
                new Card("Dire Wolf Alpha")
                {
                    Text = "Adjacent minions have +1 attack",
                    Cost = 2,
                    Attack = 2,
                    Health = 2,
                    Adjacent = true
                },
                new Card("Duskboar")
                {
                    Cost = 2,
                    Health = 1,
                    Attack = 4
                },
                new Card("Huge Toad")
                {
                    Text = "Deathrattle: Deal 1 damage to a random enemy",
                    Cost = 2,
                    Health = 2,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Jeweled Scarab")
                {
                    Text = "Battle: Discover a 3-cost card",
                    Cost = 2,
                    Health = 1,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Kindly Grandmother")
                {
                    Text = "Summon a 3/2 Big Bad Wolf",
                    Cost = 2,
                    Health = 1,
                    Attack = 1,
                    DeathRattle = true
                },
                new Card("King's Elekk")
                {
                    Text = "Battlecry: Reveal a minion in each deck. If yours costs more, draw it",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("River Crocolisk")
                {
                    Cost = 2,
                    Health = 3,
                    Attack = 2
                },
                new Card("Scavenging Hyena")
                {
                    Text = "Whenever a friendly beast dies, gain +2/+1",
                    Cost = 2,
                    Health = 2,
                    Attack = 2
                },
                new Card("Addled Grizzly")
                {
                    Text = "After you summon a minion, give it +1/+1",
                    Cost = 3,
                    Health = 2,
                    Attack = 2
                },
                new Card("Carrion Grub")
                {
                    Cost = 3,
                    Health = 5,
                    Attack = 2
                },
                new Card("Desert Camel")
                {
                    Text = "Battlecry: Put a 1-cost minion from each deck into the battlefield",
                    Cost = 3,
                    Health = 4,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Dreadscale")
                {
                    Text = "At the end of your turn, deal 1 damage to all other minions",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    EndOfTurn = true
                },
                new Card("Emperor Cobra")
                {
                    Text = "Destroy any minion damaged by this minion",
                    Cost = 3,
                    Health = 3,
                    Attack = 2,
                    Poisoned = true
                },
                new Card("Fierce Monkey")
                {
                    Text = "Taunt",
                    Cost = 3,
                    Health = 4,
                    Attack = 3,
                    Taunt = true
                },
                new Card("Jungle Panther")
                {
                    Text = "Stealth",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    Stealth = true
                },
                new Card("Ironbeak Owl")
                {
                    Text = "Battlecry: Silence a minion",
                    Cost = 3,
                    Health = 1,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Ironfur Grizzly")
                {
                    Text = "Taunt",
                    Cost = 3,
                    Health = 3,
                    Attack = 3,
                    Taunt = true
                },
                new Card("King Mukla")
                {
                    Text = "Battlecry: Give your opponent 2 bananas",
                    Cost = 3,
                    Health = 5,
                    Attack = 5,
                    Battlecry = true
                },
                new Card("Mounted Raptor")
                {
                    Text = "Deathrattle: Summon a random 1-cost minion",
                    Cost = 3,
                    Health = 2,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Pantry Spider")
                {
                    Text = "Battlecry: Summon a 1/3 spider",
                    Cost = 3,
                    Health = 3,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Silverback Patriarch")
                {
                    Text = "Taunt",
                    Cost = 3,
                    Health = 4,
                    Attack = 1,
                    Taunt = true
                },
                new Card("Armored Warhorse")
                {
                    Text = "Battlecry: Reveal a minion in each deck. If yours costs more, gain charge",
                    Cost = 4,
                    Health = 3,
                    Attack = 5,
                    Battlecry = true
                },
                new Card("Core Rager")
                {
                    Text = "If your han is empty, gain +3/+3",
                    Cost = 4,
                    Health = 4,
                    Attack = 4,
                    Battlecry = true
                },
                new Card("Infested Wolf")
                {
                    Text = "Deathrattle: Summon two 1/1 spiders",
                    Cost = 4,
                    Health = 3,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Jungle Moonkin")
                {
                    Text = "Both players have +2 spell damage",
                    Cost = 4,
                    Health = 4,
                    Attack = 4
                },
                new Card("Oasis Snapjaw")
                {
                    Cost = 4,
                    Health = 7,
                    Attack = 2
                },
                new Card("Tomb Spider")
                {
                    Text = "Battlecry: Discover a beast",
                    Cost = 4,
                    Health = 3,
                    Attack = 3,
                    Battlecry = true
                },
                new Card("Stampeding Kodo")
                {
                    Text = "Battlecry: Destroy a random enemy minion with 2 or less attack",
                    Cost = 5,
                    Attack = 3,
                    Health = 5,
                    Battlecry = true
                },
                new Card("Starving Buzzard")
                {
                    Text = "Whenever you summon a beast, draw a card",
                    Cost = 5,
                    Health = 2,
                    Attack = 3
                },
                new Card("Strangulthorn Tiger")
                {
                    Text = "Stealth",
                    Cost = 5,
                    Attack = 5,
                    Health = 5,
                    Stealth = true
                },
                new Card("Tundra Rhyno")
                {
                    Text = "Your beasts have charge",
                    Cost = 5,
                    Health = 5,
                    Attack = 2,
                    Aura = true
                },
                new Card("Mukla, Tyrant of the Vale")
                {
                    Text = "Add 2 bananas to your hand",
                    Cost = 6,
                    Health = 3,
                    Attack = 3,
                    Battlecry = true
                },
                new Card("Princess Huhuran")
                {
                    Text = "Battlecry: Trigger a friendly minion's deathrattle effect",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("The Beast")
                {
                    Text = "Deathrattle: Summon a 3/3 Finkle Einhorn for your opponent",
                    Cost = 6,
                    Health = 7,
                    Attack = 9,
                    DeathRattle = true
                },
                new Card("Savannah Highmane")
                {
                    Text = "Deathrattle: Summon two 2/2 hyenas",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    DeathRattle = true
                },
                new Card("Captured Jormungar")
                {
                    Cost = 7,
                    Health = 9,
                    Attack = 5
                },
                new Card("Core Hound")
                {
                    Cost = 7,
                    Health = 5,
                    Attack = 9
                },
                new Card("Grotesque Dragonhawk")
                {
                    Text = "Windfury",
                    Cost = 7,
                    Health = 5,
                    Attack = 5,
                    Windfury = true
                },
                new Card("Giant Sandworm")
                {
                    Text = "Whenever this attacks and kills a minion, it may attack again",
                    Cost = 8,
                    Health = 8,
                    Attack = 8
                },
                new Card("King Krush")
                {
                    Text = "Charge",
                    Cost = 9,
                    Health = 8,
                    Attack = 8,
                    Charge = true
                },
        };

            return cards;
        }
    }
}
