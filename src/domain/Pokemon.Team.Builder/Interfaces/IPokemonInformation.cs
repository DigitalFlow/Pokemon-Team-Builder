using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Interfaces
{
    public interface IPokemonInformation : IPokemonIdentifiable
    {
        string Name { get; set; }
        string Type1 { get; set; }
        string Type2 { get; set; }

        IEnumerable<IAbility> Abilities { get; set; }
        IEnumerable<ICounter> Counters { get; set; }
        IEnumerable<IHappiness> Happiness { get; set; }
        IEnumerable<IItem> Items { get; set; }
        IEnumerable<IMove> Moves { get; set; }
        IEnumerable<INature> Natures { get; set; }
        IEnumerable<ISpread> Spreads { get; set; }
        IEnumerable<ITeamMate> TeamMates { get; set; }
    }
}
