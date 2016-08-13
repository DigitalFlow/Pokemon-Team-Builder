using System;
using System.Linq;
using System.Collections.Generic;
using Pokemon.Team.Builder;

namespace Pokemon.Team.Builder
{
    public class Pokemon
    {
        public int Id { get; set; }
		public List<Name> Names { get; set; }
		public List<Variety> Varieties {get; set; }
        public string Url { get; set; }
		public string Image { get; set; }

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
