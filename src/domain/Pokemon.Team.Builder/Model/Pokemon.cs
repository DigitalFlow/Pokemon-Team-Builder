using System;
using System.Linq;
using System.Collections.Generic;
using Pokemon.Team.Builder;
using System.Xml.Serialization;

namespace Pokemon.Team.Builder
{
	[Serializable]
	[XmlRoot("Pokemon")]
    public class Pokemon : IPokemonIdentifiable
    {
        public int Id { get; set; }
		public List<Name> Names { get; set; }
		public List<Variety> Varieties {get; set; }
		public List<FlavorTextEntry> TextEntries { get; set; }
        public string Url { get; set; }
		public string Image { get; set; }

        public PokemonIdentifier Identifier
        {
            get
            {
                return new PokemonIdentifier
                {
                    MonsNo = Id,
                    Name = GetName("en")
                };
            }

            set
            {
                Id = value.MonsNo;
            }
        }

        public string GetName(string language) {
            if (Names == null)
            {
                return string.Empty;
            }

			return Names.SingleOrDefault(n => n.language != null && n.language.name == language)?.name ?? string.Empty;
		}

        public override string ToString()
        {
			return $"{Id} - {Names.SingleOrDefault(n => n.language.name == "en")}";
        }
    }
}
