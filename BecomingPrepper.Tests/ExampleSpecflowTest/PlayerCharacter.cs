﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BecomingPrepper.Tests.ExampleSpecflowTest
{
    public class PlayerCharacter
    {
        public void Hit(int damage)
        {
            var raceSpecificDamageResistance = 0;

            if (Race == "Elf")
            {
                raceSpecificDamageResistance = 20;
            }

            var totalDamageTaken = Math.Max(damage - raceSpecificDamageResistance - DamageResistance, 0);

            Health -= totalDamageTaken;
            if (Health <= 0)
            {
                IsDead = true;
            }
        }

        public void CastHealingSpell()
        {
            if (CharacterType == CharacterType.Healer)
            {
                Health = 100;
            }
            else
            {
                Health += 10;
            }
        }

        public void ReadHealthScroll()
        {
            var daysSinceLastSleep = DateTime.Now.Subtract(LastSleepTime).Days;

            if (daysSinceLastSleep <= 2)
            {
                Health = 100;
            }
        }

        public void UseMagicalItem(string itemName)
        {
            int powerReduction = 10;

            if (Race == "Elf")
            {
                powerReduction = 0;
            }

            var itemToReduce = MagicalItems.First(item => item.Name == itemName);
            itemToReduce.Power -= powerReduction;
        }

        public int Health { get; private set; } = 100;
        public bool IsDead { get; private set; }
        public int DamageResistance { get; set; }
        public string Race { get; set; }
        public int MagicalPower
        {
            get { return MagicalItems.Sum(x => x.Power); }
        }
        public List<MagicalItem> MagicalItems { get; set; } = new List<MagicalItem>();
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public CharacterType CharacterType { get; set; }
        public DateTime LastSleepTime { get; set; }

        public int WeaponsValue
        {
            get { return Weapons.Sum(w => w.Value); }
        }
    }
}