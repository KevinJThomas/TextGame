﻿using System;
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
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Enrage = true
                },
                new Card("Enchanted Raven")
                {
                    Type = "Beast",
                    Cost = 2,
                    Health = 2,
                    Attack = 2
                },
                new Card("Fiery Bat")
                {
                    Text = "Deathrattle: Deal 1 damage to a random enemy",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 2,
                    DeathRattle = true
                },
                new Card("Hungry Crab")
                {
                    Text = "Battlecry: Destroy a murloc and gain +2/+2",
                    Type = "Beast",
                    Cost = 1,
                    Health = 2,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Pit Snake")
                {
                    Text = "Destroy any minion damage by this minion",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Poisoned = true
                },
                new Card("Stonetusk Boar")
                {
                    Text = "Charge",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Charge = true
                },
                new Card("Timber Wolf")
                {
                    Text = "All beasts have +1 attack",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Aura = true
                },
                new Card("Young Dragonhawk")
                {
                    Text = "Windfury",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Windfury = true
                },
                new Card("Bloodfen Raptor")
                {
                    Type = "Beast",
                    Cost = 2,
                    Health = 2,
                    Attack = 3
                },
                new Card("Dire Wolf Alpha")
                {
                    Text = "Adjacent minions have +1 attack",
                    Type = "Beast",
                    Cost = 2,
                    Attack = 2,
                    Health = 2,
                    Adjacent = true
                },
                new Card("Duskboar")
                {
                    Type = "Beast",
                    Cost = 2,
                    Health = 1,
                    Attack = 4
                },
                new Card("Huge Toad")
                {
                    Text = "Deathrattle: Deal 1 damage to a random enemy",
                    Type = "Beast",
                    Cost = 2,
                    Health = 2,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Jeweled Scarab")
                {
                    Text = "Battlecry: Discover a 3-cost card",
                    Type = "Beast",
                    Cost = 2,
                    Health = 1,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Kindly Grandmother")
                {
                    Text = "Summon a 3/2 Big Bad Wolf",
                    Type = "Beast",
                    Cost = 2,
                    Health = 1,
                    Attack = 1,
                    DeathRattle = true
                },
                new Card("King's Elekk")
                {
                    Text = "Battlecry: Reveal a minion in each deck. If yours costs more, draw it",
                    Type = "Beast",
                    Cost = 2,
                    Health = 2,
                    Attack = 3,
                    Battlecry = true
                },
                new Card("River Crocolisk")
                {
                    Type = "Beast",
                    Cost = 2,
                    Health = 3,
                    Attack = 2
                },
                new Card("Scavenging Hyena")
                {
                    Type = "Beast",
                    Text = "Whenever a friendly beast dies, gain +2/+1",
                    Cost = 2,
                    Health = 2,
                    Attack = 2
                },
                new Card("Addled Grizzly")
                {
                    Type = "Beast",
                    Text = "After you summon a minion, give it +1/+1",
                    Cost = 3,
                    Health = 2,
                    Attack = 2
                },
                new Card("Carrion Grub")
                {
                    Type = "Beast",
                    Cost = 3,
                    Health = 5,
                    Attack = 2
                },
                new Card("Desert Camel")
                {
                    Text = "Battlecry: Put a 1-cost minion from each deck into the battlefield",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Dreadscale")
                {
                    Text = "At the end of your turn, deal 1 damage to all other minions",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    EndOfTurn = true
                },
                new Card("Emperor Cobra")
                {
                    Text = "Destroy any minion damaged by this minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 2,
                    Poisoned = true
                },
                new Card("Fierce Monkey")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 3,
                    Taunt = true
                },
                new Card("Jungle Panther")
                {
                    Text = "Stealth",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    Stealth = true
                },
                new Card("Ironbeak Owl")
                {
                    Text = "Battlecry: Silence a minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 1,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Ironfur Grizzly")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 3,
                    Taunt = true
                },
                new Card("King Mukla")
                {
                    Text = "Battlecry: Give your opponent 2 bananas",
                    Type = "Beast",
                    Cost = 3,
                    Health = 5,
                    Attack = 5,
                    Battlecry = true
                },
                new Card("Mounted Raptor")
                {
                    Text = "Deathrattle: Summon a random 1-cost minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Pantry Spider")
                {
                    Text = "Battlecry: Summon a 1/3 spider",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Silverback Patriarch")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 1,
                    Taunt = true
                },
                new Card("Armored Warhorse")
                {
                    Text = "Battlecry: Reveal a minion in each deck. If yours costs more, gain charge",
                    Type = "Beast",
                    Cost = 4,
                    Health = 3,
                    Attack = 5,
                    Battlecry = true
                },
                new Card("Core Rager")
                {
                    Text = "If your han is empty, gain +3/+3",
                    Type = "Beast",
                    Cost = 4,
                    Health = 4,
                    Attack = 4,
                    Battlecry = true
                },
                new Card("Infested Wolf")
                {
                    Text = "Deathrattle: Summon two 1/1 spiders",
                    Type = "Beast",
                    Cost = 4,
                    Health = 3,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Jungle Moonkin")
                {
                    Text = "Both players have +2 spell damage",
                    Type = "Beast",
                    Cost = 4,
                    Health = 4,
                    Attack = 4
                },
                new Card("Oasis Snapjaw")
                {
                    Type = "Beast",
                    Cost = 4,
                    Health = 7,
                    Attack = 2
                },
                new Card("Tomb Spider")
                {
                    Text = "Battlecry: Discover a beast",
                    Type = "Beast",
                    Cost = 4,
                    Health = 3,
                    Attack = 3,
                    Battlecry = true
                },
                new Card("Stampeding Kodo")
                {
                    Text = "Battlecry: Destroy a random enemy minion with 2 or less attack",
                    Type = "Beast",
                    Cost = 5,
                    Attack = 3,
                    Health = 5,
                    Battlecry = true
                },
                new Card("Starving Buzzard")
                {
                    Text = "Whenever you summon a beast, draw a card",
                    Type = "Beast",
                    Cost = 5,
                    Health = 2,
                    Attack = 3
                },
                new Card("Strangulthorn Tiger")
                {
                    Text = "Stealth",
                    Type = "Beast",
                    Cost = 5,
                    Attack = 5,
                    Health = 5,
                    Stealth = true
                },
                new Card("Tundra Rhyno")
                {
                    Text = "Your beasts have charge",
                    Type = "Beast",
                    Cost = 5,
                    Health = 5,
                    Attack = 2,
                    Aura = true
                },
                new Card("Mukla, Tyrant of the Vale")
                {
                    Text = "Add 2 bananas to your hand",
                    Type = "Beast",
                    Cost = 6,
                    Health = 3,
                    Attack = 3,
                    Battlecry = true
                },
                new Card("Princess Huhuran")
                {
                    Text = "Battlecry: Trigger a friendly minion's deathrattle effect",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("The Beast")
                {
                    Text = "Deathrattle: Summon a 3/3 Finkle Einhorn for your opponent",
                    Type = "Beast",
                    Cost = 6,
                    Health = 7,
                    Attack = 9,
                    DeathRattle = true
                },
                new Card("Savannah Highmane")
                {
                    Text = "Deathrattle: Summon two 2/2 hyenas",
                    Type = "Beast",
                    Cost = 1,
                    Health = 1,
                    Attack = 1,
                    DeathRattle = true
                },
                new Card("Captured Jormungar")
                {
                    Type = "Beast",
                    Cost = 7,
                    Health = 9,
                    Attack = 5
                },
                new Card("Core Hound")
                {
                    Type = "Beast",
                    Cost = 7,
                    Health = 5,
                    Attack = 9
                },
                new Card("Grotesque Dragonhawk")
                {
                    Text = "Windfury",
                    Type = "Beast",
                    Cost = 7,
                    Health = 5,
                    Attack = 5,
                    Windfury = true
                },
                new Card("Giant Sandworm")
                {
                    Text = "Whenever this attacks and kills a minion, it may attack again",
                    Type = "Beast",
                    Cost = 8,
                    Health = 8,
                    Attack = 8
                },
                new Card("King Krush")
                {
                    Text = "Charge",
                    Type = "Beast",
                    Cost = 9,
                    Health = 8,
                    Attack = 8,
                    Charge = true
                },
        };

            return cards;
        }

        public static List<Card> ThreeDrops()
        {
            List<Card> cards = new List<Card>()
            {
                new Card("Addled Grizzly")
                {
                    Type = "Beast",
                    Text = "After you summon a minion, give it +1/+1",
                    Cost = 3,
                    Health = 2,
                    Attack = 2
                },
                new Card("Carrion Grub")
                {
                    Type = "Beast",
                    Cost = 3,
                    Health = 5,
                    Attack = 2
                },
                new Card("Desert Camel")
                {
                    Text = "Battlecry: Put a 1-cost minion from each deck into the battlefield",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Dreadscale")
                {
                    Text = "At the end of your turn, deal 1 damage to all other minions",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    EndOfTurn = true
                },
                new Card("Emperor Cobra")
                {
                    Text = "Destroy any minion damaged by this minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 2,
                    Poisoned = true
                },
                new Card("Fierce Monkey")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 3,
                    Taunt = true
                },
                new Card("Jungle Panther")
                {
                    Text = "Stealth",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 4,
                    Stealth = true
                },
                new Card("Ironbeak Owl")
                {
                    Text = "Battlecry: Silence a minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 1,
                    Attack = 2,
                    Battlecry = true
                },
                new Card("Ironfur Grizzly")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 3,
                    Taunt = true
                },
                new Card("King Mukla")
                {
                    Text = "Battlecry: Give your opponent 2 bananas",
                    Type = "Beast",
                    Cost = 3,
                    Health = 5,
                    Attack = 5,
                    Battlecry = true
                },
                new Card("Mounted Raptor")
                {
                    Text = "Deathrattle: Summon a random 1-cost minion",
                    Type = "Beast",
                    Cost = 3,
                    Health = 2,
                    Attack = 3,
                    DeathRattle = true
                },
                new Card("Pantry Spider")
                {
                    Text = "Battlecry: Summon a 1/3 spider",
                    Type = "Beast",
                    Cost = 3,
                    Health = 3,
                    Attack = 1,
                    Battlecry = true
                },
                new Card("Silverback Patriarch")
                {
                    Text = "Taunt",
                    Type = "Beast",
                    Cost = 3,
                    Health = 4,
                    Attack = 1,
                    Taunt = true
                }
            };
            return cards;
        }
    }
}
