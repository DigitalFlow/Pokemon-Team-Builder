using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonPokemonStats
    {
        public string Name { get; set; }
        public List<SmogonAbility> Abilities { get; set; }
        public List<SmogonCheck> ChecksAndCounters { get; set; }
        public List<SmogonHappiness> Happiness { get; set; }
        public List<SmogonItem> Items { get; set; }
        public List<SmogonMove> Moves { get; set; }
        public List<SmogonSpread> Spreads { get; set; }
        public List<SmogonTeamMate> TeamMates { get; set; }
        public int RawCount { get; set; }
        public float Usage { get; set; }
        public List<int> ViabilityCeiling { get; set; }
    }
}
