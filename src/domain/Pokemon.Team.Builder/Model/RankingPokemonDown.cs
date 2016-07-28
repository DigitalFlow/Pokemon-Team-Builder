using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RankingPokemonDown
    {
        public int monsno { get; set; }
        public string formNo { get; set; }
        public string pokemonId { get; set; }
        public int ranking { get; set; }
        public int countBattleByForm { get; set; }
        public int battlingChangeFlg { get; set; }
        public string typeName1 { get; set; }
        public string typeName2 { get; set; }
        public int typeId1 { get; set; }
        public int typeId2 { get; set; }
        public object formName { get; set; }
        public string name { get; set; }
        public int sequenceNumber { get; set; }
    }
}
