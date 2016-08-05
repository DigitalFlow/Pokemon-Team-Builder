﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	/// <summary>
	/// Most often used abilities
	/// </summary>
	public class TokuseiInfo : IRankable
    {
        public int Ranking { get; set; }
        public double UsageRate { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
    }
}
