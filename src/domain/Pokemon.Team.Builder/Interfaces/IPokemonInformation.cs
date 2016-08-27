using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Interfaces
{
    public interface IPokemonInformation : IPokemonIdentifiable
    {
        string GetName();

        string GetType1();
        string GetType2();

        IEnumerable<IAbility> GetAbilities();
        IEnumerable<ICounter> GetCounters();
        IEnumerable<IHappiness> GetHappiness();
        IEnumerable<IItem> GetItems();
        IEnumerable<IMove> GetMoves();
        IEnumerable<INature> GetNatures();
        IEnumerable<ISpread> GetSpreads();
        IEnumerable<ITeamMate> GetTeamMates();
    }
}
