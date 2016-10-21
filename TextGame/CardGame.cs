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

        public int PlayerMana { get; set; } = 0;
        public int PlayerCurrentMana { get; set; }
        public int EnemyMana { get; set; } = 0;
        public int EnemyCurrentMana { get; set; }
        public int MaxMana { get; set; } = 10;

        bool _mulliganStage = true;
        bool _playerTurn;

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
            //TODO: Write intro text before the game starts
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
                _mulliganStage = false;
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
                EnemyTurn();
            }
            
        }

        public void StartTurn()
        {
            _playerTurn = true;

            if (PlayerMana <= MaxMana)
                PlayerMana++;

            PlayerCurrentMana = PlayerMana; //refill empty mana crystals for the turn

            DrawCard();
            PrintBoard();
            Listen();
        }

        public void EnemyTurn()
        {
            //TODO: Basic AI to play a full turn
            _playerTurn = false;
            Services.ScrollText("ENEMY PLAYING TURN...", 500); //testing only
            StartTurn();
        }

        public void Listen()
        {
            PrintOptions();
            Console.Write("> ");

            int cmd;
            string input = Console.ReadLine();

            if (Int32.TryParse(input, out cmd))
            {
                if (cmd <= _playerCards.Count)
                {
                    PlayCard(_playerCards[cmd - 1]);
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.", 500);
                    Listen();
                }
            }
            else
            {
                switch (input.ToLower())
                {
                    case "attack":
                        //TODO: attacking interface
                        break;
                    case "hand":
                        PlayHand();
                        Listen();
                        break;
                    case "board":
                        PrintBoard();
                        Listen();
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
                Services.FastScrollText("OPPONENT");
                if (_enemyBoard.Count > 0)
                {
                    foreach (Card card in _enemyBoard)
                    {
                        Services.FastScrollText(card.Name + "(" + card.Attack + "/" + card.Health + ") ", 0, true);
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
                        Services.FastScrollText(card.Name + "(" + card.Attack + "/" + card.Health + ") ", 0, true);
                    }
                }
                else
                {
                    Console.Write("(empty)");
                }

                Console.WriteLine();
                Services.FastScrollText("YOU\n");
            }
            else
            {
                Services.ScrollText("The board is empty\n");
            }
        }

        //public void PrintHand()
        //{
        //    foreach (Card card in _playerCards)
        //    {
        //        if (!card.Spell)
        //            Services.FastScrollText("[ *" + card.Cost + "* " + card.Name + " (" + card.Attack + "/" + card.Health + ") ] ", 0, true);
        //        else
        //            Services.FastScrollText("[ *" + card.Cost + "* " + card.Name + " ]", 0, true);
        //    }
        //    Console.WriteLine();
        //}

        public void PrintCount()
        {
            Services.FastScrollText("Your hand: " + _playerCards.Count + "\nEnemy hand: " + _enemyCards.Count + "\n");
        }

        public void PrintHand()
        {
            if (_playerCards.Count > 0)
            {
                Services.FastScrollText("\nYour hand:             Mana: (" + PlayerCurrentMana + "/" + PlayerMana + ")");
                foreach (Card card in _playerCards)
                {
                    Services.FastScrollText((_playerCards.IndexOf(card) + 1).ToString() + ") " + card.Name + " (" + card.Attack + "/" + card.Health + ")");
                }
                Console.WriteLine();
            }
            else
            {
                Services.ScrollText("Your hand is empty");
            }
        }

        public void PrintOptions()
        {
            Services.FastScrollText("You have " + _playerCards.Count + " cards. Mana: (" + PlayerCurrentMana + "/" + PlayerMana + ")\n");
            Services.FastScrollText("attack: view and select available minions to attack with");
            Services.FastScrollText("hand: view and play cards from your hand");
            Services.FastScrollText("board: print out the board");
            Services.FastScrollText("count: print out a count of each player's hand");
            Services.FastScrollText("end turn: end your turn\n");
        }

        public void PlayHand()
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
                Listen();
            }
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
    }
}
