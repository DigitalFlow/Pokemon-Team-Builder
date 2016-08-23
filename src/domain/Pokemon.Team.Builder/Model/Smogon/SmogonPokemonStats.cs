using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonPokemonStats : IPokemonInformation
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

        public PokemonIdentifier Identifier
        {
            get
            {
                return new PokemonIdentifier(Name);
            }

            set
            {
                Name = value.Name;
            }
        }

        public string GetName()
        {
            return Name;
        }

        public IEnumerable<IAbility> GetAbilities()
        {
            return Abilities;
        }

        public IEnumerable<ICounter> GetCounters()
        {
            return ChecksAndCounters;
        }

        public IEnumerable<IHappiness> GetHappiness()
        {
            return Happiness;
        }

        public IEnumerable<IItem> GetItems()
        {
            return Items;
        }

        public IEnumerable<IMove> GetMoves()
        {
            return Moves;
        }

        public IEnumerable<INature> GetNatures()
        {
            return Spreads;
        }

        public IEnumerable<ISpread> GetSpreads()
        {
            return Spreads;
        }

        public IEnumerable<ITeamMate> GetTeamMates()
        {
            return TeamMates;
        }

        public string GetType1()
        {
            return null;
        }

        public string GetType2()
        {
            return null;
        }
    }
}
