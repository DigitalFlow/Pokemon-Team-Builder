using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
    public class Team
    {
        public List<IPokemonInformation> TeamMembers { get; set; }
        public DateTime CreatedOn { get; set; }

        public Team ()
        {
            CreatedOn = DateTime.UtcNow;
            TeamMembers = new List<IPokemonInformation>();
        }

        public Team(List<IPokemonInformation> teamMembers) : this()
        {
            TeamMembers = teamMembers;
        }
    }
}
