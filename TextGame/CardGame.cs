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
    //1) Add basic AI to play a turn
    //2) Test webspinner deathrattle to make sure it's working properly
    //3) Test heroes dying
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
        public int PlayerMana { get; set; } = 0;
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
                        //TODO: read cards
                        Listen(); //remove when done
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
                foreach (Card card in _playerCards)
                {
                    Services.FastScrollText((_playerCards.IndexOf(card) + 1).ToString() + ") " + card.Name + " (" + card.Attack + "/" + card.Health + ") *" + card.Cost + "*");
                }
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

            foreach (Card card in tempList)
            {
                Services.FastScrollText((tempList.IndexOf(card) + 1).ToString() + ") " + card.Name + " (" + card.Attack + "/" + card.Health + ")");
            }

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
            
            foreach (Card target in _enemyBoard)
            {
                Services.FastScrollText((_enemyBoard.IndexOf(target) + 1).ToString() + ") " + target.Name + " (" + target.Attack + "/" + target.Health + ")");
            }
            Services.FastScrollText((_enemyBoard.Count + 1).ToString() + ") Enemy Hero");
            Console.Write("> ");

            int cmd;
            string input = Console.ReadLine();

            if (Int32.TryParse(input, out cmd))
            {
                if (cmd <= _enemyBoard.Count + 1)
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
                else
                {
                    Services.ScrollText("Invalid input. Try again.", 500);
                    ChooseTarget(card);
                }
            }
            else if (input.ToLower() == "back")
            {
                PrintBoard();
                Attacking();
            }
            else if (input.ToLower() == "exit")
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
                    DamageHero(attacker.Attack);
                    Services.ScrollText(attacker.Name + " hits Tunder for " + attacker.Attack + " damage", 600);
                }
                else
                {
                    DamageHero(attacker.Attack, false);
                    Services.ScrollText(attacker.Name + " hits you for " + attacker.Attack + " damage", 600);
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
                    _player.LevelCompleted = true;
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
            {
                switch (card.Name)
                {
                    case "Webspinner":
                        Random rand = new Random();
                        Card[] tempArr = (Card[]) _allCards.Clone();

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
                        Cost = 1,
                        Attack = 1,
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
                        Cost = 1,
                        Attack = 1,
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
                        _playerBoard.Add(card);
                        _playerCards.Remove(card);
                        Services.ScrollText("You play " + card.Name);
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
                }
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
