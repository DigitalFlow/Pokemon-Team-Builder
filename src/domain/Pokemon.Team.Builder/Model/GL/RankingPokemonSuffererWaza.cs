﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	public class RankingPokemonSuffererWaza : IRankable
    {
        public int Ranking { get; set; }
        public int TypeId { get; set; }
        public double UsageRate { get; set; }
        public string WazaName { get; set; }
        public int SequenceNumber { get; set; }
    }
}
