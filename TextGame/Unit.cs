using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    public class Unit
    {
        public int Attack { get; set; }
        public int Health { get; set; }
        public int Type { get; set; } //1: Infantry 2: Archer 3: Cavalier 4: Arrow Shield -- Use enum?
        public string TypeToString { get; set; }
        public bool Sleeping { get; set; }
        public int Location { get; set; }

        bool _hasShield = false;
        bool _hasArmor = false;

        public Unit (int type, int location)
        {
            Type = type;
            Location = location;

            switch (Type)
            {
                case 1:
                    Attack = 2;
                    Health = 10;
                    TypeToString = "Infantry";
                    break;
                case 2:
                    Attack = 4;
                    Health = 6;
                    TypeToString = "Archer";
                    break;
                case 3:
                    Attack = 3;
                    Health = 20;
                    TypeToString = "Cavalier";
                    break;
                case 4:
                    Attack = 0;
                    Health = 50;
                    TypeToString = "Arrow Shield";
                    break;
                default:
                    Attack = 1;
                    Health = 1;
                    break;
            }
            Sleeping = true;
        }

        public bool HasShield()
        {
            if (_hasShield)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasArmor()
        {
            if (_hasArmor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddSword()
        {
            if (Type == 1)
            {
                Attack = 4;
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add sword to non infantry is prohibited.");
            }
        }

        public void AddAxe()
        {
            if (Type == 1 && Attack == 4)
            {
                Attack = 6;
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add axe to non infantry is prohibited.");
            }
        }

        public void AddShield()
        {
            if (Type == 1 || Type == 3)
            {
                if (!_hasShield)
                {
                    Health += 4;
                    _hasShield = true;
                }
                else
                {
                    Services.ScrollText("That unit already has a shield!");
                }
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add shield to non infantry/cavalier is prohibited.");
            }
        }

        public void AddCrossbow()
        {
            if (Type == 2)
            {
                Attack = 7;
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add crossbow to non archer is prohibited.");
            }
        }

        public void AddJavelin()
        {
            if (Type == 3)
            {
                Attack = 6;
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add javelin to non cavalier is prohibited.");
            }
        }

        public void AddLeatherArmor()
        {
            if (Type == 2)
            {
                if (_hasArmor)
                { 
                    Health += 3;
                    _hasArmor = true;
                }
                else
                {
                    Services.ScrollText("That unit already has armor!");
                }
            }
            else
            {
                Services.ScrollText("ERROR: Attempting to add armor to non archer is prohibited.");
            }
        }
    }
}
