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
    //1) Instead of using switch statements in ShopArcher(), ShopInfantry(), etc.. maybe just do something like Purchase(int type, string name) where type refers
    //to the type of unit to not get things like 'shield' confused with different types of units that can all receive a shield
    //2) Find new music
    //TBD:
    //1) Receive currency at beginning or end of turn?
    //2) Queue up all moves and execute at end of turn or perform them immediately 1 by 1 as they're made?
    //3) Add more units/upgrades/items?
    //4) Give currency based on how many spaces occupied, how many spaces controlled, how many units? Probably a combination?
    //5) Give AI currency advantage since it will be stupid?
    //6) Give AI different unit types or use same as player?
    //7) Win condition when one player no longer controls any units, or should there be a target 'king' unit to kill like chess?
    //8) Good idea to add 'terrain' where there is advantage on high ground or some units gain stats in certain terrain?
    //9) Enemy turn instantly execute to make faster gameplay or should it be slower so the player can keep up with what's happening?
    class War : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        string[] shopTypes = new string[] { "Infantry", "Archer", "Cavalier", "Other" };
        string[] shopInfantry = new string[] { "New Infantry", "Weapon Upgrade", "Shield" };
        string[] shopArcher = new string[] { "New Archer", "Weapon Upgrade", "Armor Upgrade" };
        string[] shopCavalier = new string[] { "New Cavalier", "Weapon Upgrade", "Shield" };
        string[] shopOther = new string[] { "Arrow Shield" };

        int[] priceInfantry = new int[] { 250, 200, 250 };
        int[] priceArcher = new int[] { 350, 450, 250 };
        int[] priceCavalier = new int[] { 700, 1000, 350 };
        int[] priceOther = new int[] { 1000 };

        //ally units
        List<Unit> allyUnits1 = new List<Unit>();
        List<Unit> allyUnits2 = new List<Unit>();
        List<Unit> allyUnits3 = new List<Unit>();
        List<Unit> allyUnits4 = new List<Unit>();
        List<Unit> allyUnits5 = new List<Unit>();
        List<Unit> allyUnits6 = new List<Unit>();
        List<Unit> allyUnits7 = new List<Unit>();
        List<Unit> allyUnits8 = new List<Unit>();
        List<Unit> allyUnits9 = new List<Unit>();
        List<Unit> allyUnits10 = new List<Unit>();
        List<Unit> allyUnits11 = new List<Unit>();
        List<Unit> allyUnits12 = new List<Unit>();

        //enemy units
        List<Unit> enemyUnits1 = new List<Unit>();
        List<Unit> enemyUnits2 = new List<Unit>();
        List<Unit> enemyUnits3 = new List<Unit>();
        List<Unit> enemyUnits4 = new List<Unit>();
        List<Unit> enemyUnits5 = new List<Unit>();
        List<Unit> enemyUnits6 = new List<Unit>();
        List<Unit> enemyUnits7 = new List<Unit>();
        List<Unit> enemyUnits8 = new List<Unit>();
        List<Unit> enemyUnits9 = new List<Unit>();
        List<Unit> enemyUnits10 = new List<Unit>();
        List<Unit> enemyUnits11 = new List<Unit>();
        List<Unit> enemyUnits12 = new List<Unit>();

        List<List<Unit>> currentUnits = new List<List<Unit>>();
        int _zoomedLocation;

        bool _zoomed = false;

        double _playerCurrency;
        double _enemyCurrency;

        public War(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

            _playerCurrency = 1000;
            _enemyCurrency = 1000;
        }

        public static void PlayMusic()
        {
            Assembly assembly;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream
                ("TextGame.audio.drums.wav"));
            sp.Stream.Position = 0;
            sp.PlayLooping();
        }

        public void Start()
        {
            Services.ScrollText("You're the general of an army that is about to fight a long and bloody battle to decide to fate of your people.", 1500);
            Services.ScrollText("This war has lasted years, and it's about to be over one way or another.", 1500);
            Services.ScrollText("You've gathered your entire military for this one last stand.", 1500);
            Services.ScrollText("Ground units, vehicles, air units: you've called in all the stops.", 1500);
            Services.ScrollText("Do you have what it takes to win this war?", 1500);
            PlayTurn();
        }

        public void PlayTurn()
        {
            PrintMap();
            Listen();
        }

        public void EnemyTurn()
        {

        }

        public void PrintMap()
        {
            Console.WriteLine("E = Enemy, A = Ally\n");
            Console.WriteLine(" ___________________________________________________");
            Console.WriteLine("| E: " + enemyUnits1.Count.ToString("00") + "        1 | E: " + enemyUnits2.Count.ToString("00") + "        2 | E: " + enemyUnits3.Count.ToString("00") + "         3 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits1.Count.ToString("00") + "          | A: " + allyUnits2.Count.ToString("00") + "          | A: " + allyUnits3.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits4.Count.ToString("00") + "        4 | E: " + enemyUnits5.Count.ToString("00") + "        5 | E: " + enemyUnits6.Count.ToString("00") + "         6 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits4.Count.ToString("00") + "          | A: " + allyUnits5.Count.ToString("00") + "          | A: " + allyUnits6.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits7.Count.ToString("00") + "        7 | E: " + enemyUnits8.Count.ToString("00") + "        8 | E: " + enemyUnits9.Count.ToString("00") + "         9 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits7.Count.ToString("00") + "          | A: " + allyUnits8.Count.ToString("00") + "          | A: " + allyUnits9.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits10.Count.ToString("00") + "       10 | E: " + enemyUnits11.Count.ToString("00") + "       11 | E: " + enemyUnits12.Count.ToString("00") + "        12 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits10.Count.ToString("00") + "          | A: " + allyUnits11.Count.ToString("00") + "          | A: " + allyUnits12.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|\n");
        }

        public void PrintLocation()
        {
            //TODO
            //prints specfic units in w/e location is zoomed in on
        }

        public void Listen()
        {
            if (!_zoomed)
            {
                HelpZoomedOut();

                Console.Write("> ");
                string cmd = Console.ReadLine().ToLower();

                if (cmd.Substring(0, 4) == "zoom" && cmd.Length >= 6)
                {
                    cmd = cmd.Substring(5);
                    int num;

                    if (Int32.TryParse(cmd, out num))
                    {
                        if (num >= 1 && num <= 12)
                        {
                            Zoom(num);
                        }
                    }
                }

                switch (cmd)
                {
                    case "count":
                        Services.ScrollText("Ally units: " + CountAllyUnits() + "\nEnemy units: " + CountEnemyUnits() + "\n");
                        Listen();
                        break;
                    case "shop":
                        Services.ScrollText("Welcome to the shop!", 1000);
                        Services.ScrollText("You can type 'back' at any time to navigate 1 section back from where you currently are.", 1000);
                        Services.ScrollText("If you would ever like to completely exit the shop, type 'exit'.");
                        Shop();
                        break;
                    case "map":
                        PrintMap();
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
            else
            {
                HelpZoomedIn();

                Console.Write("> ");
                string cmd = Console.ReadLine().ToLower();

                switch (cmd)
                {
                    case "count":
                        Services.ScrollText("Ally units: " + currentUnits[0].Count + "\nEnemy units: " + currentUnits[1].Count + "\n");
                        Listen();
                        break;
                    case "map":
                        PrintLocation();
                        Listen();
                        break;
                    case "end turn":
                        EnemyTurn();
                        break;
                    case "zoom":
                        ZoomOut();
                        Listen();
                        break;
                    default:
                        Services.ScrollText("Invalid input. Try again.", 500);
                        Listen();
                        break;
                }
            }
        }

        public void Shop()
        {
            Console.WriteLine("Your Currency: $" + _playerCurrency + "\n");
            foreach (string type in shopTypes)
            {
                Console.WriteLine((Array.IndexOf(shopTypes, type) + 1) + ") " + type);
            }
            Console.WriteLine();

            int decision;
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopTypes.Length)
                {
                    switch (shopTypes[decision - 1])
                    {
                        case "Infantry":
                            ShopInfantry();
                            break;
                        case "Archer":
                            ShopArcher();
                            break;
                        case "Cavalier":
                            ShopCavalier();
                            break;
                        case "Other":
                            ShopOther();
                            break;
                        default:
                            Services.ScrollText("Invalid input. Try again.");
                            Shop();
                            break;
                    }
                }
            }
            else if (cmd == "back" || cmd == "exit")
            {
                Listen();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                Shop();
            }
        }

        public void ShopInfantry()
        {
            Console.WriteLine("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopInfantry)
            {
                Console.WriteLine((Array.IndexOf(shopInfantry, item) + 1) + ") " + item);
            }
            Console.WriteLine();

            int decision;
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopInfantry.Length)
                {
                    switch (shopInfantry[decision - 1])
                    {
                        case "New Infantry":
                            //TODO
                            break;
                        case "Weapon Upgrade":
                            //TODO
                            break;
                        case "Shield":
                            //TODO
                            break;
                        default:
                            Services.ScrollText("Invalid input. Try again.");
                            ShopInfantry();
                            break;
                    }
                }
            }
            else if (cmd == "exit")
            {
                Listen();
            }
            else if (cmd == "back")
            {
                Shop();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                ShopInfantry();
            }
        }

        public void ShopArcher()
        {
            Console.WriteLine("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopArcher)
            {
                Console.WriteLine((Array.IndexOf(shopArcher, item) + 1) + ") " + item);
            }
            Console.WriteLine();

            int decision;
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopArcher.Length)
                {
                    switch (shopArcher[decision - 1])
                    {
                        case "New Archer":
                            //TODO
                            break;
                        case "Weapon Upgrade":
                            //TODO
                            break;
                        case "Armor Upgrade":
                            //TODO
                            break;
                        default:
                            Services.ScrollText("Invalid input. Try again.");
                            ShopArcher();
                            break;
                    }
                }
            }
            else if (cmd == "exit")
            {
                Listen();
            }
            else if (cmd == "back")
            {
                Shop();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                ShopArcher();
            }
        }

        public void ShopCavalier()
        {
            Console.WriteLine("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopCavalier)
            {
                Console.WriteLine((Array.IndexOf(shopCavalier, item) + 1) + ") " + item);
            }
            Console.WriteLine();

            int decision;
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopCavalier.Length)
                {
                    switch (shopCavalier[decision - 1])
                    {
                        case "New Cavalier":
                            //TODO
                            break;
                        case "Weapon Upgrade":
                            //TODO
                            break;
                        case "Shield":
                            //TODO
                            break;
                        default:
                            Services.ScrollText("Invalid input. Try again.");
                            ShopCavalier();
                            break;
                    }
                }
            }
            else if (cmd == "exit")
            {
                Listen();
            }
            else if (cmd == "back")
            {
                Shop();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                ShopCavalier();
            }
        }

        public void ShopOther()
        {
            Console.WriteLine("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopOther)
            {
                Console.WriteLine((Array.IndexOf(shopOther, item) + 1) + ") " + item);
            }
            Console.WriteLine();

            int decision;
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopOther.Length)
                {
                    switch (shopOther[decision - 1])
                    {
                        case "Arrow Shield":
                            //TODO
                            break;
                        default:
                            Services.ScrollText("Invalid input. Try again.");
                            ShopOther();
                            break;
                    }
                }
            }
            else if (cmd == "exit")
            {
                Listen();
            }
            else if (cmd == "back")
            {
                Shop();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                ShopOther();
            }
        }

        public void HelpZoomedOut()
        {
            //TODO
            //zoom (number 1-12): take a closer look at a specfic area - this is the view where units are controlled
            //count: gives a total for all units for both players
            //shop: opens a shop to spend resources
            //end turn: ends your turns
        }

        public void HelpZoomedIn()
        {
            //TODO
            //(number 1 - unit count): select that unit
            //all: select all units in this area
            //zoom: zoom back out to the map view
        }

        public void Zoom(int num)
        {
            //TODO
            //set currentUnits with appropriate lists
            _zoomedLocation = num;
            _zoomed = true;
        }

        public void ZoomOut()
        {
            currentUnits.Clear();
            _zoomedLocation = 0;
            _zoomed = false;
        }

        public int CountAllyUnits()
        {
            return allyUnits1.Count + allyUnits2.Count + allyUnits3.Count + allyUnits4.Count + allyUnits5.Count + allyUnits6.Count + allyUnits7.Count + allyUnits8.Count
                 + allyUnits9.Count + allyUnits10.Count + allyUnits11.Count + allyUnits12.Count;
        }

        public int CountEnemyUnits()
        {
            return enemyUnits1.Count + enemyUnits2.Count + enemyUnits3.Count + enemyUnits4.Count + enemyUnits5.Count + enemyUnits6.Count + enemyUnits7.Count
                + enemyUnits8.Count + enemyUnits9.Count + enemyUnits10.Count + enemyUnits11.Count + enemyUnits12.Count;
        }
    }
}
