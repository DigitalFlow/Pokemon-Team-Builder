using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    public class SmogonTeamMate : ITeamMate
    {
        public PokemonIdentifier Identifier
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name { get; set; }
        public int Ranking { get; set; }
        public float UsageRate { get; set; }

        public bool Equals(ITeamMate other)
        {
            throw new NotImplementedException();
        }
    }
}
