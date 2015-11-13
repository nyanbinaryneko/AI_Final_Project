﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Final_Project
{
    class Team
    {
        private const int SIZE = 6;
        private List<Pokemon> team;
        private Logger log;
        private Random random;
        private int damageTaken;
        private int damageDone;

        public Team()
        {
            team = new List<Pokemon>();
            Initialize();
        }

        private void Initialize()
        {
            log = ServiceRegistry.GetInstance().GetLog();
            random = ServiceRegistry.GetInstance().GetRandom();
        }

        public double Fitness //fitness is KDR here
        {
            get
            {
                return (double) damageDone / (double)(damageDone + damageTaken);
            }
        }

        public void Generate()
        {
            List<PokemonFactory> pokemonFactories = new PokemonFactoryRepository().All();
            for (int i = 0; i < SIZE; i++)
            {
                Pokemon pokemon = pokemonFactories[random.Next(0, pokemonFactories.Count)].Generate();
                team.Add(pokemon);
            }
        }

        public void Reset()
        {
            damageDone = 0;
            damageTaken = 0;
            foreach (Pokemon pokemon in team)
            {
                pokemon.Heal();
            }
        }

        public Pokemon SelectFighter(Pokemon opponent)
        {
            foreach (Pokemon poke in team.Where(p => !p.Dead))
            {
                if (poke.Type_1_Chart[opponent.Type_1] * poke.Type_1_Chart[opponent.Type_2] >= 2.0 || poke.Type_2_Chart[opponent.Type_1] * poke.Type_2_Chart[opponent.Type_2] >= 2.0)
                {
                    return poke;
                }
            }
            return team.Where(p => !p.Dead).First();
        }

        public List<Pokemon> GetTeam
        {
            get
            {
                return team;
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (Pokemon poke in team)
            {
                str += poke;
            }
            return str;
        }
    }
}
