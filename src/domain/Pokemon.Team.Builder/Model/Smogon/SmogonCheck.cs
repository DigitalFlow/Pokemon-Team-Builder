﻿using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonCheck
    {
        public PokemonIdentifier Identifier
        {
            get
            {
                return new PokemonIdentifier(Name);
            }

            set
            {
                Name = value.Name;
            }
        }

        public string Name { get; set; }
        public int Ranking { get; set; }

        /// <summary>
        /// 
        /// Array has always three values if set:
        /// 
        /// Index 0: Number of times the matchup occurred (without U-Turn KOs or force-outs)
        /// Index 1: The fraction of times the counter got the KO or caused a switch
        /// Index 2: Standard deviation: sqrt(Index1*(1.0-Index1)/Index0)
        /// </summary>
        public List<float> Statistics { get; set; }
    }
}