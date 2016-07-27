using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model
{
    public class RetrievePokemonUsageResponse
    {
        public class RankingPokemonSuffererWaza
        {
            public int ranking { get; set; }
            public int typeId { get; set; }
            public double usageRate { get; set; }
            public string wazaName { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class RankingPokemonSufferer
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

        public class RankingPokemonIn
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

        public class TokuseiInfo
        {
            public int ranking { get; set; }
            public double usageRate { get; set; }
            public string name { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class SeikakuInfo
        {
            public int ranking { get; set; }
            public double usageRate { get; set; }
            public string name { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class ItemInfo
        {
            public int ranking { get; set; }
            public double usageRate { get; set; }
            public string name { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class WazaInfo
        {
            public int ranking { get; set; }
            public int typeId { get; set; }
            public double usageRate { get; set; }
            public string name { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class RankingPokemonTrend
        {
            public List<TokuseiInfo> tokuseiInfo { get; set; }
            public List<SeikakuInfo> seikakuInfo { get; set; }
            public List<ItemInfo> itemInfo { get; set; }
            public List<WazaInfo> wazaInfo { get; set; }
        }

        public class RankingPokemonInfo
        {
            public int monsno { get; set; }
            public string formNo { get; set; }
            public string pokemonId { get; set; }
            public int ranking { get; set; }
            public string typeName1 { get; set; }
            public string typeName2 { get; set; }
            public string weight { get; set; }
            public int typeId1 { get; set; }
            public int typeId2 { get; set; }
            public object formName { get; set; }
            public string name { get; set; }
            public int sequenceNumber { get; set; }
            public string height { get; set; }
        }

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

        public class RankingPokemonDownWazaOther
        {
            public int ranking { get; set; }
            public int typeId { get; set; }
            public double usageRate { get; set; }
            public object wazaName { get; set; }
            public int sequenceNumber { get; set; }
        }

        public class RankingPokemonDownWaza
        {
            public int ranking { get; set; }
            public int typeId { get; set; }
            public double usageRate { get; set; }
            public string wazaName { get; set; }
            public int sequenceNumber { get; set; }
        }

        public string status_code { get; set; }
        public List<RankingPokemonSuffererWaza> rankingPokemonSuffererWaza { get; set; }
        public List<RankingPokemonSufferer> rankingPokemonSufferer { get; set; }
        public List<RankingPokemonIn> rankingPokemonIn { get; set; }
        public string beforePokemonId { get; set; }
        public RankingPokemonTrend rankingPokemonTrend { get; set; }
        public RankingPokemonInfo rankingPokemonInfo { get; set; }
        public List<RankingPokemonDown> rankingPokemonDown { get; set; }
        public RankingPokemonDownWazaOther rankingPokemonDownWazaOther { get; set; }
        public string nextPokemonId { get; set; }
        public List<RankingPokemonDownWaza> rankingPokemonDownWaza { get; set; }
        public string timezoneName { get; set; }
    }
}
