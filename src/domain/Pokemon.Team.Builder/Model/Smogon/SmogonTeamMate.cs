using Pokemon.Team.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Model.Smogon
{
    [Serializable]
    public class SmogonTeamMate : ITeamMate
    {
        public PokemonIdentifier Identifier
        {
            get
            {
                return new PokemonIdentifier
                {
                    Name = Name,
                    MonsNo = Id
                };
            }

            set
            {
                if (value != null)
                {
                    Id = value.MonsNo; 
                }

                Name = value?.Name;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
        public float UsageRate { get; set; }

        public bool Equals(ITeamMate other)
        {
            throw new NotImplementedException();
        }
    }
}
