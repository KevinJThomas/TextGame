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

        public int PlayerMana { get; set; }
        public int EnemyMana { get; set; }

        bool _mulliganStage = true;

        public CardGame(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

            AddWebspinnerToDeck(true, 30);
            AddWebspinnerToDeck(false, 30);

            PlayerMana = 0;
            EnemyMana = 0;
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
            if (PlayerMana <= 10)
                PlayerMana++;

            PrintBoard();
            DrawCard();
            //TODO: Funtionality for playing a full turn
        }

        public void EnemyTurn()
        {
            //TODO: Basic AI to play a full turn
        }

        public void PrintBoard()
        {
            //TODO: Prints both sides of the board
        }

        public void PrintHand()
        {
            //TODO: Prints the cards in the player's hand
        }

        public void PrintCount()
        {
            Services.FastScrollText("Your hand: " + _playerCards.Count + "\nEnemy hand: " + _enemyCards.Count + "\n");
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
