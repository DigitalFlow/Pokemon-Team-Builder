using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Interfaces
{
    public interface ITeamMate : IPokemonIdentifiable, IRankable, IEquatable<ITeamMate>
    {
        string Name { get; set; }
    }
}
