using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.PokeAPI
{
    public class AbilityOverviewResponse
    {
        public class Result
        {
            public string Url { get; set; }
            public string Name { get; set; }
        }

        public int Count { get; set; }
        public object Previous { get; set; }
        public List<Result> Results { get; set; }
        public string Next { get; set; }
    }
}
