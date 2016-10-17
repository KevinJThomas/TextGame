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

        List<Card> _playerCards = new List<Card>();
        List<Card> _enemyCards = new List<Card>();

        public CardGame(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();
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
            //TODO
            StartTurn();
        }

        public void StartTurn()
        {
            //TODO
        }
    }
}
