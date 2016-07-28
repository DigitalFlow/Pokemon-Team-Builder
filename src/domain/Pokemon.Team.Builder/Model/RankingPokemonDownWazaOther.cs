using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonDownWazaOther
    {
        public int ranking { get; set; }
        public int typeId { get; set; }
        public double usageRate { get; set; }
        public object wazaName { get; set; }
        public int sequenceNumber { get; set; }
    }
}
