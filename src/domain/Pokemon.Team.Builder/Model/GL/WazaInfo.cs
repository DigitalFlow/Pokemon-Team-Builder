using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
	/// <summary>
	/// Most often used Moves
	/// </summary>
	public class WazaInfo : IMove
    {
        public int Ranking { get; set; }
        public int TypeId { get; set; }
        public double UsageRate { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
    }
}
