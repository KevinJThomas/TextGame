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
    //1) Change text in a few 'invalid input's to see what is looping.. probably PrintList().. change tunder health to 10-15 to make testing faster
    class CardGame : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        Random rand = new Random();

        Card[] _allCards = Cards.GetCards();

        List<Card> _playerDeck = new List<Card>();
        List<Card> _enemyDeck = new List<Card>();

        List<Card> _playerCards = new List<Card>();
        List<Card> _enemyCards = new List<Card>();

        List<Card> _playerBoard = new List<Card>();
        List<Card> _enemyBoard = new List<Card>();

        public int PlayerHeroHealth { get; set; } = 30;
        public int EnemyHeroHealth { get; set; } = 30;
        public int PlayerMana { get; set; } = 0;
        public int PlayerCurrentMana { get; set; }
        public int EnemyMana { get; set; } = 0;
        public int EnemyCurrentMana { get; set; }
        public int MaxMana { get; set; } = 10;

        bool _mulliganStage = true;
        bool _playerTurn;
        bool _tunderCoin;
        bool _gameOver = false;

        bool _playerRhyno = false;
        bool _enemyRhyno = false;

        public CardGame(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

            AddWebspinnerToDeck(true, 30);
            AddWebspinnerToDeck(false, 30);

            if (player.DifficultyLevel == 3)
            {
                PlayerHeroHealth = 25;
                EnemyHeroHealth = 40;
                EnemyMana = 1;
            }
            else if (player.DifficultyLevel == 4)
            {
                PlayerHeroHealth = 15;
                EnemyHeroHealth = 50;
                EnemyMana = 4;
            }
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
            //Services.FastScrollText("2) You have a limited amount of mana each turn, which will increase by 1 each turn");
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
                if (card.Windfury)
                    card.Attacks = 2;
                else
                    card.Attacks = 1;

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
                if (card.Windfury)
                    card.Attacks = 2;
                else
                    card.Attacks = 1;

            DrawCard(false);
            EnemyMakeAttacks();
            EnemySpendMana();
            Services.ScrollText("Tunder ends his turn", 600);
            EndOfTurn();
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
                        EndOfTurn(false);
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
                Services.FastScrollText(" - TUNDER - (" + EnemyHeroHealth + ")");
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
                Services.ScrollText("\nThe board is empty\n");
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
                if (card.Attacks > 0)
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
            if (!_gameOver)
            {
                Services.ScrollText("\nSelect a target to attack with your " + card.Name + " (" + card.Attack + "/" + card.Health + ")");

                List<Card> tauntList = new List<Card>();
                foreach (Card target in _enemyBoard)
                {
                    if (target.Taunt)
                        tauntList.Add(target);
                }

                int cmd;

                if (tauntList.Count == 0)
                {
                    cmd = PrintList(_enemyBoard, false, true);
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
                
                }
                else
                {
                    cmd = PrintList(tauntList);
                    if (cmd > 0)
                    {
                        if (cmd != tauntList.Count + 1)
                        {
                            Swing(card, tauntList[cmd - 1]);
                            Attacking();
                        }
                        else
                        {
                            Swing(card, null);
                            Attacking();
                        }
                    }
                }

                if (cmd == -1)
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
                Services.ScrollText(attacker.Name + " attacks " + defender.Name, 600);

                if (!attacker.Poisoned)
                    defender.Health -= attacker.Attack;
                else
                {
                    Die(defender);
                    Services.ScrollText(defender.Name + " dies!", 400);
                }
                    

                if (!defender.Poisoned)
                    attacker.Health -= defender.Attack;
                else
                {
                    Die(attacker);
                    Services.ScrollText(attacker.Name + " dies!", 400);
                }

                DeathCheck(new List<Card> { attacker, defender });
            }

            attacker.Attacks--;
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
            if (card.Aura)
                StopAura(card);

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
                    if (card.Attacks > 0)
                        return true;
                }
                return false;
            }
            return false;
        }

        public bool GoingFirst()
        {
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
            else
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
                        if (card.Aura)
                            Aura(card);
                        if (_playerRhyno)
                        {
                            if (card.Windfury)
                                card.Attacks = 2;
                            else
                                card.Attacks = 1;
                        }
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
                {
                    if (card.Windfury)
                        card.Attacks = 2;
                    else
                        card.Attacks = 1;
                }
                    
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
                        if (_playerTurn)
                        {
                            if (PlayerCurrentMana >= 1)
                            {
                                bool used;
                                BananaTarget(out used);

                                if (used)
                                {
                                    PlayerCurrentMana--;
                                    _playerCards.Remove(card);
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough mana", 400);
                            }
                        }
                        else
                        {
                            if (_enemyBoard.Count > 0)
                            {
                                int minion = rand.Next(_enemyBoard.Count);
                                _enemyBoard[minion].Attack++;
                                _enemyBoard[minion].Health++;
                                _enemyCards.Remove(card);
                            }
                            EnemyCurrentMana--;
                        }
                        break;
                }
            }
        }

        public void BananaTarget(out bool used)
        {
            if (_playerBoard.Count > 0)
            {
                int cmd = PrintList(_playerBoard);

                if (cmd > 0)
                {
                    _playerBoard[cmd - 1].Attack++;
                    _playerBoard[cmd - 1].Health++;
                    used = true;
                }
                else if (cmd == -1 || cmd == -2)
                {
                    used = false;
                }
                else
                {
                    used = false;
                    BananaTarget(out used);
                }
            }
            else
            {
                Services.ScrollText("You have no minions to buff with a banana", 400);
                used = false;
            }
        }

        public void Aura(Card card)
        {
            switch (card.Name)
            {
                case "Timber Wolf":
                    if (_playerBoard.Contains(card))
                    {
                        foreach (Card beast in _playerBoard)
                            if (beast.Type == "Beast" && beast != card)
                                beast.Attack++;
                    }
                    else
                    {
                        foreach (Card enemyBeast in _playerBoard)
                            if (enemyBeast.Type == "Beast" && enemyBeast != card)
                                enemyBeast.Attack++;
                    }
                    break;
                case "Tundra Rhyno":
                    if (_playerBoard.Contains(card))
                        _playerRhyno = true;
                    else
                        _enemyRhyno = true;
                    break;
            }
        }

        public void StopAura(Card card)
        {
            switch (card.Name)
            {
                case "Timber Wolf":
                    if (_playerBoard.Contains(card))
                    {
                        foreach (Card beast in _playerBoard)
                            beast.Attack--;
                    } 
                    else
                    {
                        foreach (Card enemyBeast in _playerBoard)
                            enemyBeast.Attack--;
                    }
                    break;
                case "Tundra Rhyno":
                    if (_playerBoard.Contains(card))
                    {
                        bool noRhyno = true;

                        foreach (Card minion in _playerBoard)
                            if (minion.Name == "Tundra Rhyno")
                                noRhyno = false;

                        if (noRhyno)
                            _playerRhyno = false;
                    }
                    else
                    {
                        bool noRhyno = true;

                        foreach (Card minion in _enemyBoard)
                            if (minion.Name == "Tundra Rhyno")
                                noRhyno = false;

                        if (noRhyno)
                            _enemyRhyno = false;
                    }
                    break;
            }
        }

        public void Battlecry(Card card)
        {
            switch (card.Name)
            {
                case "Hungry Crab":
                    if (_playerTurn)
                    {
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
                    }
                    break;
                case "Jeweled Scarab":
                    if (_playerTurn)
                        Discover(Cards.ThreeDrops());
                    else
                        _enemyCards.Add(Cards.ThreeDrops()[rand.Next(Cards.ThreeDrops().Count)]);
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
                case "Ironbeak Owl":
                    if (_playerTurn)
                    {
                        List<Card> allMinions = ListAllMinions();
                        if (allMinions.Count > 0)
                            Silence(allMinions);
                    }
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
                case "Tomb Spider":
                    if (_playerTurn)
                    {
                        List<Card> beasts = new List<Card>();

                        foreach (Card beast in _allCards)
                            beasts.Add(beast);

                        Discover(beasts);
                    }
                    else
                    {
                        _enemyCards.Add(_allCards[_allCards.Length]);
                    }
                    
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
                            Die(kodoTargets[rand.Next(kodoTargets.Count)]);
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
                        List<Card> deathrattles = new List<Card>();

                        foreach (Card deathrattle in _enemyBoard)
                            if (deathrattle.DeathRattle)
                                deathrattles.Add(deathrattle);

                        if (deathrattles.Count > 0)
                            Deathrattle(deathrattles[rand.Next(deathrattles.Count)]);
                    }
                    break;
            }
        }

        public void Deathrattle(Card card)
        {
            switch (card.Name)
            {
                case "Webspinner":
                    Card[] cards = Cards.GetCards();

                    Card randomBeast = cards[rand.Next(cards.Length)];

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
                case "Fiery Bat":
                    if (_playerBoard.Contains(card))
                    {
                        int target = rand.Next(_enemyBoard.Count + 1);
                        
                        if (target != _enemyBoard.Count)
                        {
                            _enemyBoard[target].Health -= 1;
                            Services.ScrollText("Fiery bat's deathrattle deals 1 damage to " + _enemyBoard[target].Name, 400);
                            DeathCheck(new List<Card> { _enemyBoard[target] });
                        }
                        else
                        {
                            Services.ScrollText("Fiery bat's deathrattle deals 1 damage to Tunder", 400);
                            DamageHero(1);
                        }
                    }
                    else
                    {
                        int target = rand.Next(_playerBoard.Count + 1);

                        if (target != _playerBoard.Count)
                        {
                            _playerBoard[target].Health -= 1;
                            Services.ScrollText("Fiery bat's deathrattle deals 1 damage to " + _playerBoard[target].Name, 400);
                            DeathCheck(new List<Card> { _playerBoard[target] });
                        }
                        else
                        {
                            Services.ScrollText("Fiery bat's deathrattle deals 1 damage to Tunder", 400);
                            DamageHero(1);
                        }
                    }
                    break;
                case "Huge Toad":
                    if (_playerBoard.Contains(card))
                    {
                        int target = rand.Next(_enemyBoard.Count + 1);

                        if (target != _enemyBoard.Count)
                        {
                            _enemyBoard[target].Health -= 1;
                            Services.ScrollText("Huge Toad's deathrattle deals 1 damage to " + _enemyBoard[target].Name, 400);
                            DeathCheck(new List<Card> { _enemyBoard[target] });
                        }
                        else
                        {
                            Services.ScrollText("Huge Toad's deathrattle deals 1 damage to Tunder", 400);
                            DamageHero(1);
                        }
                    }
                    else
                    {
                        int target = rand.Next(_playerBoard.Count + 1);

                        if (target != _playerBoard.Count)
                        {
                            _playerBoard[target].Health -= 1;
                            Services.ScrollText("Huge Toad's deathrattle deals 1 damage to " + _playerBoard[target].Name, 400);
                            DeathCheck(new List<Card> { _playerBoard[target] });
                        }
                        else
                        {
                            Services.ScrollText("Huge Toad's deathrattle deals 1 damage to Tunder", 400);
                            DamageHero(1);
                        }
                    }
                    break;
                case "Kindly Grandmother":
                    if (_playerBoard.Contains(card))
                    {
                        _playerBoard.Add(new Card("Kindly Grandmother")
                        {
                            Text = "Deathrattle: Summon a 3/2 Big Bad Wolf",
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1,
                            DeathRattle = true
                        });
                        
                    }
                    else
                    {
                        _enemyBoard.Add(new Card("Kindly Grandmother")
                        {
                            Text = "Deathrattle: Summon a 3/2 Big Bad Wolf",
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1,
                            DeathRattle = true
                        });
                    }
                    Services.ScrollText("Kindly Grandmother summons a Big Bad Wolf", 400);
                    break;
                case "Mounted Raptor":
                    if (_playerTurn)
                    {
                        Card spawn = Cards.OneDrops()[rand.Next(Cards.OneDrops().Count)];
                        _playerBoard.Add(spawn);
                        Services.ScrollText("Mounted raptor spawns you a " + spawn.Name);
                    }
                    else
                    {
                        Card spawn = Cards.OneDrops()[rand.Next(Cards.OneDrops().Count)];
                        _enemyBoard.Add(spawn);
                        Services.ScrollText("Mounted raptor spawns Tunder a " + spawn.Name);
                    } 
                    break;
                case "Infested Wolf":
                    if (_playerTurn)
                    {
                        _playerBoard.Add(new Card("Spider")
                        {
                            Type = "Beast",
                            BaseCost = 1,
                            Cost = 1,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1
                        });

                        _playerBoard.Add(new Card("Spider")
                        {
                            Type = "Beast",
                            BaseCost = 1,
                            Cost = 1,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1
                        });
                        Services.ScrollText("Infested Wolf spawns you two spiders");
                    }
                    else
                    {
                        _enemyBoard.Add(new Card("Spider")
                        {
                            Type = "Beast",
                            BaseCost = 1,
                            Cost = 1,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1
                        });

                        _enemyBoard.Add(new Card("Spider")
                        {
                            Type = "Beast",
                            BaseCost = 1,
                            Cost = 1,
                            BaseHealth = 1,
                            Health = 1,
                            BaseAttack = 1,
                            Attack = 1
                        });
                        Services.ScrollText("Infested Wolf spawns Tunder two spiders");
                    }
                    break;
                case "The Beast":
                    if (!_playerTurn)
                    {
                        _playerBoard.Add(new Card("Finkle Einhorn")
                        {
                            BaseCost = 3,
                            Cost = 3,
                            BaseHealth = 3,
                            Health = 3,
                            BaseAttack = 3,
                            Attack = 3
                        });
                        Services.ScrollText("The Beast spawns you Finkle Einhorn");
                    }
                    else
                    {
                        _enemyBoard.Add(new Card("Finkle Einhorn")
                        {
                            BaseCost = 3,
                            Cost = 3,
                            BaseHealth = 3,
                            Health = 3,
                            BaseAttack = 3,
                            Attack = 3
                        });
                        Services.ScrollText("The Beast spawns Tunder Finkle Einhorn");
                    }
                    break;
                case "Savannah Highmane":
                    if (_playerTurn)
                    {
                        _playerBoard.Add(new Card("Hyena")
                        {
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 2,
                            Health = 2,
                            BaseAttack = 2,
                            Attack = 2
                        });

                        _playerBoard.Add(new Card("Hyena")
                        {
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 2,
                            Health = 2,
                            BaseAttack = 2,
                            Attack = 2
                        });
                        Services.ScrollText("Savannah Highmane spawns you two hyenas");
                    }
                    else
                    {
                        _enemyBoard.Add(new Card("Hyena")
                        {
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 2,
                            Health = 2,
                            BaseAttack = 2,
                            Attack = 2
                        });

                        _enemyBoard.Add(new Card("Hyena")
                        {
                            Type = "Beast",
                            BaseCost = 2,
                            Cost = 2,
                            BaseHealth = 2,
                            Health = 2,
                            BaseAttack = 2,
                            Attack = 2
                        });
                        Services.ScrollText("Savannah Highmane spawns Tunder two hyenas");
                    }
                    break;
                    
            }
        }

        public List<Card> ListDeathrattles()
        {
            List<Card> deathrattles = new List<Card>();

            foreach (Card card in _playerBoard)
                if (card.DeathRattle)
                    deathrattles.Add(card);

            return deathrattles;
        }

        public List<Card> CanKodo()
        {
            List<Card> kodoTargets = new List<Card>();

            if (_playerTurn)
                foreach (Card card in _enemyBoard)
                    if (card.Attack <= 2)
                        kodoTargets.Add(card);
            else
                foreach (Card smallCard in _playerBoard)
                    if (smallCard.Attack <= 2)
                        kodoTargets.Add(smallCard);

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
            if (player)
            {
                for (int i = 0; i < amount; i++)
                {
                    int num = rand.Next(_playerDeck.Count);

                    if (!_mulliganStage)
                        Services.ScrollText("You draw a " + _playerDeck[num].Name, 400);

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
                        Services.ScrollText("Tunder draws a card", 400);

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
            Card[] tempAry = new Card[_enemyBoard.Count];
            _enemyBoard.CopyTo(tempAry);

            foreach (Card card in tempAry)
            {
                if (card.Attacks > 0)
                {
                    List<Card> tauntList = new List<Card>();

                    foreach (Card minion in _playerBoard)
                        if (minion.Taunt)
                            tauntList.Add(minion);

                    Card targetCard;

                    if (tauntList.Count == 0)
                    {
                        int target = rand.Next(_playerBoard.Count + 1);

                        if (target == _playerBoard.Count)
                            targetCard = null;
                        else
                            targetCard = _playerBoard[target];
                    }
                    else
                    {
                        int target = rand.Next(tauntList.Count);

                        targetCard = tauntList[target];
                    }

                    Swing(card, targetCard);
                }
            }
        }

        public void EndOfTurn(bool Tunder = true)
        {
            if (Tunder)
            {
                foreach (Card card in _enemyBoard)
                    if (card.EndOfTurn)
                        ProcEndOfTurn(card);
            }
            else
            {
                foreach (Card card in _playerBoard)
                    if (card.EndOfTurn)
                        ProcEndOfTurn(card);
            }
        }

        public void ProcEndOfTurn(Card card)
        {
            switch (card.Name)
            {
                case "Dreadscale":
                    foreach (Card minion in _playerBoard)
                        if (minion != card)
                            minion.Health--;

                    foreach (Card enemyMinion in _enemyBoard)
                        if (enemyMinion != card)
                            enemyMinion.Health--;

                    Services.ScrollText("Dreadscale deals 1 damage to all other minions", 500);

                    DeathCheck(_playerBoard);
                    DeathCheck(_enemyBoard);
                    break;
            }
        }
    }
}
