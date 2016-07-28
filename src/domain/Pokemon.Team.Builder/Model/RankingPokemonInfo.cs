using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonInfo
    {
        public int MonsNo { get; set; }
        public string FormNo { get; set; }
        public string PokemonId { get; set; }
        public int Ranking { get; set; }
        public string TypeName1 { get; set; }
        public string TypeName2 { get; set; }
        public string Weight { get; set; }
        public int TypeId1 { get; set; }
        public int TypeId2 { get; set; }
        public object FormName { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
        public string Height { get; set; }
    }
}
