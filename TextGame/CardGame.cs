using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextGame
{
    //TODO
    //1) Add in functionality for specific minions such as taunt/battlecry/etc.
    class CardGame : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        Card[] _allCards = Cards.GetCards();

        List<Card> _playerDeck = new List<Card>();
        List<Card> _enemyDeck = new List<Card>();

        List<Card> _playerCards = new List<Card>();
        List<Card> _enemyCards = new List<Card>();

        List<Card> _playerBoard = new List<Card>();
        List<Card> _enemyBoard = new List<Card>();

        public int PlayerHeroHealth { get; set; } = 30;
        public int EnemyHeroHealth { get; set; } = 30;
        public int PlayerMana { get; set; } = 10; //change back to 0 after testing
        public int PlayerCurrentMana { get; set; }
        public int EnemyMana { get; set; } = 0;
        public int EnemyCurrentMana { get; set; }
        public int MaxMana { get; set; } = 10;

        bool _mulliganStage = true;
        bool _playerTurn;
        bool _tunderCoin;
        bool _gameOver = false;

        public CardGame(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

            AddWebspinnerToDeck(true, 30);
            AddWebspinnerToDeck(false, 30);

            _playerCards.Add( //testing only
                new Card("Core Rager")
                {
                    Text = "Battlecry: If your hand is empty, gain +3/+3",
                    Type = "Beast",
                    BaseCost = 4,
                    Cost = 4,
                    BaseHealth = 4,
                    Health = 4,
                    BaseAttack = 4,
                    Attack = 4,
                    Battlecry = true
                });
        }

        public static void PlayMusic()
        {
            Assembly assembly;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream
                ("TextGame.audio.night.wav"));
            sp.Stream.Position = 0;
            sp.PlayLooping();
        }

        public void Start()
        {
            //Services.ScrollText("You are sitting across the table from a tough-looking dwarf.", 1200);
            //Services.ScrollText(". . .", 500);
            //Services.ScrollText("Oh hello there! My name is Tunder the Great.", 800);
            //Services.ScrollText("I hope you're ready to lose! I've never lost a card game in my life.", 800);
            //Services.ScrollText("\nIn case you don't know, here are the rules of the game:", 600);
            //Services.FastScrollText("1) You draw one card at the beginning of each turn");
            //Services.FastScrollText("2) You have a limited amount on mana each turn, which will increase by 1 each turn");
            //Services.FastScrollText("3) You'll start with 1 mana on turn 1, and once you reach 10 mana it will stop increasing");
            //Services.FastScrollText("4) Each player has 30 health, and the first player to bring the opponent to 0 health wins");
            //Services.FastScrollText("5) The attacking player chooses each minion's target", 6000);
            //Services.ScrollText("\n. . .", 500);
            //Services.ScrollText("Tunder takes two 30 card decks out of his leather sack and hands one to you.", 1100);
            //Services.ScrollText("He takes a coin out of his pocket, flips it in the air, and catches it on the back of his hand.", 1100);

            if (GoingFirst())
            {
                _playerTurn = true;
                DrawCard(true, 3);
                DrawCard(false, 4);
                _enemyCards.Add(new Card("The Coin")
                {
                    Text = "Gain 1 mana crystal this turn only",
                    Cost = 0,
                    Spell = true
                });
                _tunderCoin = true;
                _mulliganStage = false;
                Services.ScrollText("Well, it looks like you get to go first this time, laddy.", 1100);
                StartTurn();
            }
            else
            {
                _playerTurn = false;
                DrawCard(false, 3);
                DrawCard(true, 4);
                _playerCards.Add(new Card("The Coin")
                {
                    Text = "Gain 1 mana crystal this turn only",
                    Cost = 0,
                    Spell = true
                });
                _mulliganStage = false;
                Services.ScrollText("It looks like I'll be going first today, laddy.", 1100);
                EnemyTurn();
            }
            
        }

        public void StartTurn()
        {
            _playerTurn = true;

            if (PlayerMana < MaxMana)
                PlayerMana++;

            PlayerCurrentMana = PlayerMana; //refill empty mana crystals for the turn

            foreach (Card card in _playerBoard)
                card.Sleeping = false;

            DrawCard();
            PrintBoard();
            Listen();
        }

        public void EnemyTurn()
        {
            _playerTurn = false;

            if (EnemyMana < MaxMana)
                EnemyMana++;

            EnemyCurrentMana = EnemyMana; //refill empty mana crystals for the turn

            foreach (Card card in _enemyBoard)
                card.Sleeping = false;

            DrawCard(false);
            Services.ScrollText("ENEMY PLAYING TURN...", 500); //testing only
            EnemyMakeAttacks();
            EnemySpendMana();
            Services.ScrollText("Tunder ends his turn", 600);
            StartTurn();
        }

        public void Listen()
        {
            if (!_gameOver)
            {
                PrintOptions();
                Console.Write("> ");
                
                string input = Console.ReadLine();
                
                switch (input.ToLower())
                {
                    case "attack":
                        if (PlayerHasAvailableMinions())
                        {
                            Attacking();
                        }
                        else
                        {
                            Services.ScrollText("You don't have any minions available to attack with!", 500);
                            PrintBoard();
                            Listen();
                        }
                        break;
                    case "hand":
                        PlayHand();
                        break;
                    case "board":
                        PrintBoard();
                        Listen();
                        break;
                    case "read":
                        ReadCards();
                        break;
                    case "count":
                        PrintCount();
                        Listen();
                        break;
                    case "end turn":
                        EnemyTurn();
                        break;
                    default:
                        Services.ScrollText("Invalid input. Try again.", 500);
                        Listen();
                        break;
                }
            }
        }

        public void PrintBoard()
        {
            if (_playerBoard.Count != 0 || _enemyBoard.Count != 0)
            {
                Console.WriteLine();
                Services.FastScrollText(" - OPPONENT - (" + EnemyHeroHealth + ")");
                if (_enemyBoard.Count > 0)
                {
                    foreach (Card card in _enemyBoard)
                    {
                        Services.FastScrollText(card.Name + "(" + card.Attack + "/" + card.Health + ")  ", 0, true);
                    }
                }
                else
                {
                    Console.Write("(empty)");
                }
                

                Console.WriteLine("\n\n");

                if (_playerBoard.Count > 0)
                {
                    foreach (Card card in _playerBoard)
                    {
                        Services.FastScrollText(card.Name + "(" + card.Attack + "/" + card.Health + ")  ", 0, true);
                    }
                }
                else
                {
                    Console.Write("(empty)");
                }

                Console.WriteLine();
                Services.FastScrollText(" -   YOU   - (" + PlayerHeroHealth + ")\n");
            }
            else
            {
                Services.ScrollText("The board is empty\n");
            }
        }

        public void PrintCount()
        {
            Services.FastScrollText("Your hand: " + _playerCards.Count + "\nEnemy hand: " + _enemyCards.Count + "\n");
        }

        public void PrintHand()
        {
            Services.FastScrollText("\n(You can type 'back' at any point to return to the main menu of commands)");
            if (_playerCards.Count > 0)
            {
                Services.FastScrollText("\nYour hand:             Mana: (" + PlayerCurrentMana + "/" + PlayerMana + ")");
                PrintList(_playerCards, true);
                Console.WriteLine();
            }
            else
            {
                Services.ScrollText("Your hand is empty");
            }
        }

        public List<Card> PrintAvailableMinions()
        {
            Services.FastScrollText("\n(You can type 'back' at any point to return to the main menu of commands)");
            List<Card> tempList = new List<Card>();
            
            foreach (Card card in _playerBoard)
            {
                if (!card.Sleeping)
                    tempList.Add(card);
            }

            PrintList(tempList, true);

            return tempList;
        }

        public void PrintOptions()
        {
            Services.FastScrollText("You have " + _playerCards.Count + " cards. Mana: (" + PlayerCurrentMana + "/" + PlayerMana + ")\n");
            Services.FastScrollText("attack: view and select available minions to attack with");
            Services.FastScrollText("hand: view and play cards from your hand");
            Services.FastScrollText("board: print out the board");
            Services.FastScrollText("read: view and select cards in your hand or on the board to read their text");
            Services.FastScrollText("count: print out a count of each player's hand");
            Services.FastScrollText("end turn: end your turn\n");
        }

        public int PrintList(List<Card> list, bool noCmd = false, bool includeHero = false)
        {
            if (!noCmd)
            {
                foreach (Card card in list)
                    Services.FastScrollText((list.IndexOf(card) + 1).ToString() + ") " + card.Name + " (" + card.Attack + "/" + card.Health + ") *" + card.Cost + "*");
                if (includeHero)
                    Services.FastScrollText((list.Count + 1).ToString() + ") Enemy Hero");

                int cmd;
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (Int32.TryParse(input, out cmd))
                {
                    if (!includeHero)
                    {
                        if (cmd > 0 && cmd <= list.Count)
                        {
                            return cmd;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        if (cmd > 0 && cmd <= list.Count + 1)
                        {
                            return cmd;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else if (input == "back")
                {
                    return -1;
                }
                else if (input == "exit")
                {
                    return -2;
                }
                else if (input == "board")
                {
                    return -3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                foreach (Card card in list)
                    Services.FastScrollText((list.IndexOf(card) + 1).ToString() + ") " + card.Name + " (" + card.Attack + "/" + card.Health + ") *" + card.Cost + "*");
                return 0;
            }
            
        }

        public void ReadCards()
        {
            if (!_gameOver)
            {
                List<Card> readableCards = new List<Card>();
                List<string> alreadyListed = new List<string>();

                foreach (Card card in _playerCards)
                {
                    if (!alreadyListed.Contains(card.Name))
                    {
                        readableCards.Add(card);
                        alreadyListed.Add(card.Name);
                    }
                        
                }
                    

                foreach (Card card in _playerBoard)
                {
                    if (!alreadyListed.Contains(card.Name))
                    {
                        readableCards.Add(card);
                        alreadyListed.Add(card.Name);
                    }
                }

                foreach (Card card in _enemyBoard)
                {
                    if (!alreadyListed.Contains(card.Name))
                    {
                        readableCards.Add(card);
                        alreadyListed.Add(card.Name);
                    }
                }

                Services.FastScrollText("(You can type 'back' at any point to navigate back to the main menu)");
                int cmd = PrintList(readableCards);
                if (cmd > 0)
                {
                    if (readableCards[cmd - 1].Text != "")
                        Services.ScrollText("\n" + readableCards[cmd - 1].Text + "\n");
                    else
                        Services.ScrollText("-no text-");
                    ReadCards();
                }
                else if (cmd == -1 || cmd == -2)
                {
                    PrintBoard();
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.", 500);
                    ReadCards();
                }
            }
        }

        public void PlayHand()
        {
            if (!_gameOver)
            {
                PrintHand();
                Console.Write("> ");
                int cmd;
                string input = Console.ReadLine();

                if (Int32.TryParse(input, out cmd))
                {
                    if (cmd <= _playerCards.Count)
                    {
                        PlayCard(_playerCards[cmd - 1]);
                        PlayHand();
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.", 500);
                        PlayHand();
                    }
                }
                else if (input.ToLower() == "back" || input.ToLower() == "exit")
                {
                    PrintBoard();
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.", 500);
                    PlayHand();
                }
            }
        }

        public void Attacking()
        {
            if (!_gameOver)
            {
                if (PlayerHasAvailableMinions())
                {
                    List<Card> availableMinions = PrintAvailableMinions();
                    Console.Write("> ");
                    int cmd;
                    string input = Console.ReadLine();

                    if (Int32.TryParse(input, out cmd))
                    {
                        if (cmd <= availableMinions.Count)
                        {
                            ChooseTarget(availableMinions[cmd - 1]);
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try again.", 500);
                            Attacking();
                        }
                    }
                    else if (input.ToLower() == "back" || input.ToLower() == "exit")
                    {
                        PrintBoard();
                        Listen();
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.", 500);
                        Attacking();
                    }
                }
                else
                {
                    Services.ScrollText("You don't have any minions available to attack with!", 500);
                    PrintBoard();
                    Listen();
                }
            }
        }

        public void ChooseTarget(Card card)
        {
            Services.ScrollText("\nSelect a target to attack with your " + card.Name + " (" + card.Attack + "/" + card.Health + ")");

            int cmd = PrintList(_enemyBoard, false, true);
            if (cmd > 0)
            {
                if (cmd != _enemyBoard.Count + 1)
                {
                    Swing(card, _enemyBoard[cmd - 1]);
                    Attacking();
                }
                else
                {
                    Swing(card, null);
                    Attacking();
                }
            }
            else if (cmd == -1)
            {
                PrintBoard();
                Attacking();
            }
            else if (cmd == -2)
            {
                PrintBoard();
                Listen();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.", 500);
                ChooseTarget(card);
            }
        }

        public void Swing(Card attacker, Card defender)
        {
            if (defender == null)
            {
                if (_playerTurn)
                {
                    Services.ScrollText(attacker.Name + " hits Tunder for " + attacker.Attack + " damage", 600);
                    DamageHero(attacker.Attack);
                }
                else
                {
                    Services.ScrollText(attacker.Name + " hits you for " + attacker.Attack + " damage", 600);
                    DamageHero(attacker.Attack, false);
                } 
            }
            else
            {
                defender.Health -= attacker.Attack;
                attacker.Health -= defender.Attack;
                Services.ScrollText(attacker.Name + " attacks " + defender.Name, 600);
                DeathCheck(new List<Card> { attacker, defender });
            }

            attacker.Sleeping = true;
        }

        public void DamageHero(int damage, bool enemyHero = true)
        {
            if (enemyHero)
            {
                EnemyHeroHealth -= damage;
                if (EnemyHeroHealth <= 0)
                {
                    _player.LevelCompleted = true;
                    _gameOver = true;
                }
            }
            else
            {
                PlayerHeroHealth -= damage;
                if (PlayerHeroHealth <= 0)
                    _gameOver = true;
            }
        }

        public void DeathCheck(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                if (card.Health <= 0)
                {
                    Services.ScrollText(card.Name + " dies!");
                    Die(card);
                }
            }
        }

        public void Die(Card card)
        {
            if (card.DeathRattle)
                Deathrattle(card);

            if (_playerBoard.Contains(card))
                _playerBoard.Remove(card);
            else if (_enemyBoard.Contains(card))
                _enemyBoard.Remove(card);
        }

        public bool PlayerHasAvailableMinions()
        {
            if (_playerBoard.Count > 0)
            {
                foreach (Card card in _playerBoard)
                {
                    if (!card.Sleeping)
                        return true;
                }
                return false;
            }
            return false;
        }

        public bool GoingFirst()
        {
            Random rand = new Random();
            int first = rand.Next(2);

            if (first == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddWebspinnerToDeck(bool player, int num)
        {
            if (player)
            {
                for (int i = 0; i < num; i++)
                {
                    _playerDeck.Add(new Card("Webspinner")
                    {
                        Text = "Deathrattle: Add a random beast to your hand",
                        BaseCost = 1,
                        Cost = 1,
                        BaseAttack = 1,
                        Attack = 1,
                        BaseHealth = 1,
                        Health = 1,
                        DeathRattle = true
                    });
                }
            }
            else if (!player)
            {
                for (int i = 0; i < num; i++)
                {
                    _enemyDeck.Add(new Card("Webspinner")
                    {
                        Text = "Deathrattle: Add a random beast to your hand",
                        BaseCost = 1,
                        Cost = 1,
                        BaseAttack = 1,
                        Attack = 1,
                        BaseHealth = 1,
                        Health = 1,
                        DeathRattle = true
                    });
                }
            }
        }

        public void PlayCard(Card card)
        {
            if (!card.Spell)
            {
                if (_playerTurn)
                {
                    if (PlayerCurrentMana >= card.Cost)
                    {
                        PlayerCurrentMana -= card.Cost;

                        Services.ScrollText("You play " + card.Name);
                        
                        _playerBoard.Add(card);
                        _playerCards.Remove(card);

                        if (card.Battlecry)
                            Battlecry(card);
                    }
                    else
                    {
                        Services.ScrollText("You don't have enough mana");
                    }
                }
                else
                {
                    if (EnemyCurrentMana >= card.Cost)
                    {
                        EnemyCurrentMana -= card.Cost;
                        _enemyBoard.Add(card);
                        _enemyCards.Remove(card);

                        

                        Services.ScrollText("Your opponent plays " + card.Name);
                    }
                }

                if (card.Charge)
                    card.Sleeping = false;
            }
            else
            {
                switch (card.Name)
                {
                    case "The Coin":
                        if (_playerTurn)
                        {
                            _playerCards.Remove(card);
                            if (PlayerCurrentMana < 10)
                                PlayerCurrentMana++;
                        }
                        else
                        {
                            _enemyCards.Remove(card);
                            if (EnemyCurrentMana < 10)
                                EnemyCurrentMana++;
                        }
                        break;
                    case "Banana":
                        //TODO: playing bananas
                        break;
                }
            }
        }

        public void Battlecry(Card card)
        {
            switch (card.Name)
            {
                case "Hungry Crab": //TODO: AI?
                    List<Card> murlocTargets = new List<Card>();
                    bool isMurloc = false;

                    foreach (Card minion in _enemyBoard)
                    {
                        if (minion.Type == "Murloc")
                        {
                            isMurloc = true;
                            murlocTargets.Add(minion);
                        }
                    }
                    if (isMurloc)
                    {
                        Services.ScrollText("Select a murloc to eat:");

                        int cmd = PrintList(murlocTargets);
                        if (cmd > 0)
                        {
                            _enemyBoard.Remove(murlocTargets[cmd - 1]);
                            card.Attack += 2;
                            card.Health += 2;
                            Services.ScrollText("Your hungry crab eats " + murlocTargets[cmd - 1].Name);
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try again.", 500);
                            Battlecry(card);
                        }
                    }
                    break;
                case "Jeweled Scarab": //TODO: AI?
                    Discover(Cards.ThreeDrops());
                    break;
                case "King's Elekk":
                    Services.ScrollText("A minion is revealed from both decks:\nYou: Webspinner (1)\nTunder: Webspinner (1)\n\nA card is not drawn\n", 500);
                    break;
                case "Desert Camel":
                    if (_playerDeck.Count > 0)
                    {
                        _playerBoard.Add(_playerDeck[_playerDeck.Count - 1]);
                        _playerDeck.Remove(_playerDeck[_playerDeck.Count - 1]);
                        Services.ScrollText("Desert camel pulls a webspinner out of your deck", 400);
                    }
                    if (_enemyDeck.Count > 0)
                    {
                        _enemyBoard.Add(_enemyDeck[_enemyDeck.Count - 1]);
                        _enemyDeck.Remove(_enemyDeck[_enemyDeck.Count - 1]);
                        Services.ScrollText("Desert camel pulls a webspinner out of Tunder's deck", 400);
                    }
                    break;
                case "Ironbeak Owl": //TODO: AI?
                    List<Card> allMinions = ListAllMinions();
                    if (allMinions.Count > 0)
                        Silence(allMinions);
                    break;
                case "King Mukla":
                    if (_playerTurn)
                        GiveBananas(false, 2);
                    else
                        GiveBananas(true, 2);
                    break;
                case "Pantry Spider":
                    if (_playerTurn)
                    {
                        _playerBoard.Add(new Card("Spider")
                        {
                            Cost = 3,
                            BaseHealth = 3,
                            Health = 3,
                            BaseAttack = 1,
                            Attack = 1
                        });
                    }
                    else
                    {
                        _enemyBoard.Add(new Card("Spider")
                        {
                            Cost = 3,
                            BaseHealth = 3,
                            Health = 3,
                            BaseAttack = 1,
                            Attack = 1
                        });
                    }
                    Services.ScrollText("It spawns a spider", 500);
                    break;
                case "Armored Warhorse":
                    Services.ScrollText("A minion is revealed from both decks:\nYou: Webspinner (1)\nTunder: Webspinner (1)\n\nCharge is not given\n", 500);
                    break;
                case "Core Rager":
                    if (_playerTurn)
                    {
                        if (_playerCards.Count == 0)
                        {
                            card.Attack += 3;
                            card.Health += 3;
                            Services.ScrollText("It gains +3/+3");
                        }
                    }
                    else
                    {
                        if (_enemyCards.Count == 0)
                        {
                            card.Attack += 3;
                            card.Health += 3;
                            Services.ScrollText("It gains +3/+3");
                        }
                    }
                    break;
                case "Tomb Spider": //TODO: AI?
                    List<Card> beasts = new List<Card>();

                    foreach (Card beast in _allCards)
                        beasts.Add(beast);

                    Discover(beasts);
                    break;
                case "Stampeding Kodo":
                    if (_playerTurn)
                    {
                        List<Card> kodoTargets = CanKodo();
                        if (kodoTargets.Count > 0)
                        {
                            int cmd = PrintList(kodoTargets);
                            if (cmd > 0)
                            {
                                Die(kodoTargets[cmd - 1]);
                            }
                            else
                            {
                                Battlecry(card);
                            }
                        }
                    }
                    else
                    {
                        List<Card> kodoTargets = CanKodo();
                        if (kodoTargets.Count > 0)
                        {
                            Random rand = new Random();
                            Die(kodoTargets[rand.Next(kodoTargets.Count)]);
                        }
                    }
                    break;
                case "Mukla, Tyrant of the Vale":
                    if (_playerTurn)
                        GiveBananas(true, 2);
                    else
                        GiveBananas(false, 2);
                    break;
                case "Princess Huhuran":
                    if (_playerTurn)
                    {
                        List<Card> deathrattles = ListDeathrattles();
                        int cmd = PrintList(deathrattles);
                        if (cmd > 0)
                        {
                            Deathrattle(deathrattles[cmd - 1]);
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try again.");
                            Battlecry(card);
                        }
                    }
                    else
                    {
                        //TODO
                    }
                    break;
            }
        }

        public void Deathrattle(Card card)
        {
            //TODO
            switch (card.Name)
            {
                case "Webspinner":
                    Random rand = new Random();
                    Card[] tempArr = (Card[])_allCards.Clone();

                    Card randomBeast = tempArr[rand.Next(tempArr.Length)];

                    if (_playerBoard.Contains(card))
                    {
                        _playerCards.Add(randomBeast);
                        Services.ScrollText("Webspinner gives you a " + randomBeast.Name);
                    }
                    else if (_enemyBoard.Contains(card))
                    {
                        _enemyCards.Add(randomBeast);
                    }
                    break;
            }
        }

        public List<Card> ListDeathrattles()
        {
            List<Card> deathrattles = new List<Card>();

            foreach (Card card in _playerBoard)
            {
                if (card.DeathRattle)
                    deathrattles.Add(card);
            }

            return deathrattles;
        }

        public List<Card> CanKodo()
        {
            List<Card> kodoTargets = new List<Card>();
            if (_playerTurn)
            {
                foreach (Card card in _enemyBoard)
                {
                    if (card.Attack <= 2)
                        kodoTargets.Add(card);
                }
            }
            else
            {
                foreach (Card card in _playerBoard)
                {
                    if (card.Attack <= 2)
                        kodoTargets.Add(card);
                }
            }
            return kodoTargets;
        }

        public void GiveBananas(bool toPlayer, int amount = 1)
        {
            if (toPlayer)
            {
                for (int i = 0; i < amount; i++)
                {
                    _playerCards.Add(new Card("Banana")
                    {
                        Text = "Give a minion +1/+1",
                        Cost = 1,
                        Spell = true
                    });
                }
                Services.ScrollText("You receive " + amount + " banana(s)", 400);
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    _enemyCards.Add(new Card("Banana")
                    {
                        Text = "Give a minion +1/+1",
                        Cost = 1,
                        Spell = true
                    });
                }
                Services.ScrollText("Tunder receives " + amount + " banana(s)", 400);
            }
        }

        public List<Card> ListAllMinions()
        {
            List<Card> allMinions = new List<Card>();

            foreach (Card card in _enemyBoard)
                allMinions.Add(card);

            foreach (Card card in _playerBoard)
                allMinions.Add(card);

            return allMinions;
        }

        public void Silence(List<Card> targets)
        {
            Services.ScrollText("\nSelect a minion to silence:");
            int cmd = PrintList(targets);

            if (cmd > 0)
            {
                targets[cmd - 1].Charge = false;
                targets[cmd - 1].Adjacent = false;
                targets[cmd - 1].Aura = false;
                targets[cmd - 1].DeathRattle = false;
                targets[cmd - 1].EndOfTurn = false;
                targets[cmd - 1].Enrage = false;
                targets[cmd - 1].Poisoned = false;
                targets[cmd - 1].Taunt = false;
                targets[cmd - 1].Windfury = false;
                targets[cmd - 1].Attack = targets[cmd - 1].BaseAttack;

                if (targets[cmd - 1].BaseHealth < targets[cmd - 1].Health)
                    targets[cmd - 1].Health = targets[cmd - 1].BaseHealth;

                Services.ScrollText(targets[cmd - 1].Name + " is silenced!");
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.", 500);
                Silence(targets);
            }
        }

        public void Discover(List<Card> cardList)
        {
            List<Card> discoverOptions = new List<Card>();

            Random rand = new Random();

            int firstOption = rand.Next(cardList.Count);
            discoverOptions.Add(cardList[firstOption]);
            cardList.Remove(cardList[firstOption]);

            int secondOption = rand.Next(cardList.Count);
            discoverOptions.Add(cardList[secondOption]);
            cardList.Remove(cardList[secondOption]);

            int thirdOption = rand.Next(cardList.Count);
            discoverOptions.Add(cardList[thirdOption]);
            cardList.Remove(cardList[thirdOption]);

            ChooseDiscover(discoverOptions);
        }

        public void ChooseDiscover(List<Card> options)
        {
            Services.FastScrollText("(You can type 'board' at any point to view the board while discovering)");
            int cmd = PrintList(options);
            if (cmd > 0)
            {
                _playerCards.Add(options[cmd - 1]);
                Services.ScrollText("You choose " + options[cmd - 1].Name + " and put it into your hand", 600);
            }
            else if (cmd == -3)
            {
                PrintBoard();
                ChooseDiscover(options);
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.", 500);
                ChooseDiscover(options);
            }
        }

        public void DrawCard(bool player = true, int amount = 1)
        {
            Random rand = new Random();

            if (player)
            {
                for (int i = 0; i < amount; i++)
                {
                    int num = rand.Next(_playerDeck.Count);

                    if (!_mulliganStage)
                        Services.ScrollText("You draw a " + _playerDeck[num].Name);

                    _playerCards.Add(_playerDeck[num]);
                    _playerDeck.RemoveAt(num);
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    int num = rand.Next(_enemyDeck.Count);

                    if (!_mulliganStage)
                        Services.ScrollText("The opponent draws a " + _enemyDeck[num].Name);

                    _enemyCards.Add(_enemyDeck[num]);
                    _enemyDeck.RemoveAt(num);
                }
            }
        }

        public void EnemySpendMana()
        {
            if (_tunderCoin)
            {
                _enemyCards.RemoveAt(4);
                Services.ScrollText("Tunder plays the coin", 600);
                EnemyCurrentMana++;
                _tunderCoin = false;
            }

            EnemyPlayCard();

            int minCost = 10;

            foreach (Card card in _enemyCards)
            {
                if (card.Cost < minCost)
                    minCost = card.Cost;
            }

            if (EnemyCurrentMana >= minCost)
                EnemySpendMana();
        }

        public void EnemyPlayCard()
        {
            Random rand = new Random();

            Card card = _enemyCards[rand.Next(_enemyCards.Count)];

            if (card.Cost <= EnemyCurrentMana)
            {
                Services.ScrollText("Tunder plays " + card.Name, 600);
                EnemyCurrentMana -= card.Cost;
                _enemyBoard.Add(card);
                _enemyCards.Remove(card);

                if (card.Charge)
                {
                    DamageHero(card.Attack, false);
                    Services.ScrollText("He charges it at you and you take " + card.Attack + " damage", 600);
                }
                    
            }
        }

        public void EnemyMakeAttacks()
        {
            Random rand = new Random();

            Card[] tempAry = new Card[_enemyBoard.Count];
            _enemyBoard.CopyTo(tempAry);

            foreach (Card card in tempAry)
            {
                if (!card.Sleeping)
                {
                    Card targetCard;
                    int target = rand.Next(_playerBoard.Count + 1);

                    if (target == _playerBoard.Count)
                        targetCard = null;
                    else
                        targetCard = _playerBoard[target];

                    Swing(card, targetCard);
                }
            }
        }
    }
}
