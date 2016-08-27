using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Team.Builder.Logic
{
	[Serializable]
    public class Ability
    {
		[Serializable]
        public class Generation
        {
            public string url { get; set; }
            public string name { get; set; }
        }

		[Serializable]
        public class Pokemon2
        {
            public string url { get; set; }
            public string name { get; set; }
        }

		[Serializable]
        public class Pokemon
        {
            public int slot { get; set; }
            public bool is_hidden { get; set; }
            public Pokemon2 pokemon { get; set; }
        }

		[Serializable]
        public class Language
        {
            public string url { get; set; }
            public string name { get; set; }
        }

		[Serializable]
        public class EffectEntry
        {
            public string short_effect { get; set; }
            public string effect { get; set; }
            public Language language { get; set; }
        }

		[Serializable]
        public class Name
        {
            public string name { get; set; }
            public Language language { get; set; }
        }

		[Serializable]
        public class VersionGroup
        {
            public string url { get; set; }
            public string name { get; set; }
        }

		[Serializable]
        public class FlavorTextEntry
        {
            public string flavor_text { get; set; }
            public Language language { get; set; }
            public VersionGroup version_group { get; set; }
        }

        // public List<object> effect_changes { get; set; }
        public string name { get; set; }
        public Generation generation { get; set; }
        public List<Pokemon> pokemon { get; set; }
        public bool Is_Main_Series { get; set; }
        public List<EffectEntry> Effect_Entries { get; set; }
        public List<Name> Names { get; set; }
        public List<FlavorTextEntry> Flavor_Text_Entries { get; set; }
        public int Id { get; set; }

        public string GetName(string language)
        {
            if (Names == null)
            {
                return string.Empty;
            }

            return Names.SingleOrDefault(n => n.language != null && n.language.name == language)?.name ?? name;
        }
    }
}
