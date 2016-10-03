using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextGame
{
    class Haunted : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        public Haunted(Player player) : base(player)
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
                ("TextGame.audio.eerieMusic.wav"));
            sp.Stream.Position = 0;
            sp.PlayLooping();
        }

        public void Start()
        {
            Console.WriteLine(";) ");
            Console.ReadKey();
        }
        
    }
}
