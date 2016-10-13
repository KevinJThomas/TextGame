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
    //4) Give AI currency advantage since it will be stupid?
    //5) Give AI different unit types or use same as player?
    //6) Win condition when one player no longer controls any units, or should there be a target 'king' unit to kill like chess?
    //7) Enemy turn instantly execute to make faster gameplay or should it be slower so the player can keep up with what's happening?
    //TODO:
    //1) Attacking and moving units (1 action per turn using Sleeping prop on units) - add 'all units' command where you can do the same command with all units in the
    //   location at once
    //2) EnemyTurn() - make enemies always attack if available else randomly move.. could make a predefined strat for them to always use
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

        public War(Player player) : base(player)
        {
            _player = player;
            musicThread.Start();

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
                foreach (Unit unit in allyUnits)
                {
                    unit.Sleeping = false;
                }
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
            Services.ScrollText("Enemy playing turn. . .", 2000);
            StartTurn();
        }

        public void PrintMap()
        {
            Console.WriteLine("E = Enemy, A = Ally\n");
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
            Console.WriteLine("AREA " + _zoomedLocation);
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
                    case "zoom":
                        ZoomOut();
                        Listen();
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
                Services.FastScrollText("(You can type 'back' at any time to leave the unit control session)");
                Services.FastScrollText("Available Units:");
                foreach (Unit unit in tempUnitList)
                {
                    Services.FastScrollText((tempUnitList.IndexOf(unit) + 1).ToString() + ") " + unit.TypeToString + " (" + unit.Attack + "/" + unit.Health + ")");
                }

                int decision;
                Console.Write("> ");
                string answer = Console.ReadLine();

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
                        Console.Write("> ");
                        string input = Console.ReadLine();

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
                        UseUnits();
                    }
                }
                else if (answer == "back" || answer == "exit")
                {
                    PrintLocation();
                    Listen();
                }
                else
                {
                    Services.ScrollText("Invalid input. Try again.");
                    UseUnits();
                }
            }
            else
            {
                Services.ScrollText("There are no units available to use this turn in this location!", 500);
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
            Console.Write("> ");
            string input = Console.ReadLine();

            if (Int32.TryParse(input, out cmd))
            {
                if (moveableLocations.Contains(cmd))
                {
                    selectedUnit.Location = cmd;
                    selectedUnit.Sleeping = true;
                    _unitSelected = false;
                    UseUnits();
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
            //TODO
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

        public int CalculatePay()
        {
            int unitCompensation = CountAllyUnits() * 10;
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

            locationCompensation = locationCompensation * 50;

            return locationCompensation + unitCompensation + 200;
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
