﻿using Pokemon.Team.Builder.Interfaces;
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
                    MonsNo = Id,
                    FormNo = FormNo
                };
            }

            set
            {
                if (value != null)
                {
                    Id = value.MonsNo; 
                }

                FormNo = value?.FormNo;
                Name = value?.Name;
            }
        }

        public string FormNo { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
        public float UsageRate { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals(ITeamMate otherMate)
        {
            if (otherMate == null)
            {
                return false;
            }

            return Identifier == otherMate.Identifier;
        }

        public override bool Equals(object obj)
        {
            var otherMate = obj as SmogonTeamMate;

            if (otherMate == null)
            {
                return false;
            }

            return Equals(otherMate);
        }
    }
}
