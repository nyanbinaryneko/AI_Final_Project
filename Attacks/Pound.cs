﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Final_Project.Attacks
{
    class Pound : IAttack
    {
        Random random;
        Logger log;
        public Pound()
        {
            log = ServiceRegistry.GetInstance().GetLog();
            random = ServiceRegistry.GetInstance().GetRandom();
        }

        public int GetDamage(Pokemon attacker, Pokemon defender)
        {
            log.Log(attacker.Name + " used Pound on " + defender.Name);
            double damage = (2 * attacker.Level + 10) / 250.0;
            double modifier = random.NextDouble() * (1.0 - 0.85) + 0.85;
            if (random.Critical()) //determines critical hit!
            {
                log.Log("Critical Hit!");
                modifier *= 2;
            }
            if (random.FlipCoin()) //determines which type is hit with.
            {
                modifier *= TypeEffectivenessModifier(attacker.Type_1, defender);
            }
            else
            {
                modifier *= TypeEffectivenessModifier(attacker.Type_2, defender);
            }
            damage *= (double) attacker.AttackPower / (double) defender.Defense;
            damage *= 40;
            damage += 2;
            damage *= modifier;
            log.Log("Total damage: " + damage);
            if (damage == 0)
            {
                return 1;
            }
            return (int)(Math.Round(damage));
        }

        private double TypeEffectivenessModifier(int attacker_type, Pokemon defender)
        {
            double modifier = 1.0;
            SpecialAttack special = new SpecialAttack();
            modifier *= special.GetMultiplier(attacker_type, defender.Type_1);
            if (defender.Type_1 == defender.Type_2)
            {
                return modifier;
            }
            modifier *= special.GetMultiplier(attacker_type, defender.Type_2);
            log.Log("Type effectiveness modifier for " + attacker_type + ": " + modifier);
            return modifier;
        }
    }
}
