using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonPokemonStats : IPokemonInformation, IEquatable<SmogonPokemonStats>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string FormNo { get; set; }
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
                return new PokemonIdentifier
                {
                    Name = Name,
                    MonsNo = Id,
                    FormNo = FormNo
                };
            }

            set
            {
                Id = value.MonsNo;
                Name = value.Name;
                FormNo = value.FormNo;
            }
        }

        public string GetName()
        {
            return Name;
        }

        public List<T> SetRankings<T>(IEnumerable<T> rankables, Func<T, double> selector) where T : IRankable
        {
            var list = rankables
                .OrderByDescending(selector)
                .ToList();

            list
                .ForEach(ability => ability.Ranking = list.IndexOf(ability));

            return list;
        }

        public IEnumerable<IAbility> GetAbilities()
        {
            Abilities = SetRankings(Abilities, ability => ability.UsageRate);

            return Abilities;
        }

        public IEnumerable<ICounter> GetCounters()
        {
            ChecksAndCounters = SetRankings(ChecksAndCounters, check => (double) check.Statistics?.FirstOrDefault());

            return ChecksAndCounters;
        }

        public IEnumerable<IHappiness> GetHappiness()
        {
            Happiness = SetRankings(Happiness, happy => happy.UsageRate);

            return Happiness;
        }

        public IEnumerable<IItem> GetItems()
        {
            Items = SetRankings(Items, item => item.UsageRate);

            return Items;
        }

        public IEnumerable<IMove> GetMoves()
        {
            Moves = SetRankings(Moves, move => move.UsageRate);

            return Moves;
        }

        public IEnumerable<ISpread> GetSpreads()
        {
            Spreads = SetRankings(Spreads, spread => spread.UsageRate);

            return Spreads;
        }

        public IEnumerable<INature> GetNatures()
        {
            Spreads = SetRankings(Spreads, nature => nature.UsageRate);

            return Spreads;
        }

        public IEnumerable<ITeamMate> GetTeamMates()
        {
            TeamMates = SetRankings(TeamMates, mate => mate.UsageRate);

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

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals(SmogonPokemonStats otherStat)
        {
            if (otherStat == null)
            {
                return false;
            }

            return Name.Equals(otherStat.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var otherStats = obj as SmogonPokemonStats;

            if (otherStats == null)
            {
                return false;
            }

            return Equals(otherStats);
        }
    }
}
