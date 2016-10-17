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
    class War : Scenario
    {
        Thread musicThread = new Thread(PlayMusic);

        string[] shopTypes = new string[] { "Infantry", "Archer", "Cavalier", "Other" };
        string[] shopInfantry = new string[] { "New Infantry", "Weapon Upgrade", "Shield" };
        string[] shopArcher = new string[] { "New Archer", "Weapon Upgrade", "Armor Upgrade" };
        string[] shopCavalier = new string[] { "New Cavalier", "Weapon Upgrade", "Shield" };
        string[] shopOther = new string[] { "Giant Shield" };

        int[] priceInfantry = new int[] { 250, 200, 250 };
        int[] priceArcher = new int[] { 350, 450, 250 };
        int[] priceCavalier = new int[] { 700, 1000, 350 };
        int[] priceOther = new int[] { 1000 };

        string[] unitCommands = new string[] { "Move", "Attack" };

        int[] moveableLocations1 = new int[] { 2, 4 };
        int[] moveableLocations2 = new int[] { 1, 3, 5 };
        int[] moveableLocations3 = new int[] { 2, 6 };
        int[] moveableLocations4 = new int[] { 1, 5, 7 };
        int[] moveableLocations5 = new int[] { 2, 4, 6, 8 };
        int[] moveableLocations6 = new int[] { 3, 5, 9 };
        int[] moveableLocations7 = new int[] { 4, 8, 10 };
        int[] moveableLocations8 = new int[] { 5, 7, 9, 11 };
        int[] moveableLocations9 = new int[] { 6, 8, 12 };
        int[] moveableLocations10 = new int[] { 7, 11 };
        int[] moveableLocations11 = new int[] { 8, 10, 12 };
        int[] moveableLocations12 = new int[] { 9, 12 };

        List<Unit> allyUnits = new List<Unit>();

        List<Unit> enemyUnits = new List<Unit>();

        Unit selectedUnit;
        bool _unitSelected = false;
        
        int _zoomedLocation;

        bool _zoomed = false;

        int _playerCurrency;
        int _enemyCurrency;

        bool _firstTurn = true;
        bool _gameOver = false;

        public War(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();
            
            //Starting units for enemy
            enemyUnits.Add(new Unit(3, 10));
            enemyUnits.Add(new Unit(2, 12));
            enemyUnits.Add(new Unit(1, 11));
            enemyUnits.Add(new Unit(1, 11));

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
                Services.FastScrollText("Your resources: $" + _playerCurrency + "\n");
                Listen();
            }
            else
            {
                _zoomed = false;
                _zoomedLocation = 0;

                foreach (Unit unit in allyUnits)
                {
                    unit.Sleeping = false;
                }
                Services.ScrollText("It's your turn!", 500);
                
                _playerCurrency += CalculatePay();
                Services.ScrollText("You are given $" + CalculatePay(), 1200);

                if (!CheckWin() && !CheckLose())
                {
                    PrintMap();
                    Services.FastScrollText("Your resources: $" + _playerCurrency + "\n");
                    Listen();
                }
            }
            
        }

        public void EnemyTurn()
        {
            foreach (Unit unit in enemyUnits)
            {
                unit.Sleeping = false;
            }
            
            _enemyCurrency += EnemyCalculatePay();
            Services.ScrollText("The enemy is given $" + EnemyCalculatePay(), 1200);

            EnemyShop();

            foreach (Unit unit in enemyUnits)
            {
                if (unit.Sleeping == false)
                {
                    if (CheckForTargets(unit))
                    {
                        EnemyAttack(unit);
                    }
                    else
                    {
                        EnemyMove(unit);
                    }
                }
            }

            StartTurn();
        }

        public void PrintMap()
        {
            Console.WriteLine("\nE = Enemy, A = Ally\n");
            Console.WriteLine(" ___________________________________________________");
            Console.WriteLine("| E: " + GetUnitsAtLocation(10, false).Count.ToString("00") + "       10 | E: " + GetUnitsAtLocation(11, false).Count.ToString("00") + "       11 | E: " + GetUnitsAtLocation(12, false).Count.ToString("00") + "        12 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + GetUnitsAtLocation(10, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(11, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(12, true).Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + GetUnitsAtLocation(7, false).Count.ToString("00") + "        7 | E: " + GetUnitsAtLocation(8, false).Count.ToString("00") + "        8 | E: " + GetUnitsAtLocation(9, false).Count.ToString("00") + "         9 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + GetUnitsAtLocation(7, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(8, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(9, true).Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + GetUnitsAtLocation(4, false).Count.ToString("00") + "        4 | E: " + GetUnitsAtLocation(5, false).Count.ToString("00") + "        5 | E: " + GetUnitsAtLocation(6, false).Count.ToString("00") + "         6 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + GetUnitsAtLocation(4, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(5, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(6, true).Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|");
            Console.WriteLine("| E: " + GetUnitsAtLocation(1, false).Count.ToString("00") + "        1 | E: " + GetUnitsAtLocation(2, false).Count.ToString("00") + "        2 | E: " + GetUnitsAtLocation(3, false).Count.ToString("00") + "         3 |");
            Console.WriteLine("|                |                |                 |");
            Console.WriteLine("| A: " + GetUnitsAtLocation(1, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(2, true).Count.ToString("00") + "          | A: " + GetUnitsAtLocation(3, true).Count.ToString("00") + "           |");
            Console.WriteLine("|________________|________________|_________________|\n");
        }

        public void PrintLocation()
        {
            Console.WriteLine("\nAREA " + _zoomedLocation);
            Console.WriteLine("_______________________________________________\n");
            Services.ScrollText("ENEMIES:");
            if (GetUnitsAtLocation(_zoomedLocation, false).Count > 0)
            {
                foreach (Unit unit in GetUnitsAtLocation(_zoomedLocation, false))
                {
                    if (unit != GetUnitsAtLocation(_zoomedLocation, false)[GetUnitsAtLocation(_zoomedLocation, false).Count - 1])
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ") - ");
                    }
                    else
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")\n");
                    }
                }
            }
            else
            {
                Services.ScrollText("There are no enemy units here\n");
            }
            

            Services.ScrollText("ALLIES:");
            if (GetUnitsAtLocation(_zoomedLocation, true).Count > 0)
            {
                foreach (Unit unit in GetUnitsAtLocation(_zoomedLocation, true))
                {
                    if (unit != GetUnitsAtLocation(_zoomedLocation, true)[GetUnitsAtLocation(_zoomedLocation, true).Count - 1])
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ") - ");
                    }
                    else
                    {
                        Console.Write(unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")\n");
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
            if (!_gameOver)
            {
                if (!_zoomed)
                {
                    HelpZoomedOut();

                    CheckAvailableMoves();
                    Console.Write("\n> ");
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
                            else
                            {
                                Services.ScrollText("Invalid input. Try Again.");
                                Listen();
                            }
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try Again.");
                            Listen();
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

                    CheckAvailableMoves();
                    Console.Write("\n> ");
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
                            else
                            {
                                Services.ScrollText("Invalid input. Try Again.");
                                Listen();
                            }
                        }
                        else if (cmd == "out")
                        {
                            ZoomOut();
                            Listen();
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try Again.");
                            Listen();
                        }
                    }
                    else
                    {
                        switch (cmd)
                        {
                            case "count":
                                Services.ScrollText("Ally units: " + GetUnitsAtLocation(_zoomedLocation, true).Count +
                                    "\nEnemy units: " + GetUnitsAtLocation(_zoomedLocation, false).Count + "\n");
                                Listen();
                                break;
                            case "map":
                                PrintLocation();
                                Listen();
                                break;
                            case "end turn":
                                EnemyTurn();
                                break;
                            case "units":
                                UseUnits();
                                break;
                            default:
                                Services.ScrollText("Invalid input. Try again.", 500);
                                Listen();
                                break;
                        }
                    }
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
            Console.Write("\n> ");
            string cmd = Console.ReadLine().ToLower();

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
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    Shop();
                }
            }
            else if (cmd == "back" || cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n", 500);
                PrintMap();
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
            Console.Write("\n> ");
            string cmd = Console.ReadLine().ToLower();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopInfantry.Length)
                {
                    Purchase(1, shopInfantry[decision - 1]);
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    ShopInfantry();
                }

            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n", 500);
                PrintMap();
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
            Console.Write("\n> ");
            string cmd = Console.ReadLine().ToLower();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopArcher.Length)
                {
                    Purchase(2, shopArcher[decision - 1]);
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    ShopArcher();
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n", 500);
                PrintMap();
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
            Console.Write("\n> ");
            string cmd = Console.ReadLine().ToLower();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopCavalier.Length)
                {
                    Purchase(3, shopCavalier[decision - 1]);
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    ShopCavalier();
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n", 500);
                PrintMap();
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
            Console.Write("\n> ");
            string cmd = Console.ReadLine().ToLower();

            if (Int32.TryParse(cmd, out decision))
            {
                if (decision >= 1 && decision <= shopOther.Length)
                {
                    Purchase(4, shopOther[decision - 1]);
                }
            }
            else if (cmd == "exit")
            {
                Services.ScrollText("Exiting shop. . .\n", 500);
                PrintMap();
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
                        case "Giant Shield":
                            if (_playerCurrency >= priceOther[Array.IndexOf(shopOther, item)])
                            {
                                _playerCurrency -= priceOther[Array.IndexOf(shopOther, item)];
                                Services.ScrollText("You purchased a new giant shield.", 300);
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
            Console.Write("\n> ");
            string answer = Console.ReadLine().ToLower();

            if(Int32.TryParse(answer, out decision))
            {
                if (decision >= 1 && decision <= 3)
                {
                    allyUnits.Add(new Unit(type, decision));
                    Shop();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    PlaceUnit(type);
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
            
            foreach (Unit unit in allyUnits)
            {
                if (unit.Type == 1)
                {
                    tempUnitList.Add(unit);
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) + 
                        ") Infantry A:" + unit.Attack + " H:" + unit.Health + " Location: " + unit.Location);
                }

                int decision;
                Console.Write("\n> ");
                string answer = Console.ReadLine().ToLower();

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

            foreach (Unit unit in allyUnits)
            {
                if (unit.Type == 2)
                {
                    tempUnitList.Add(unit);
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) +
                        ") Archer A:" + unit.Attack + " H:" + unit.Health + " Location: " + unit.Location);
                }

                int decision;
                Console.Write("\n> ");
                string answer = Console.ReadLine().ToLower();

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

            foreach (Unit unit in allyUnits)
            {
                if (unit.Type == 3)
                {
                    tempUnitList.Add(unit);
                }
            }
            Console.WriteLine();

            if (tempUnitList.Count > 0)
            {
                Services.ScrollText("Select the unit you would like to upgrade:\n");

                foreach (Unit unit in tempUnitList)
                {
                    Services.ScrollText((tempUnitList.IndexOf(unit) + 1) +
                        ") Cavalier A:" + unit.Attack + " H:" + unit.Health + " Location: " + unit.Location);
                }

                int decision;
                Console.Write("\n> ");
                string answer = Console.ReadLine().ToLower();

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

        public void UseUnits()
        {
            List<Unit> tempUnitList = new List<Unit>();

            foreach (Unit unit in allyUnits)
            {
                if (unit.Location == _zoomedLocation && unit.Sleeping == false)
                {
                    tempUnitList.Add(unit);
                }
            }

            if (tempUnitList.Count > 0)
            {
                Console.WriteLine();
                Services.FastScrollText("(You can type 'back' at any time to leave the unit control session)\n");
                Services.FastScrollText("Available Units: (or type 'all' to control them all at once)");

                foreach (Unit unit in tempUnitList)
                {
                    Services.FastScrollText((tempUnitList.IndexOf(unit) + 1).ToString() + ") " + unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")");
                }

                int decision;
                Console.Write("\n> ");
                string answer = Console.ReadLine().ToLower();

                if (Int32.TryParse(answer, out decision))
                {
                    if (decision >= 1 && decision <= tempUnitList.Count)
                    {
                        _unitSelected = true;
                        selectedUnit = tempUnitList[decision - 1];

                        Console.WriteLine();
                        Services.ScrollText("What would you like to do with this unit?");

                        foreach(string option in unitCommands)
                        {
                            Services.FastScrollText((Array.IndexOf(unitCommands, option) + 1).ToString() + ") " + option);
                        }

                        int cmd;
                        Console.Write("\n> ");
                        string input = Console.ReadLine().ToLower();

                        if (Int32.TryParse(input, out cmd))
                        {
                            if (cmd >= 1 && cmd <= unitCommands.Length)
                            {
                                switch (unitCommands[cmd - 1])
                                {
                                    case "Move":
                                        UnitMove();
                                        break;
                                    case "Attack":
                                        UnitAttack();
                                        break;
                                    default:
                                        Services.ScrollText("Invalid input. Try again.");
                                        _unitSelected = false;
                                        UseUnits();
                                        break;
                                }
                            }
                            else
                            {
                                Services.ScrollText("Invalid input. Try again.");
                                _unitSelected = false;
                                UseUnits();
                            }
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try again.");
                            _unitSelected = false;
                            UseUnits();
                        }
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                        _unitSelected = false;
                        UseUnits();
                    }
                }
                else if (answer == "back" || answer == "exit")
                {
                    PrintLocation();
                    Listen();
                }
                else if (answer == "all")
                {
                    Console.WriteLine();
                    Services.ScrollText("What would you like to do with your units?");

                    foreach (string option in unitCommands)
                    {
                        Services.FastScrollText((Array.IndexOf(unitCommands, option) + 1).ToString() + ") " + option);
                    }

                    int cmd;
                    Console.Write("\n> ");
                    string input = Console.ReadLine().ToLower();

                    if (Int32.TryParse(input, out cmd))
                    {
                        if (cmd >= 1 && cmd <= unitCommands.Length)
                        {
                            switch (unitCommands[cmd - 1])
                            {
                                case "Move":
                                    MoveAll();
                                    break;
                                case "Attack":
                                    AttackAll();
                                    break;
                                default:
                                    Services.ScrollText("Invalid input. Try again.");
                                    UseUnits();
                                    break;
                            }
                        }
                        else
                        {
                            Services.ScrollText("Invalid input. Try again.");
                            UseUnits();
                        }
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                        UseUnits();
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    _unitSelected = false;
                    UseUnits();
                }
            }
            else
            {
                Services.ScrollText("There are no units available to use this turn in this location!", 500);
                PrintLocation();
                Listen();
            }
        }

        public void UnitMove()
        {
            int[] moveableLocations = new int[0];

            switch (selectedUnit.Location)
            {
                case 1:
                    moveableLocations = moveableLocations1;
                    break;
                case 2:
                    moveableLocations = moveableLocations2;
                    break;
                case 3:
                    moveableLocations = moveableLocations3;
                    break;
                case 4:
                    moveableLocations = moveableLocations4;
                    break;
                case 5:
                    moveableLocations = moveableLocations5;
                    break;
                case 6:
                    moveableLocations = moveableLocations6;
                    break;
                case 7:
                    moveableLocations = moveableLocations7;
                    break;
                case 8:
                    moveableLocations = moveableLocations8;
                    break;
                case 9:
                    moveableLocations = moveableLocations9;
                    break;
                case 10:
                    moveableLocations = moveableLocations10;
                    break;
                case 11:
                    moveableLocations = moveableLocations11;
                    break;
                case 12:
                    moveableLocations = moveableLocations12;
                    break;
                default:
                    Services.ScrollText("ERROR: The unit's location is invalid. .returning to main view");
                    Listen();
                    break;
            }

            Services.FastScrollText("You can type 'back' at any time to cancel this move)");
            Services.FastScrollText("Where would you like to move?");

            PrintMoveOptions(moveableLocations);

            int cmd;
            Console.Write("\n> ");
            string input = Console.ReadLine().ToLower();

            if (Int32.TryParse(input, out cmd))
            {
                if (moveableLocations.Contains(cmd))
                {
                    selectedUnit.Location = cmd;
                    selectedUnit.Sleeping = true;
                    _unitSelected = false;
                    if (!_gameOver)
                    {
                        if (CheckLocation(true))
                        {
                            PrintLocation();
                            Listen();
                        }
                        else
                        {
                            UseUnits();
                        }
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    UnitMove();
                }
            }
            else if (input == "back" || input == "exit")
            {
                _unitSelected = false;
                UseUnits();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                UnitMove();
            }
        }

        public void PrintMoveOptions(int[] options)
        {
            foreach (int num in options)
            {
                Console.WriteLine(num + ") " + num);
            }
        }

        public void UnitAttack()
        {
            List<Unit> tempEnemyUnitList = new List<Unit>();

            foreach (Unit unit in enemyUnits)
            {
                if (unit.Location == _zoomedLocation)
                {
                    tempEnemyUnitList.Add(unit);
                }
            }

            if (tempEnemyUnitList.Count > 0)
            {
                Services.FastScrollText("You can type 'back' at any point to return to the previous menu)");
                Services.ScrollText("You are attacking with: " + selectedUnit.TypeToString + " (" + selectedUnit.Attack + "/" + selectedUnit.Health + ")");
                Services.FastScrollText("Select which unit you'd like to attack:");

                foreach (Unit unit in tempEnemyUnitList)
                {
                    Services.FastScrollText((tempEnemyUnitList.IndexOf(unit) + 1).ToString() + ") " + unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")");
                }

                int cmd;
                Console.Write("\n> ");
                string input = Console.ReadLine().ToLower();

                if (Int32.TryParse(input, out cmd))
                {
                    if (cmd >= 1 && cmd <= tempEnemyUnitList.Count)
                    {
                        Services.ScrollText(selectedUnit.TypeToString + " attacks " + tempEnemyUnitList[cmd - 1].TypeToString + "!");
                        Attack(selectedUnit, tempEnemyUnitList[cmd - 1]);
                        _unitSelected = false;
                        if (!_gameOver)
                        {
                            if (CheckLocation(true))
                            {
                                PrintLocation();
                                Listen();
                            }
                            else
                            {
                                UseUnits();
                            }
                        }
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                        UnitAttack();
                    }
                }
                else if (input == "back")
                {
                    _unitSelected = false;
                    UseUnits();
                }
                else if (input == "exit")
                {
                    _unitSelected = false;
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    UnitAttack();
                }
            }
            else
            {
                Services.ScrollText("There are no enemies to attack in this location!");
                _unitSelected = false;
                UseUnits();
            }
        }

        public void MoveAll()
        {
            int[] moveableLocations = new int[0];

            switch (_zoomedLocation)
            {
                case 1:
                    moveableLocations = moveableLocations1;
                    break;
                case 2:
                    moveableLocations = moveableLocations2;
                    break;
                case 3:
                    moveableLocations = moveableLocations3;
                    break;
                case 4:
                    moveableLocations = moveableLocations4;
                    break;
                case 5:
                    moveableLocations = moveableLocations5;
                    break;
                case 6:
                    moveableLocations = moveableLocations6;
                    break;
                case 7:
                    moveableLocations = moveableLocations7;
                    break;
                case 8:
                    moveableLocations = moveableLocations8;
                    break;
                case 9:
                    moveableLocations = moveableLocations9;
                    break;
                case 10:
                    moveableLocations = moveableLocations10;
                    break;
                case 11:
                    moveableLocations = moveableLocations11;
                    break;
                case 12:
                    moveableLocations = moveableLocations12;
                    break;
                default:
                    Services.ScrollText("ERROR: The units' location is invalid. .returning to main view");
                    Listen();
                    break;
            }

            Services.FastScrollText("You can type 'back' at any time to cancel this move)");
            Services.FastScrollText("Where would you like to move?");

            PrintMoveOptions(moveableLocations);

            int cmd;
            Console.Write("\n> ");
            string input = Console.ReadLine().ToLower();

            if (Int32.TryParse(input, out cmd))
            {
                if (moveableLocations.Contains(cmd))
                {
                    foreach (Unit unit in allyUnits)
                    {
                        if (unit.Location == _zoomedLocation && unit.Sleeping == false)
                        {
                            unit.Location = cmd;
                            unit.Sleeping = true;
                        }
                    }
                    if (!_gameOver)
                    {
                        if (CheckLocation(true))
                        {
                            PrintLocation();
                            Listen();
                        }
                        else
                        {
                            UseUnits();
                        }
                    }
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    MoveAll();
                }
            }
            else if (input == "back" || input == "exit")
            {
                UseUnits();
            }
            else
            {
                Services.ScrollText("Invalid input. Try again.");
                MoveAll();
            }
        }

        public void AttackAll()
        {
            List<Unit> tempEnemyUnitList = new List<Unit>();

            foreach (Unit unit in enemyUnits)
            {
                if (unit.Location == _zoomedLocation)
                {
                    tempEnemyUnitList.Add(unit);
                }
            }

            if (tempEnemyUnitList.Count > 0)
            {
                Services.FastScrollText("You can type 'back' at any point to return to the previous menu)");
                Services.ScrollText("You are attacking with all units");
                Services.FastScrollText("Select which unit you'd like to attack:");

                foreach (Unit unit in tempEnemyUnitList)
                {
                    Services.FastScrollText((tempEnemyUnitList.IndexOf(unit) + 1).ToString() + ") " + unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")");
                }

                int cmd;
                Console.Write("\n> ");
                string input = Console.ReadLine().ToLower();

                if (Int32.TryParse(input, out cmd))
                {
                    if (cmd >= 1 && cmd <= tempEnemyUnitList.Count)
                    {
                        Services.ScrollText("All units attack " + tempEnemyUnitList[cmd - 1].TypeToString + "!");

                        foreach (Unit unit in allyUnits)
                        {
                            if (unit.Location == _zoomedLocation && unit.Sleeping == false)
                            {
                                Attack(unit, tempEnemyUnitList[cmd - 1]);
                            }
                        }
                        if (!_gameOver)
                        {
                            if (CheckLocation(true))
                            {
                                PrintLocation();
                                Listen();
                            }
                            else
                            {
                                UseUnits();
                            }
                        }
                    }
                    else
                    {
                        Services.ScrollText("Invalid input. Try again.");
                        AttackAll();
                    }
                }
                else if (input == "back")
                {
                    UseUnits();
                }
                else if (input == "exit")
                {
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    AttackAll();
                }
            }
            else
            {
                Services.ScrollText("There are no enemies to attack in this location!");
                UseUnits();
            }
        }

        public bool CheckLocation(bool ally)
        {
            bool clear = true;
            if (ally)
            {
                foreach (Unit unit in allyUnits)
                {
                    if (unit.Location == _zoomedLocation)
                    {
                        if (unit.Sleeping == false)
                        {
                            clear = false;
                        }
                    }
                }
            }
            return clear;
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
            Services.FastScrollText("zoom *1-12*: take a closer look at a specific location - this is the view where units are controlled");
            Services.FastScrollText("zoom out: go back to the overhead view of the whole map");
            Services.FastScrollText("end turn: ends your turn\n");
        }

        public void Zoom(int num)
        {
            _zoomedLocation = num;
            _zoomed = true;
            PrintLocation();
            Listen();
        }

        public void ZoomOut()
        {
            _zoomedLocation = 0;
            _zoomed = false;
            PrintMap();
        }

        public void Attack(Unit attacker, Unit defender)
        {
            attacker.Health -= defender.Attack;
            defender.Health -= attacker.Attack;
            attacker.Sleeping = true;

            CheckDeath(attacker, defender);
        }

        public void CheckDeath(Unit attacker, Unit defender)
        {
            if (attacker.Health < 1)
            {
                if (allyUnits.Contains(attacker))
                {
                    Services.ScrollText("Your " + attacker.TypeToString + " dies!");
                    allyUnits.Remove(attacker);
                }
                else
                {
                    Services.ScrollText("The enemy " + attacker.TypeToString + " dies!");
                    enemyUnits.Remove(attacker);
                }
            }
            if (defender.Health < 1)
            {
                if (allyUnits.Contains(defender))
                {
                    Services.ScrollText("Your " + defender.TypeToString + " dies!");
                    allyUnits.Remove(defender);
                }
                else
                {
                    Services.ScrollText("The enemy " + defender.TypeToString + " dies!");
                    enemyUnits.Remove(defender);
                }
            }

            if (CheckLose())
            {
                Services.ScrollText("You lost!");
            }
            else if (CheckWin())
            {
                Services.ScrollText("You win!");
            }
        }

        public bool CheckWin()
        {
            if (enemyUnits.Count < 1)
            {
                _player.LevelCompleted = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckLose()
        {
            if (allyUnits.Count < 1)
            {
                _gameOver = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CalculatePay()
        {
            int unitCompensation = CountAllyUnits() * 12;
            int locationCompensation = 0;

            for (int i = 1; i <= 12; i++)
            {
                int ally = 0;
                int enemy = 0;
                foreach (Unit unit in allyUnits)
                {
                    if (unit.Location == i)
                    {
                        ally++;
                    }
                }
                foreach (Unit unit in enemyUnits)
                {
                    if (unit.Location == i)
                    {
                        enemy++;
                    }
                }
                if (ally > enemy)
                {
                    locationCompensation++;
                }
            }

            locationCompensation = locationCompensation * 60;

            return locationCompensation + unitCompensation + 600;
        }

        public int EnemyCalculatePay()
        {
            int unitCompensation = CountEnemyUnits() * 12;
            int locationCompensation = 0;

            for (int i = 1; i <= 12; i++)
            {
                int ally = 0;
                int enemy = 0;
                foreach (Unit unit in allyUnits)
                {
                    if (unit.Location == i)
                    {
                        ally++;
                    }
                }
                foreach (Unit unit in enemyUnits)
                {
                    if (unit.Location == i)
                    {
                        enemy++;
                    }
                }
                if (ally < enemy)
                {
                    locationCompensation++;
                }
            }

            locationCompensation = locationCompensation * 60;

            return locationCompensation + unitCompensation + 900;
        }

        public List<Unit> GetUnitsAtLocation(int location, bool allies)
        {
            List<Unit> tempUnitList = new List<Unit>();

            if (allies)
            {
                foreach (Unit unit in allyUnits)
                {
                    if (unit.Location == location)
                    {
                        tempUnitList.Add(unit);
                    }
                }
            }
            else
            {
                foreach (Unit unit in enemyUnits)
                {
                    if (unit.Location == location)
                    {
                        tempUnitList.Add(unit);
                    }
                }
            }
            

            return tempUnitList;
        }

        public bool CheckForTargets(Unit enemyUnit)
        {
            bool target = false;
            foreach (Unit unit in allyUnits)
            {
                if (unit.Location == enemyUnit.Location)
                {
                    target = true;
                }
            }
            return target;
        }

        public void EnemyAttack(Unit enemyUnit)
        {
            List<Unit> targetList = new List<Unit>();

            foreach (Unit unit in allyUnits)
            {
                if (unit.Location == enemyUnit.Location)
                {
                    targetList.Add(unit);
                }
            }

            Random rand = new Random();
            int index = rand.Next(targetList.Count);
            Attack(enemyUnit, targetList[index]);
            Services.ScrollText("The enemy's " + enemyUnit.TypeToString + " attacks your " + targetList[index].TypeToString + "!", 1200);
        }

        public void EnemyMove(Unit enemyUnit)
        {
            int[] moveableLocations = new int[0];

            switch (enemyUnit.Location)
            {
                case 1:
                    moveableLocations = moveableLocations1;
                    break;
                case 2:
                    moveableLocations = moveableLocations2;
                    break;
                case 3:
                    moveableLocations = moveableLocations3;
                    break;
                case 4:
                    moveableLocations = moveableLocations4;
                    break;
                case 5:
                    moveableLocations = moveableLocations5;
                    break;
                case 6:
                    moveableLocations = moveableLocations6;
                    break;
                case 7:
                    moveableLocations = moveableLocations7;
                    break;
                case 8:
                    moveableLocations = moveableLocations8;
                    break;
                case 9:
                    moveableLocations = moveableLocations9;
                    break;
                case 10:
                    moveableLocations = moveableLocations10;
                    break;
                case 11:
                    moveableLocations = moveableLocations11;
                    break;
                case 12:
                    moveableLocations = moveableLocations12;
                    break;
                default:
                    moveableLocations = moveableLocations12;
                    break;
            }

            Random rand = new Random();
            int index = rand.Next(moveableLocations.Length);

            enemyUnit.Location = moveableLocations[index];
            if (enemyUnit.Type == 3)
            {
                Services.ScrollText("The enemy moves a " + enemyUnit.TypeToString + " to the " + enemyUnit.Location.ToString() + " sqaure.", 1200);
            }
            else
            {
                Services.ScrollText("The enemy moves an " + enemyUnit.TypeToString + " to the " + enemyUnit.Location.ToString() + " sqaure.", 1200);
            }
            
        }

        public void EnemyShop()
        {
            while (_enemyCurrency >= 250)
            {
                Random rand = new Random();

                EnemyPurchase(rand.Next(3));
            }
        }

        public void EnemyPurchase(int type)
        {
            switch (type)
            {
                case 0:
                    if (_enemyCurrency >= 250)
                    {
                        _enemyCurrency -= 250;
                        Services.ScrollText("The enemy purchases an infantry.", 1200);
                        EnemyPlaceUnit(type);
                    }
                    break;
                case 1:
                    if (_enemyCurrency >= 350)
                    {
                        _enemyCurrency -= 350;
                        Services.ScrollText("The enemy purchases an archer.", 1200);
                        EnemyPlaceUnit(type);
                    }
                    break;
                case 2:
                    if (_enemyCurrency >= 700)
                    {
                        _enemyCurrency -= 700;
                        Services.ScrollText("The enemy purchases a cavalier.", 1200);
                        EnemyPlaceUnit(type);
                    }
                    break;
            }
        }

        public void EnemyPlaceUnit(int type)
        {
            Random rand = new Random();
            switch (rand.Next(3))
            {
                case 0:
                    switch (type)
                    {
                        case 0:
                            enemyUnits.Add(new Unit(1, 10));
                            break;
                        case 1:
                            enemyUnits.Add(new Unit(2, 10));
                            break;
                        case 2:
                            enemyUnits.Add(new Unit(3, 10));
                            break;
                    }
                    Services.ScrollText("It's placed in square 10.", 600);
                    break;
                case 1:
                    switch (type)
                    {
                        case 0:
                            enemyUnits.Add(new Unit(1, 11));
                            break;
                        case 1:
                            enemyUnits.Add(new Unit(2, 11));
                            break;
                        case 2:
                            enemyUnits.Add(new Unit(3, 11));
                            break;
                    }
                    Services.ScrollText("It's placed in square 11.", 600);
                    break;
                case 2:
                    switch (type)
                    {
                        case 0:
                            enemyUnits.Add(new Unit(1, 12));
                            break;
                        case 1:
                            enemyUnits.Add(new Unit(2, 12));
                            break;
                        case 2:
                            enemyUnits.Add(new Unit(3, 12));
                            break;
                    }
                    Services.ScrollText("It's placed in square 12.", 600);
                    break;
            }
        }

        public void CheckAvailableMoves()
        {
            List<Unit> tempUnitList = new List<Unit>();
            bool turnOver = true;

            foreach (Unit unit in allyUnits)
            {
                if (unit.Sleeping == false)
                {
                    turnOver = false;
                }
            }

            if (_playerCurrency >= 250)
            {
                turnOver = false;
            }

            if (turnOver)
            {
                Services.FastScrollText("It looks like you have no more available moves this turn. You should end your turn!", 200);
            }
        }

        public int CountAllyUnits()
        {
            return allyUnits.Count;
        }

        public int CountEnemyUnits()
        {
            return enemyUnits.Count;
        }
    }
}
