using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonAbility : IAbility
    {
        public string Name { get; set; }
        public double UsageRate { get; set; }
        public int Ranking { get; set; }
    }
}
