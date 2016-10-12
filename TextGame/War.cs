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
    //1) Find new music
    //TBD:
    //1) Receive currency at beginning or end of turn?
    //2) Queue up all moves and execute at end of turn or perform them immediately 1 by 1 as they're made?
    //3) Add more units/upgrades/items?
    //4) Give currency based on how many spaces occupied, how many spaces controlled, how many units? Probably a combination?
    //5) Give AI currency advantage since it will be stupid?
    //6) Give AI different unit types or use same as player?
    //7) Win condition when one player no longer controls any units, or should there be a target 'king' unit to kill like chess?
    //8) Enemy turn instantly execute to make faster gameplay or should it be slower so the player can keep up with what's happening?
    //TODO:
    //1) CalculatePay()
    //2) Attacking and moving units (1 action per turn using Sleeping prop on units) - add 'all units' command where you can do the same command with all units in the
    //   location at once
    //3) EnemyTurn() - make enemies always attack if available else randomly move.. could make a predefined strat for them to always use
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
        List<List<Unit>> allAllyUnits;

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
        List<List<Unit>> allEnemyUnits;

        Unit selectedUnit;
        bool _unitSelected = false;

        List<List<Unit>> currentUnits = new List<List<Unit>>();
        int _zoomedLocation;

        bool _zoomed = false;

        double _playerCurrency;
        double _enemyCurrency;

        bool _firstTurn = true;

        public War(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

            allAllyUnits = new List<List<Unit>>() { allyUnits1, allyUnits2, allyUnits3, allyUnits4, allyUnits5, allyUnits6, allyUnits7,
                allyUnits8, allyUnits9, allyUnits10, allyUnits11, allyUnits12};
            allEnemyUnits = new List<List<Unit>>() { enemyUnits1, enemyUnits2, enemyUnits3, enemyUnits4, enemyUnits5, enemyUnits6,
                enemyUnits7, enemyUnits8, enemyUnits9, enemyUnits10, enemyUnits11, enemyUnits12};

            _playerCurrency = 100000; //change back to 1000 after testing
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
            Services.ScrollText("Infantry, ranged, and mounted units: you've called in all the stops.", 1500);
            Services.ScrollText("Do you have what it takes to win this war?", 1500);
            StartTurn();
        }

        public void StartTurn()
        {
            if (_firstTurn)
            {
                _firstTurn = false;
                PrintMap();
                Listen();
            }
            else
            {
                Services.ScrollText("It's your turn!", 500);
                int pay = CalculatePay();
                _playerCurrency += pay;
                Services.ScrollText("You are given $" + pay, 1200);
                PrintMap();
                Listen();
            }
            
        }

        public void EnemyTurn()
        {
            //TODO
        }

        public void PrintMap()
        {
            Console.WriteLine("E = Enemy, A = Ally\n");
            Console.WriteLine(" ___________________________________________________");
            Console.WriteLine("| E: " + enemyUnits10.Count.ToString("00") + "       10 | E: " + enemyUnits11.Count.ToString("00") + "       11 | E: " + enemyUnits12.Count.ToString("00") + "        12 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits10.Count.ToString("00") + "          | A: " + allyUnits11.Count.ToString("00") + "          | A: " + allyUnits12.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits7.Count.ToString("00") + "        7 | E: " + enemyUnits8.Count.ToString("00") + "        8 | E: " + enemyUnits9.Count.ToString("00") + "         9 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits7.Count.ToString("00") + "          | A: " + allyUnits8.Count.ToString("00") + "          | A: " + allyUnits9.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits4.Count.ToString("00") + "        4 | E: " + enemyUnits5.Count.ToString("00") + "        5 | E: " + enemyUnits6.Count.ToString("00") + "         6 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits4.Count.ToString("00") + "          | A: " + allyUnits5.Count.ToString("00") + "          | A: " + allyUnits6.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + enemyUnits1.Count.ToString("00") + "        1 | E: " + enemyUnits2.Count.ToString("00") + "        2 | E: " + enemyUnits3.Count.ToString("00") + "         3 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + allyUnits1.Count.ToString("00") + "          | A: " + allyUnits2.Count.ToString("00") + "          | A: " + allyUnits3.Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|\n");
        }

        public void PrintLocation()
        {
            Console.WriteLine("AREA " + _zoomedLocation);
            Console.WriteLine("_______________________________________________\n");
            Services.ScrollText("ENEMIES:");
            if (currentUnits[1].Count > 0)
            {
                foreach (Unit unit in currentUnits[1])
                {
                    if (unit != currentUnits[1][currentUnits[1].Count - 1])
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ") - ");
                    }
                    else
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")\n\n");
                    }
                }
            }
            else
            {
                Services.ScrollText("There are no enemy units here\n");
            }
            

            Services.ScrollText("ALLIES:");
            if (currentUnits[0].Count > 0)
            {
                foreach (Unit unit in currentUnits[0])
                {
                    if (unit != currentUnits[0][currentUnits[0].Count - 1])
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ") - ");
                    }
                    else
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")\n\n");
                    }
                }
            }
            else
            {
                Services.ScrollText("There are no ally units here\n");
            }
            Console.WriteLine("_______________________________________________\n");
        }

        public void Listen()
        {
            if (!_zoomed)
            {
                HelpZoomedOut();

                Console.Write("> ");
                string cmd = Console.ReadLine().ToLower();

                if (cmd.Length >= 4 && cmd.Substring(0, 4) == "zoom" && cmd.Length >= 6)
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
                else
                {
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
                    case "units":
                        //TODO
                        //Lists all units with the option of either selecting one for further options or typing back/exit to go back to listening
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
            Services.ScrollText("Your Currency: $" + _playerCurrency + "\n");
            foreach (string type in shopTypes)
            {
                Services.ScrollText((Array.IndexOf(shopTypes, type) + 1) + ") " + type);
            }
            Console.WriteLine();

            int decision;
            Console.Write("> ");
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
                Services.ScrollText("Exiting shop. . .\n");
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
            Services.ScrollText("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopInfantry)
            {
                Services.ScrollText((Array.IndexOf(shopInfantry, item) + 1) + ") " + item + " (" + priceInfantry[Array.IndexOf(shopInfantry, item)] + ")");
            }
            Console.WriteLine();

            int decision;
            Console.Write("> ");
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopInfantry.Length)
                {
                    Purchase(1, shopInfantry[decision - 1]);
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n");
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
            Services.ScrollText("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopArcher)
            {
                Services.ScrollText((Array.IndexOf(shopArcher, item) + 1) + ") " + item + " (" + priceArcher[Array.IndexOf(shopArcher, item)] + ")");
            }
            Console.WriteLine();

            int decision;
            Console.Write("> ");
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopArcher.Length)
                {
                    Purchase(2, shopArcher[decision - 1]);
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n");
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
            Services.ScrollText("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopCavalier)
            {
                Services.ScrollText((Array.IndexOf(shopCavalier, item) + 1) + ") " + item + " (" + priceCavalier[Array.IndexOf(shopCavalier, item)] + ")");
            }
            Console.WriteLine();

            int decision;
            Console.Write("> ");
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopCavalier.Length)
                {
                    Purchase(3, shopCavalier[decision - 1]);
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n");
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
            Services.ScrollText("Your Currency: $" + _playerCurrency + "\n");
            foreach (string item in shopOther)
            {
                Services.ScrollText((Array.IndexOf(shopOther, item) + 1) + ") " + item + " (" + priceOther[Array.IndexOf(shopOther, item)] + ")");
            }
            Console.WriteLine();

            int decision;
            Console.Write("> ");
            string cmd = Console.ReadLine();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopOther.Length)
                {
                    Purchase(4, shopOther[decision - 1]);
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n");
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

        public void Purchase(int type, string item)
        {
            switch (type)
            {
                case 1: //Infantry
                    switch (item)
                    {
                        case "New Infantry":
                            if (_playerCurrency >= priceInfantry[Array.IndexOf(shopInfantry, item)])
                            {
                                _playerCurrency -= priceInfantry[Array.IndexOf(shopInfantry, item)];
                                Services.ScrollText("You purchased a new infantry unit.", 300);
                                PlaceUnit(1);
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopInfantry();
                            }
                            break;
                        case "Weapon Upgrade":
                            if (_playerCurrency >= priceInfantry[Array.IndexOf(shopInfantry, item)])
                            {
                                ListInfantry();
                                if (_unitSelected)
                                {
                                    switch (selectedUnit.Attack)
                                    {
                                        case 2:
                                            _playerCurrency -= priceInfantry[Array.IndexOf(shopInfantry, item)];
                                            Services.ScrollText("You upgrade your unit with a sword!");
                                            selectedUnit.AddSword();
                                            Shop();
                                            break;
                                        case 4:
                                            _playerCurrency -= priceInfantry[Array.IndexOf(shopInfantry, item)];
                                            Services.ScrollText("You upgrade your unit with an axe!");
                                            selectedUnit.AddAxe();
                                            Shop();
                                            break;
                                        case 6:
                                            Services.ScrollText("You can't upgrade that unit's weapon anymore!");
                                            Shop();
                                            break;
                                        default:
                                            Services.ScrollText("ERROR: Purchase(int, string) case 1, case Weapon Upgrade..selectedUnit.Attack outOfIndex (" +
                                                selectedUnit.Attack + ")");
                                            Shop();
                                            break;
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopInfantry();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopInfantry();
                            }
                            break;
                        case "Shield":
                            if (_playerCurrency >= priceInfantry[Array.IndexOf(shopInfantry, item)])
                            {
                                ListInfantry();
                                if (_unitSelected)
                                {
                                    if (!selectedUnit.HasShield())
                                    {
                                        _playerCurrency -= priceInfantry[Array.IndexOf(shopInfantry, item)];
                                        Services.ScrollText("You upgrade your unit with a shield!");
                                        selectedUnit.AddShield();
                                        Shop();
                                    }
                                    else
                                    {
                                        Services.ScrollText("That unit already has a shield!");
                                        Shop();
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopInfantry();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopInfantry();
                            }
                            break;
                        default:
                            Services.ScrollText("ERROR: Purchase(int, string) case 1..no matched case for " + item);
                            Shop();
                            break;
                    }
                    break;
                case 2: //Archer
                    switch (item)
                    {
                        case "New Archer":
                            if (_playerCurrency >= priceArcher[Array.IndexOf(shopArcher, item)])
                            {
                                _playerCurrency -= priceArcher[Array.IndexOf(shopArcher, item)];
                                Services.ScrollText("You purchased a new archer unit.", 300);
                                PlaceUnit(2);
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopArcher();
                            }
                            break;
                        case "Weapon Upgrade":
                            if (_playerCurrency >= priceArcher[Array.IndexOf(shopArcher, item)])
                            {
                                ListArcher();
                                if (_unitSelected)
                                {
                                    switch (selectedUnit.Attack)
                                    {
                                        case 4:
                                            _playerCurrency -= priceArcher[Array.IndexOf(shopArcher, item)];
                                            Services.ScrollText("You upgrade your unit with a crossbow!");
                                            selectedUnit.AddCrossbow();
                                            Shop();
                                            break;
                                        case 7:
                                            Services.ScrollText("You can't upgrade that unit's weapon anymore!");
                                            Shop();
                                            break;
                                        default:
                                            Services.ScrollText("ERROR: Purchase(int, string) case 1, case Weapon Upgrade..selectedUnit.Attack outOfIndex (" +
                                                selectedUnit.Attack + ")");
                                            Shop();
                                            break;
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopArcher();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopArcher();
                            }
                            break;
                        case "Armor Upgrade":
                            if (_playerCurrency >= priceArcher[Array.IndexOf(shopArcher, item)])
                            {
                                ListArcher();
                                if (_unitSelected)
                                {
                                    if (!selectedUnit.HasArmor())
                                    {
                                        _playerCurrency -= priceArcher[Array.IndexOf(shopArcher, item)];
                                        Services.ScrollText("You upgrade your unit with armor!");
                                        selectedUnit.AddLeatherArmor();
                                        Shop();
                                    }
                                    else
                                    {
                                        Services.ScrollText("That unit already has armor!");
                                        Shop();
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopArcher();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopArcher();
                            }
                            break;
                        default:
                            Services.ScrollText("ERROR: Purchase(int, string) case 2..no matched case for " + item);
                            Shop();
                            break;
                    }
                            break;
                case 3: //Cavalier
                    switch (item)
                    {
                        case "New Cavalier":
                            if (_playerCurrency >= priceCavalier[Array.IndexOf(shopCavalier, item)])
                            {
                                _playerCurrency -= priceCavalier[Array.IndexOf(shopCavalier, item)];
                                Services.ScrollText("You purchased a new cavalier unit.", 300);
                                PlaceUnit(3);
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopCavalier();
                            }
                            break;
                        case "Weapon Upgrade":
                            if (_playerCurrency >= priceCavalier[Array.IndexOf(shopCavalier, item)])
                            {
                                ListCavalier();
                                if (_unitSelected)
                                {
                                    switch (selectedUnit.Attack)
                                    {
                                        case 3:
                                            _playerCurrency -= priceCavalier[Array.IndexOf(shopCavalier, item)];
                                            Services.ScrollText("You upgrade your unit with a javelin!");
                                            selectedUnit.AddJavelin();
                                            Shop();
                                            break;
                                        case 6:
                                            Services.ScrollText("You can't upgrade that unit's weapon anymore!");
                                            Shop();
                                            break;
                                        default:
                                            Services.ScrollText("ERROR: Purchase(int, string) case 1, case Weapon Upgrade..selectedUnit.Attack outOfIndex (" +
                                                selectedUnit.Attack + ")");
                                            Shop();
                                            break;
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopCavalier();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopCavalier();
                            }
                            break;
                        case "Shield":
                            if (_playerCurrency >= priceCavalier[Array.IndexOf(shopCavalier, item)])
                            {
                                ListCavalier();
                                if (_unitSelected)
                                {
                                    if (!selectedUnit.HasShield())
                                    {
                                        _playerCurrency -= priceCavalier[Array.IndexOf(shopCavalier, item)];
                                        Services.ScrollText("You upgrade your unit with a shield!");
                                        selectedUnit.AddShield();
                                        Shop();
                                    }
                                    else
                                    {
                                        Services.ScrollText("That unit already has a shield!");
                                        Shop();
                                    }
                                    _unitSelected = false;
                                }
                                else
                                {
                                    ShopCavalier();
                                }
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopCavalier();
                            }
                            break;
                        default:
                            Services.ScrollText("ERROR: Purchase(int, string) case 3..no matched case for " + item);
                            Shop();
                            break;
                    }
                    break;
                case 4: //Other
                    switch (item)
                    {
                        case "Arrow Shield":
                            if (_playerCurrency >= priceOther[Array.IndexOf(shopOther, item)])
                            {
                                _playerCurrency -= priceOther[Array.IndexOf(shopOther, item)];
                                Services.ScrollText("You purchased a new arrow shield.", 300);
                                PlaceUnit(4);
                            }
                            else
                            {
                                Services.ScrollText("You don't have enough money for that!");
                                ShopOther();
                            }
                            break;
                        default:
                            Services.ScrollText("ERROR: Purchase(int, string) case 4..no matched case for " + item);
                            Shop();
                            break;
                    }
                    break;
                default:
                    Services.ScrollText("Error: Purchase(int, string) outOfIndex for base case statement (" + type + ")");
                    break;
            }
        }

        public void PlaceUnit(int type)
        {
            if (type >= 1 && type <= 3)
            {
                Services.ScrollText("Where would you like to place your new unit?");
            }
            else
            {
                Services.ScrollText("Where would you like to place your new item?");
            }
            
            Services.ScrollText("1) 1");
            Services.ScrollText("2) 2");
            Services.ScrollText("3) 3");

            int decision;
            Console.Write("> ");
            string answer = Console.ReadLine();

            if(Int32.TryParse(answer, out decision))
            {
                switch (decision)
                {
                    case 1:
                        allyUnits1.Add(new Unit(type));
                        Shop();
                        break;
                    case 2:
                        allyUnits2.Add(new Unit(type));
                        Shop();
                        break;
                    case 3:
                        allyUnits3.Add(new Unit(type));
                        Shop();
                        break;
                    default:
                        Services.ScrollText("Invalid input. Try again.");
                        PlaceUnit(type);
                        break;
                }
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                PlaceUnit(type);
            }
        }

        public void ListInfantry()
        {
            List<Unit> tempUnitList = new List<Unit>();
            List<int> tempIntList = new List<int>();
            foreach (List<Unit> list in allAllyUnits)
            {
                foreach (Unit unit in list)
                {
                    if (unit.Type == 1)
                    {
                        tempUnitList.Add(unit);
                        tempIntList.Add(allAllyUnits.IndexOf(list) + 1);
                    }
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) + 
                        ") Infantry A:" + unit.Attack + " H:" + unit.Health + " Location: " + tempIntList[tempUnitList.IndexOf(unit)]);
                }

                int decision;
                Console.Write("> ");
                string answer = Console.ReadLine();

                if (Int32.TryParse(answer, out decision))
                {
                    if (decision <= tempUnitList.Count)
                    {
                        selectedUnit = tempUnitList[decision - 1];
                        _unitSelected = true;
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                }
            }
            else
            {
                Services.ScrollText("You don't have any infantry to upgrade!", 500);
            }
        }

        public void ListArcher()
        {
            List<Unit> tempUnitList = new List<Unit>();
            List<int> tempIntList = new List<int>();
            foreach (List<Unit> list in allAllyUnits)
            {
                foreach (Unit unit in list)
                {
                    if (unit.Type == 2)
                    {
                        tempUnitList.Add(unit);
                        tempIntList.Add(allAllyUnits.IndexOf(list) + 1);
                    }
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) +
                        ") Archer A:" + unit.Attack + " H:" + unit.Health + " Location: " + tempIntList[tempUnitList.IndexOf(unit)]);
                }

                int decision;
                Console.Write("> ");
                string answer = Console.ReadLine();

                if (Int32.TryParse(answer, out decision))
                {
                    if (decision <= tempUnitList.Count)
                    {
                        selectedUnit = tempUnitList[decision - 1];
                        _unitSelected = true;
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                }
            }
            else
            {
                Services.ScrollText("You don't have any infantry to upgrade!", 500);
            }
        }

        public void ListCavalier()
        {
            List<Unit> tempUnitList = new List<Unit>();
            List<int> tempIntList = new List<int>();
            foreach (List<Unit> list in allAllyUnits)
            {
                foreach (Unit unit in list)
                {
                    if (unit.Type == 3)
                    {
                        tempUnitList.Add(unit);
                        tempIntList.Add(allAllyUnits.IndexOf(list) + 1);
                    }
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) +
                        ") Cavalier A:" + unit.Attack + " H:" + unit.Health + " Location: " + tempIntList[tempUnitList.IndexOf(unit)]);
                }

                int decision;
                Console.Write("> ");
                string answer = Console.ReadLine();

                if (Int32.TryParse(answer, out decision))
                {
                    if (decision <= tempUnitList.Count)
                    {
                        selectedUnit = tempUnitList[decision - 1];
                        _unitSelected = true;
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                }
            }
            else
            {
                Services.ScrollText("You don't have any cavaliers to upgrade!", 500);
            }
        }

        public void HelpZoomedOut()
        {
            Services.FastScrollText("COMMANDS:");
            Services.FastScrollText("map: prints out the overview of the battlefield");
            Services.FastScrollText("count: gives a total for all units for both players");
            Services.FastScrollText("shop: opens the shop where you can spend your resources");
            Services.FastScrollText("zoom *1-12*: take a closer look at a specific location - this is the view where units are controlled");
            Services.FastScrollText("end turn: ends your turn\n");
        }

        public void HelpZoomedIn()
        {
            Services.FastScrollText("COMMANDS:");
            Services.FastScrollText("map: prints out the overview of the location you're zoomed in on");
            Services.FastScrollText("count: gives a total for units in the location you're zoomed in on for both players");
            Services.FastScrollText("units: access individual units to move or attack with them");
            Services.FastScrollText("zoom: zooms out to the full map view");
            Services.FastScrollText("end turn: ends your turn\n");
        }

        public void Zoom(int num)
        {
            switch (num)
            {
                case 1:
                    currentUnits.Add(allyUnits1);
                    currentUnits.Add(enemyUnits1);
                    break;
                case 2:
                    currentUnits.Add(allyUnits2);
                    currentUnits.Add(enemyUnits2);
                    break;
                case 3:
                    currentUnits.Add(allyUnits3);
                    currentUnits.Add(enemyUnits3);
                    break;
                case 4:
                    currentUnits.Add(allyUnits4);
                    currentUnits.Add(enemyUnits4);
                    break;
                case 5:
                    currentUnits.Add(allyUnits5);
                    currentUnits.Add(enemyUnits5);
                    break;
                case 6:
                    currentUnits.Add(allyUnits6);
                    currentUnits.Add(enemyUnits6);
                    break;
                case 7:
                    currentUnits.Add(allyUnits7);
                    currentUnits.Add(enemyUnits7);
                    break;
                case 8:
                    currentUnits.Add(allyUnits8);
                    currentUnits.Add(enemyUnits8);
                    break;
                case 9:
                    currentUnits.Add(allyUnits9);
                    currentUnits.Add(enemyUnits9);
                    break;
                case 10:
                    currentUnits.Add(allyUnits10);
                    currentUnits.Add(enemyUnits10);
                    break;
                case 11:
                    currentUnits.Add(allyUnits11);
                    currentUnits.Add(enemyUnits11);
                    break;
                case 12:
                    currentUnits.Add(allyUnits12);
                    currentUnits.Add(enemyUnits12);
                    break;
                default:
                    Services.ScrollText("ERROR: Zoom(int) case not defined for (" + num + ")");
                    break;
            }
            _zoomedLocation = num;
            _zoomed = true;
            PrintLocation();
            Listen();
        }

        public void ZoomOut()
        {
            currentUnits.Clear();
            _zoomedLocation = 0;
            _zoomed = false;
        }

        public int CalculatePay()
        {
            return 0; //TODO
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
