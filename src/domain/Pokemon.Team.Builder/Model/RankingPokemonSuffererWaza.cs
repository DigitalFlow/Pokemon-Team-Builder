using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonSuffererWaza
    {
        public int ranking { get; set; }
        public int typeId { get; set; }
        public double usageRate { get; set; }
        public string wazaName { get; set; }
        public int sequenceNumber { get; set; }
    }
}
